using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleTurtle.System_;
using System.Threading.Tasks;
using LittleTurtle.UI;

namespace LittleTurtle.Turtle {
    public class TurtleMgr : MonoBehaviour, IDatapersistence{

        public ParticleSystem particle_Upgrade;
        private TurtleInfoPanel turtleInfoPanel;
        public static TurtleMgr Instance;
        public Dictionary<int, TurtleData_ScriptableObject> AllTurtleData = new Dictionary<int, TurtleData_ScriptableObject>();
        public TurtleCtrl turtleCtrl;

        private void Awake() {
            if (!Instance) Instance = this;
        }

        private void Start() {
            turtleInfoPanel = Canvas_Game.Instance.turtleInfoPanel;
            AllTurtleData = new Dictionary<int, TurtleData_ScriptableObject>();
            
            Object[] objects = Resources.LoadAll("Turtle", typeof(TurtleData_ScriptableObject));
            foreach (var item in objects) {
                TurtleData_ScriptableObject data = item as TurtleData_ScriptableObject;
                AllTurtleData.Add(data.ID, data);
            }
        }

        public void TurtleUpgrade(TurtleCtrl turtleCtrl) {

            //int[] vs = { };
            //turtleCtrl.Dict_ConsumedItem
            //Mathf.Max()

            InstantiateTurtle(1);
        }

        public async void InstantiateTurtle() {
            Vector3 pos = new Vector3(PlayerCtrl.Instance.transform.position.x - 2,
                                        PlayerCtrl.Instance.transform.position.y, 0);
            Instantiate(particle_Upgrade, pos, Quaternion.identity);
            
            await Task.Delay(500);

            turtleCtrl = Instantiate(AllTurtleData[0].prefab, pos, Quaternion.identity).GetComponent<TurtleCtrl>();
            turtleCtrl.InitTurtle();
            turtleInfoPanel.turtleCtrl = turtleCtrl;
            turtleInfoPanel.InitTurtleData();
        }

        public async void InstantiateTurtle(int turtleID) {
            GameObject turtleGameObject = turtleCtrl.gameObject;

            Vector3 pos = turtleGameObject.transform.position;
            Quaternion rotation = turtleGameObject.transform.rotation;
            Instantiate(particle_Upgrade, pos, Quaternion.identity);

            await Task.Delay(500);
            if (turtleGameObject != null) Destroy(turtleGameObject);
            turtleCtrl = Instantiate(AllTurtleData[turtleID].prefab, pos,rotation).GetComponent<TurtleCtrl>();
            turtleCtrl.InitTurtle();
            turtleInfoPanel.turtleCtrl = turtleCtrl;
            turtleInfoPanel.InitTurtleData();
        }


        #region = Data =
        public void LoadData(GameData data) {
            TurtleData d = data.turleData;

            turtleCtrl = Instantiate(AllTurtleData[d.TurtleID].prefab).GetComponent<TurtleCtrl>();
            turtleCtrl.State = d.state;

            turtleInfoPanel.turtleCtrl = turtleCtrl;
            turtleCtrl.InitTurtle(d);
        }

        public void SaveData(ref GameData data) {
            ref TurtleData d = ref data.turleData;

            d.TurtleID = turtleCtrl.data_turtle.ID;
            d.HP = turtleCtrl.HP;
            d.Growth = turtleCtrl.Growth;
            d.state = turtleCtrl.State;

            d.position = turtleCtrl.gameObject.transform.position;
            d.rotation = turtleCtrl.gameObject.transform.rotation;

            d.Dict_ConsumedItem = turtleCtrl.Dict_ConsumedItem;
        }
        #endregion


    }
}
