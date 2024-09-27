using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UI {
    public class DetectTouch : MonoBehaviour{

        public System.Action action_GetTouch;
        private int touchID = 0;

        void Update(){
            if (action_GetTouch == null) {
                return;
            }
            
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
                if (Input.GetMouseButtonUp(0)) {
                    action_GetTouch?.Invoke();
                    action_GetTouch = null;
                    Destroy(this);
                }
            }
            else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
                foreach (Touch touch in Input.touches) {
                    if (touch.phase == TouchPhase.Began) {
                        touchID = touch.fingerId;
                    }
                    if (touch.phase == TouchPhase.Ended && touch.fingerId == touchID) {
                        action_GetTouch?.Invoke();
                        action_GetTouch = null;
                        Destroy(this);
                    }
                }

            }
        }
    }
}
