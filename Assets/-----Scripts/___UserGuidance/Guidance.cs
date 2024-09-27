using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.UserGuidacnce{
    public class Guidance : MonoBehaviour{

        public virtual void CompleteGuidance() {
            UserGuidanceMgr.Instance.GuideComplete();
            Destroy(gameObject);
        }

    }
}
