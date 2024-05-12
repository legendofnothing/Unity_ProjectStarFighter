using System.Linq;
using Combat;
using Combat.ProjectileScript;
using PlayerScript;
using UnityEngine;

namespace EnemyScript {
    public class EnemyRadar : MonoBehaviour {
        public float detectRadius = 5f;
        public LayerMask detectLayer;
        private Collider2D _currentHit;
        public Collider2D CurrentHit => _currentHit;
        

        private void Update() {
            var hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, detectLayer);
            if (hits.Length > 0) {
                _currentHit = hits.OrderBy(x => (x.gameObject.transform.position - transform.position).sqrMagnitude).ToArray()[0];
                if (!_currentHit.TryGetComponent<Projectile>(out var p)) return;
                
                if (p.owner is Enemy) {
                    _currentHit = null;
                }
            }
            else {
                _currentHit = null;
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
            if (_currentHit) {
                Gizmos.DrawWireCube(_currentHit.transform.position, Vector3.one);
            }
        }
    }
}