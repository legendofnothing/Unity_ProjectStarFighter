using Core;
using UnityEngine;

namespace Effect {
    public class EngineEffect : MonoBehaviour {
        public Rigidbody2D rb;
        public float maxSpeed;
        public float ratioToMaxThrust;
        
        private SpriteRenderer _renderer;

        private void Start() {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            if (rb) {
                var t = maxSpeed * ratioToMaxThrust;
                var r = Mathf.Clamp(rb.velocity.magnitude, 0, t) / t;
                _renderer.color = new Color(1, 1, 1, r);
            }
        }
    }
}