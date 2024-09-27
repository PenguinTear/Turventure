using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.Turtle{

    [CreateAssetMenu(menuName = "--Scriptable Object / Turtle Data")]
    public class TurtleData_ScriptableObject : ScriptableObject {

        [Header("Basic")]
        public int ID;
        public string Name;
        public GameObject prefab;

        [Header("ability")]
        public int level;
        public float MaxHP;
        public float MaxGrowth;
        public float speed;

    }
}
