using PlayerScript;
using Scripts.Core;
using UnityEngine;

namespace EnemyScript.v2.BehaviorTree.Variations.EnemyRam {
    public class RamBehavior : MonoBehaviour {
        public Enemy enemy;
        public EnemyBehaviors enemyBehaviors;
        public float damage;

        private void Update() {
            enemyBehaviors.LookAt(Player.Instance.transform.position, enemy.angularSpeed);
            enemyBehaviors.FlyForward(enemy.speed);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.TryGetComponent<Player>(out var p)) {
                p.TakeDamage(damage);
                enemy.TakeDamage(9999f);
            }
        }
    }
}