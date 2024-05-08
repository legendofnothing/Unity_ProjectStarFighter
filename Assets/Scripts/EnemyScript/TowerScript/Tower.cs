using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Events;
using EnemyScript.Commander;
using Sirenix.OdinInspector;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace EnemyScript.TowerScript {
    public class Tower : Enemy {
        [Serializable]
        public struct DeployConfig {
            public Transform point;
            public GameObject enemyToSpawn;
        }
        
        [TitleGroup("Tower Config")] 
        public float turretAttackRadius;
        public float spawnUnitRadius;
        [Space] 
        public LayerMask detectLayer;
        public List<DeployConfig> deployConfig = new();
        [Space] public List<Turret> turrets = new();

        [ReadOnly] public bool isPlayerInRange;
        [ReadOnly] public bool canSpawn = true;
        private BehaviorTree.BehaviorTree _bt;

        private void Update() {
            OnUpdate();
        }

        protected void DetectPlayer() {
            var hit = Physics2D.OverlapCircle(transform.position, spawnUnitRadius, detectLayer);
            isPlayerInRange = hit;
        }

        protected virtual void OnUpdate() {
            enemyBehaviors.LookAt(Vector3.zero, angularSpeed);
            enemyBehaviors.Fly(speed, transform.right);
            DetectPlayer();

            if (isPlayerInRange && canSpawn) {
                StartCoroutine(SpawnShips());
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnUnitRadius);
        }

        public IEnumerator SpawnShips() {
            canSpawn = false;
            foreach (var deploys in deployConfig) {
                Instantiate(deploys.enemyToSpawn, deploys.point.position, deploys.point.rotation);
            }
            yield return new WaitForSeconds(50f);
            canSpawn = true;
        }

        public List<Troop> SpawnTroopDirectly() {
            var tempList = new List<Troop>();
            foreach (var deploys in deployConfig) {
                var inst = Instantiate(deploys.enemyToSpawn, deploys.point.position, deploys.point.rotation);
                tempList.Add(inst.GetComponent<Troop>());
            }

            return tempList;
        }

        protected override void Death() {
            base.Death();
            this.FireEvent(EventType.OnTowerDestroyed, this);
            if (turrets.Count > 0) {
                foreach (var turret in turrets.Where(turret => turret)) {
                    turret.GetComponent<Enemy>().TakeDamage(9999);
                }
            }
        }
    }
}
