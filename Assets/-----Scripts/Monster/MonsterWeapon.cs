using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleTurtle.Turtle;

namespace LittleTurtle.Enemy {

    public class MonsterWeapon : MonoBehaviour{

        public float destroyTime;
        public bool PlayDestroyAnimation;

        private bool isAnimationFinished = false;
        private List<Collider2D> damagedCollisions = new List<Collider2D>();
        private Monster monster;

        private PlayerCtrl playerCtrl;

        private void Start() {
            playerCtrl = PlayerCtrl.Instance;
        }


        private void Update() {
            transform.rotation = Quaternion.identity;
        }

        public void SetInfo(Monster monster) {
            this.monster = monster;
        }

        public void _AttackFinished() {
            isAnimationFinished = true;

            if (monster != null) {
                monster.AttackFinished();
            }

            StartCoroutine(DestroyObject());
        }


        public void _Destroy() {
            Destroy(transform.parent.gameObject);
        }
        IEnumerator DestroyObject() {
            yield return new WaitForSeconds(destroyTime);

            if (PlayDestroyAnimation) {
                GetComponent<Animator>().SetTrigger("Destroy");
            }
            else {
                _Destroy();
            }
        }


        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag == "Player") {
                if (!damagedCollisions.Contains(collision) & !isAnimationFinished) {
                    damagedCollisions.Add(collision);
                    playerCtrl.GetHurt(monster.data.Damage);
                }
            }

            if (collision.tag == "Turtle") {
                if (!damagedCollisions.Contains(collision) & !isAnimationFinished) {
                    damagedCollisions.Add(collision);
                    TurtleCtrl.Instance.GetHurt(monster.data.Damage);
                }
            }
        }
    }
}
