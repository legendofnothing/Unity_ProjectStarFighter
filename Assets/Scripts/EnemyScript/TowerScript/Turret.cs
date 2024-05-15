using PlayerScript;
using UnityEngine;

namespace EnemyScript.TowerScript {
    public class Turret : Enemy {
        [Space]
        public Tower tower;
        public Transform followPoint;

        protected override void Update() {
            base.Update();
            if (!tower) return;

            transform.position = followPoint.position;

            var hit = Physics2D.OverlapCircle(transform.position, tower.turretAttackRadius, tower.detectLayer);
            if (hit) {
                enemyBehaviors.LookAt(Player.Instance.PlayerPos, 2);
                if (GetDotToPlayer > 0.98f) {
                    enemyBehaviors.FireWeapon();
                }
            }
        }
        
        private void OnDrawGizmos() {
            if (tower) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, tower.turretAttackRadius);
            }
        }
    }
}
