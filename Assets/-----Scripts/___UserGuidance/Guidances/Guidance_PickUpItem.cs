using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UserGuidacnce{

    public class Guidance_PickUpItem : Guidance{

        

        private void Start() {
            UI.Canvas_Game.Instance.joyStick_Movement_Gameobject.gameObject.SetActive(true);

            Vector3 v3 = new Vector3(PlayerCtrl.Instance.transform.position.x + 3, 
                                        PlayerCtrl.Instance.transform.position.y, 1);
            Instantiate(FindItemByID.FindItem(2).prefab, v3, Quaternion.identity);

            Inventory.Bag.action_AddItem += CompleteGuidance;
        }

        public override void CompleteGuidance() {
            Inventory.Bag.action_AddItem -= CompleteGuidance;
            base.CompleteGuidance();
        }

    }

}
