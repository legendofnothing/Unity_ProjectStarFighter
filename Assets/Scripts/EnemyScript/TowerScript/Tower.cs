using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Events;
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
        public float powerAmount;
        [Space] 
        public LayerMask detectLayer;
        public List<DeployConfig> deployConfig = new();
        [Space] public List<Turret> turrets = new();

        private bool _isPlayerInRange;
        private bool _canSpawn = true;
        private BehaviorTree.BehaviorTree _bt;

        private void Start() {
            
        }

        private void Update() {
            enemyBehaviors.LookAt(Vector3.zero, angularSpeed);
            enemyBehaviors.Fly(speed, transform.right);
            DetectPlayer();

            if (_isPlayerInRange && _canSpawn) {
                StartCoroutine(SpawnShips());
            }
        }

        private void DetectPlayer() {
            var hit = Physics2D.OverlapCircle(transform.position, spawnUnitRadius, detectLayer);
            _isPlayerInRange = hit;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnUnitRadius);
        }

        private IEnumerator SpawnShips() {
            _canSpawn = false;
            foreach (var deploys in deployConfig) {
                Instantiate(deploys.enemyToSpawn, deploys.point.position, deploys.point.rotation);
            }
            yield return new WaitForSeconds(50f);
            _canSpawn = true;
        }

        public void RemoveTurret(Turret turret) {
            if (turrets.Contains(turret)) {
                turrets.Remove(turret);
            }
        }

        protected override void Death() {
            base.Death();
            this.FireEvent(EventType.OnTowerDestroyed, this);
            if (turrets.Count > 0) {
                foreach (var turret in turrets) {
                    turret.GetComponent<Enemy>().TakeDamage(9999);
                }
            }
        }
    }
}
