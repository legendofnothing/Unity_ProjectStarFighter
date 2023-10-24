using Effect;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript {
    public class Enemy : MonoBehaviour {
        [TitleGroup("Config")] 
        public float hp = 100;
        [ReadOnly] public float currentHp;
        [ReadOnly] public bool hasDied;

        [TitleGroup("Options")]
        public bool useExplosionEffect = true;

        [ShowIf(nameof(useExplosionEffect))] [TitleGroup("Explosion Config")] 
        public GameObject explosionEffect;
        [ShowIf(nameof(useExplosionEffect))] public Vector3 explosionSize = Vector3.one;

        private void Start() {
            currentHp = hp;
        }

        public void TakeDamage(float amount) {
            if (hasDied) return;
            currentHp -= amount;
            if (!(currentHp <= 0)) return;
            hasDied = true;
            Death();
        }

        private void Death() {
            if (useExplosionEffect && explosionEffect != null) {
                var explosionInst = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosionInst.GetComponent<EffectBase>().Init(explosionSize);
            }
            Destroy(gameObject);
        }
    }
}
