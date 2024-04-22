using Core;
using Core.Patterns;
using Unity.Collections;
using UnityEngine;

namespace PlayerScript {
    public class Player : Singleton<Player> {
        public float hp;
        [ReadOnly] public float currentHp;

        public Vector2 PlayerDir {
            get {
                if (_rb) return _rb.velocity;
                
                _rb = GetComponent<Rigidbody2D>();
                return _rb.velocity;
            }
        }

        public Vector2 PlayerPos {
            get {
                if (_rb) return _rb.position;
                
                _rb = GetComponent<Rigidbody2D>();
                return _rb.position;
            }
        }
        
        
        private bool _hasDied;
        private Rigidbody2D _rb;
        private bool _isRunning;

        private void Start() {
            _rb = GetComponent<Rigidbody2D>();
            currentHp = hp;
            _isRunning = true;
        }
        
        public void TakeDamage(float amount) {
            if (_hasDied) return;
            currentHp -= amount;
            if (currentHp <= 0) {
                currentHp = 0;
                Death();
            }
        }

        public void Death() {
            Debug.Log("death");
            _hasDied = true;
        }

        private void OnDrawGizmos() {
            if (_isRunning) {
                DebugExtension.DrawString($"velocity {_rb.velocity}", transform.position + new Vector3(1.5f, 0, 0), Color.red, Vector2.zero);
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(_rb.position + _rb.velocity * (Time.fixedDeltaTime * 5), Vector3.one);
            }
        }
    }
}
