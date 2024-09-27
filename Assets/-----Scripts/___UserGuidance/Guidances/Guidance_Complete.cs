using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UserGuidacnce {

    public class Guidance_Complete : Guidance{

        public GameObject Panel_1;
        public GameObject Panel_2;

        private void Start() {
            Time.timeScale = 0;
            StartCoroutine(test());
        }

        IEnumerator test() {
            yield return 10;
            Panel_1.SetActive(true);
        }
        int touchID = 0;
        private void Update() {

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
                if (Input.GetMouseButtonUp(0)) {
                    if (Panel_1.activeSelf) {
                        Panel_1.SetActive(false);
                        Panel_2.SetActive(true);
                    }
                    else if (Panel_2.activeSelf) {
                        Panel_2.SetActive(false);
                        CompleteGuidance();
                    }
                }
            }
                
            else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
                
                foreach (Touch touch in Input.touches) {
                    if (touch.phase == TouchPhase.Began) {
                        touchID = touch.fingerId;
                    }

                    if (touch.phase == TouchPhase.Ended && touch.fingerId == touchID) {
                        if (Panel_1.activeSelf) {
                            Panel_1.SetActive(false);
                            Panel_2.SetActive(true);
                        }
                        else if (Panel_2.activeSelf) {
                            Panel_2.SetActive(false);
                            CompleteGuidance();
                        }
                    }
                }
            }


        }
    }
}
