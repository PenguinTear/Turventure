using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleTurtle.UI;

namespace LittleTurtle.UserGuidacnce {

    public class UserGuidanceMgr : MonoBehaviour{

        public static UserGuidanceMgr Instance;

        public GameObject[] GuidanceList;

        private Canvas_Game canvas_Game;
        private int guidanceProgress;

        private void Awake() {

            if (System_.MyGameManager.isLoadingSaved) {
                Destroy(gameObject);
            }

            Instance = this;
            Time.timeScale = 0;
        }

        void Start(){
            canvas_Game = Canvas_Game.Instance;
            System_.MyGameManager.isGuiding = true;

            canvas_Game.HPar_Gameobject.gameObject.SetActive(false);
            canvas_Game.turtleButton.gameObject.SetActive(false);
            canvas_Game.bagButton.gameObject.SetActive(false);
            canvas_Game.joyStick_Attack_Gameobject.gameObject.SetActive(false);
            canvas_Game.joyStick_Movement_Gameobject.gameObject.SetActive(false);
            

            guidanceProgress = 0;
            Instantiate(GuidanceList[guidanceProgress], transform);
        }

        public void SkipGuidance() {
            Time.timeScale = 1;
            guidanceProgress = GuidanceList.Length;
            GuideComplete();
            System_.MyGameManager.isGuiding = false;
        }

        public void GuideComplete() {
            if (guidanceProgress < GuidanceList.Length - 1) {
                guidanceProgress++;
                Instantiate(GuidanceList[guidanceProgress], transform);
            }
            else {
                canvas_Game.joyStick_Attack_Gameobject.gameObject.SetActive(true);
                canvas_Game.joyStick_Movement_Gameobject.gameObject.SetActive(true);
                canvas_Game.bagButton.SetActive(true);
                canvas_Game.turtleButton.SetActive(true);
                canvas_Game.HPar_Gameobject.SetActive(true);

                Time.timeScale = 1;
                Destroy(gameObject);
            }

        }

    }
}
