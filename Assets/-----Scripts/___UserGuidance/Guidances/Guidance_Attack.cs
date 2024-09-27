using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UserGuidacnce {

    public class Guidance_Attack : Guidance {

        private UI.Canvas_Game canvas_Game;

        void Start() {
            canvas_Game = UI.Canvas_Game.Instance;
            canvas_Game.joyStick_Attack_Gameobject.SetActive(true);
            canvas_Game.joyStick_Movement_Gameobject.gameObject.SetActive(false);
            PlayerCtrl.Instance.action_AttackObserver += CompleteGuidance;
        }

        public override void CompleteGuidance() {
            PlayerCtrl.Instance.action_AttackObserver -= CompleteGuidance;
            canvas_Game.joystick_Attack.isCooldowning = false;
            canvas_Game.joyStick_Attack_Gameobject.gameObject.SetActive(false);
            base.CompleteGuidance();
        }

    }
}
