using UnityEngine;

namespace Scenes.PredictionTest {
    public class Bullet : MonoBehaviour {
        private Rigidbody2D _rb;
        public float speed;
        private void Start() {
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = transform.up * speed;
            Destroy(gameObject, 10f);
        }
    }
}
