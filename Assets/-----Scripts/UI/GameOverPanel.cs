using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleTurtle.System_;

namespace LittleTurtle.UI{
    public class GameOverPanel : MonoBehaviour{

        public TMPro.TMP_Text text;
        private int touchID;

        public void SetText(string s) {
            text.text = s;
            print(s);
        }

        void Awake() {
            Time.timeScale = 0;
        }

        private int forBug = 0;
        void Update(){

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
                if (Input.GetMouseButtonUp(0)){
                    print(forBug);
                    if (forBug == 1)
                    {
                        MyGameManager.Instance.ExitGame();
                        MyGameManager.Instance.DeleteGameData();
                        Destroy(this);
                    }
                    forBug = 1;

                }
            }
            else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
                foreach (Touch touch in Input.touches) {
                    if (touch.phase == TouchPhase.Began) {
                        touchID = touch.fingerId;
                    }
                    if (touch.phase == TouchPhase.Ended && touch.fingerId == touchID) {

                        if (forBug == 1)
                        {
                            MyGameManager.Instance.ExitGame();
                            MyGameManager.Instance.DeleteGameData();
                            Destroy(this);
                        }

                        forBug = 1;
                        
                    }
                }

            }

        }
    }
}
