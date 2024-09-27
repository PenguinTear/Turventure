using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle.Enemy{

    public class Monster_3_Snake : Monster{

        public GameObject weapon;

        public override void Attack() {
            base.Attack();

            Attack_General(weapon);
        }
    }
}
