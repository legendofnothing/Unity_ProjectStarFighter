using EnemyScript;
using UnityEngine;

namespace Combat.ProjectileScript {
    public class BasicProjectile : Projectile {
        protected override void OnImpact(Collider2D obj) {
            if (obj.gameObject.TryGetComponent<Enemy>(out var enemy)) {
                enemy.TakeDamage(_setting.damage);
            }
            
            Destroy(gameObject);
        }
    }
}