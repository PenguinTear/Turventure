using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UserGuidacnce{

    public class Guidance_Move : Guidance{

        private Vector3 ortiginPos;
        private Transform playerTransform;

        void Start(){
            UI.Canvas_Game.Instance.joyStick_Movement_Gameobject.gameObject.SetActive(true);
            playerTransform = PlayerCtrl.Instance.transform;
            ortiginPos = playerTransform.position;
        }

        void Update(){

            if (Vector3.Distance(playerTransform.position, ortiginPos) > 1.5f) {
                UI.Canvas_Game.Instance.joystick_Movement.PointerUp();
                CompleteGuidance();
            }
        }

    }
}
