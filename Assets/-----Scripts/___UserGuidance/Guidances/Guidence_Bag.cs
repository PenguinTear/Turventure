using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UserGuidacnce{

    public class Guidence_Bag : Guidance{

        public GameObject Panel_1;
        public GameObject Panel_2;
        public GameObject Panel_3;

        private UI.Canvas_Game canvas_Game;

        private void Start() {
            Panel_1.SetActive(true);
            Panel_2.SetActive(false);

            canvas_Game = UI.Canvas_Game.Instance;

            canvas_Game.bagButton.gameObject.SetActive(true);
            canvas_Game.action_BagOpen += OpenBag;
        }


        // 1
        private void OpenBag() {
            canvas_Game.action_BagOpen -= OpenBag;
            canvas_Game.itemInfoPanel.action_OpenItemInfoPanel += OpenItemInfoPanel;

            Panel_1.SetActive(false);
            Panel_2.SetActive(true);
        }

        // 2
        private void OpenItemInfoPanel() {
            canvas_Game.itemInfoPanel.action_OpenItemInfoPanel -= OpenItemInfoPanel;

            // temp
            System_.MyGameManager.isGuiding = false;

            Panel_2.SetActive(false);
            Panel_3.SetActive(true);
            StartCoroutine(test());
        }

        IEnumerator test() {
            yield return new WaitForSeconds(1);
            Panel_3.GetComponent<UI.DetectTouch>().action_GetTouch += CompleteGuidance;
        }

        public override void CompleteGuidance() {
            base.CompleteGuidance();
        }


    }
}
