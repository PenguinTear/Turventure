using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UserGuidacnce {

    public class StartPanel : Guidance{

        int touchID;

        void Update() {

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
                if (Input.GetMouseButtonUp(0)) {
                    CompleteGuidance();
                }
            }
            else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {

                foreach (Touch touch in Input.touches) {
                    if (touch.phase == TouchPhase.Began) {
                        touchID = touch.fingerId;
                    }

                    if (touch.phase == TouchPhase.Ended && touch.fingerId == touchID) {
                        CompleteGuidance();
                    }
                }
            }
        }
        public void _SkipGuide() {
            Turtle.TurtleMgr.Instance.InstantiateTurtle();
            UserGuidanceMgr.Instance.SkipGuidance();
        }

    }
}
