using Effect;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript {
    public class Enemy : MonoBehaviour {
        [TitleGroup("Config")] public float hp = 100;
        [ReadOnly] public float currentHp;
        [ReadOnly] public bool hasDied;
        [Space] public float speed;
        public float minimumSpeed;
        [Space] public float minimumAngularSpeed;
        public float angularSpeed;

        [ReadOnly] public float currentSpeed;
        [ReadOnly] public float currentAngularSpeed;

        [TitleGroup("Options")] public bool useExplosionEffect = true;

        [ShowIf(nameof(useExplosionEffect))] [TitleGroup("Explosion Config")]
        public GameObject explosionEffect;

        [ShowIf(nameof(useExplosionEffect))] public Vector3 explosionSize = Vector3.one;

        private void Start() {
            currentHp = hp;
            currentSpeed = speed;
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

        public float GetDotToPlayer => Vector2.Dot(transform.up,
            (PlayerScript.Player.Instance.transform.position - transform.position).normalized);

        public float GetDotToPoint(Vector2 pos) {
            return  Vector2.Dot(transform.up,
                (pos - (Vector2) transform.position).normalized);
        }
        
        public float GetDistanceToPlayer => Vector2.Distance(transform.position,
            PlayerScript.Player.Instance.transform.position);
    }
}


