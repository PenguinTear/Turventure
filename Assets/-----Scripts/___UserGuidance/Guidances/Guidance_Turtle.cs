using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UserGuidacnce {

    public class Guidance_Turtle : Guidance {

        public GameObject Panel_1;
        public GameObject Panel_2;
        public GameObject Panel_3;

        private UI.Canvas_Game canvas_Game;

        async void Start() {
            canvas_Game = UI.Canvas_Game.Instance;

            Turtle.TurtleMgr.Instance.InstantiateTurtle();
            
            Panel_1.SetActive(true);

            await System.Threading.Tasks.Task.Delay(500);
            Panel_1.GetComponent<UI.DetectTouch>().action_GetTouch += Action_Panel_1;
        }


        private void Action_Panel_1() {
            Panel_1.SetActive(false);
            Panel_2.SetActive(true);

            canvas_Game.turtleButton.gameObject.SetActive(true);
            Panel_1.GetComponent<UI.DetectTouch>().action_GetTouch -= Action_Panel_1;

            StartCoroutine(TEST());
        }

        IEnumerator TEST() {
            yield return 10;
            canvas_Game.action_OpenTurtlePanel += Action_Panel_2;
        }

        // open turtle info panel
        private void Action_Panel_2() {
            Panel_2.SetActive(false);
            Panel_3.SetActive(true);

            canvas_Game.action_OpenTurtlePanel -= Action_Panel_2;
            canvas_Game.turtleInfoPanel.gameObject.
                GetComponent<UI.UI_PanelCloseDetect>().action_ClosePanel += CompleteGuidance;
        }


        public override void CompleteGuidance() {
            canvas_Game.turtleInfoPanel.gameObject.
                GetComponent<UI.UI_PanelCloseDetect>().action_ClosePanel -= CompleteGuidance;

            base.CompleteGuidance();
        }

    }

}
