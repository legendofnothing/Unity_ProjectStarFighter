using Core;
using UnityEngine;

namespace Scenes.PredictionTest {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Target : MonoBehaviour {
        public Rigidbody2D rb;
        public bool isInstantVelocity;
        private Vector2 _refVel = Vector2.zero;
        private float speed = 5f;
        private bool _started;

        private void Start() {
            _started = true;
        }

        private void Update() {
            switch (transform.position.x) {
                case > 10 when speed > 0f:
                case < -10 when speed < 0f:
                    speed *= -1f;
                    break;
            }
        }

        private void FixedUpdate() {
            if (isInstantVelocity) {
                rb.velocity = speed * transform.right;
                return;
            }
            rb.velocity 
                = Vector2.SmoothDamp(rb.velocity, transform.right * speed, ref _refVel, 0.7f);
        }

        private void OnDrawGizmos() {
            if (_started) {
                DebugExtension.DrawString($"{rb.velocity}", transform.position - new Vector3(0, 1), Color.white, Vector2.zero);
            }
        }
    }
}
