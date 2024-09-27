using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleTurtle{
    public class MonsterSpawner : MonoBehaviour{

        public GameObject MonsterGroup;
        public Transform[] SpawnPos;

        private Coroutine coroutine_SpawnMonster;

        GameObject MonsterPrefab_HugeRoach;
        GameObject MonsterPrefab_GiantRoach;
        GameObject MonsterPrefab_Snake;

        private void Start() {
            MonsterPrefab_HugeRoach = FindItemByID.GetMonsterByID(1).prefab;
            MonsterPrefab_GiantRoach = FindItemByID.GetMonsterByID(2).prefab;
            MonsterPrefab_Snake = FindItemByID.GetMonsterByID(3).prefab;
        }

        private bool Spawning = false;
        void Update(){
            
            if(System_.SaveDataMgr.monsterList.Count == 0 && !Spawning &! System_.MyGameManager.isGuiding){
                if (coroutine_SpawnMonster == null) {
                    coroutine_SpawnMonster = StartCoroutine(SpawnMonster());
                    Spawning = true;
                }
            }
        }

        private int SpawnCount;
        IEnumerator SpawnMonster() {
            if (SpawnCount == 0) {
                yield return new WaitForSeconds(0);
            }
            else if(SpawnCount > 0 && SpawnCount <= 2) {
                print("spawn 2 ");
                yield return new WaitForSeconds(Random.Range(2, 6));
            }
            else {
                yield return new WaitForSeconds(Random.Range(10, 16));
            }
            
            if (SpawnCount != 0) {
                _SpawnMonster();
            }
            else {
                if (!System_.MyGameManager.isLoadingSaved) {
                    Instantiate(MonsterGroup);
                }
            }
            SpawnCount++;
            Spawning = false;

            coroutine_SpawnMonster = null;
        }

        private void _SpawnMonster() {
            int amount = Random.Range(1, 4);

            for (int i = 0; i < amount; i++) {
                Vector3 pos = SpawnPos[Random.Range(0, SpawnPos.Length)].position;

                int r = Random.Range(1, 101);
                if (r <= 20) {
                    Instantiate(MonsterPrefab_Snake, pos, Quaternion.identity);
                }
                else if (r > 20 && r <= 50) {
                    Instantiate(MonsterPrefab_GiantRoach, pos, Quaternion.identity);
                }
                else {
                    Instantiate(MonsterPrefab_HugeRoach, pos, Quaternion.identity);
                }

            }
        }

    }
}
