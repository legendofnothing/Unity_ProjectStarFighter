using System;
using UnityEngine;

namespace Combat {
    [Serializable]
    public struct ProjectileSetting {
        public float _speed;
        public float _lifeSpan;
    }
    
    public class Projectile : MonoBehaviour {
        private bool _ready;
        private float _speed;
        private Rigidbody2D _rb;
        
        public void Init(ProjectileSetting setting) {
            _rb = GetComponent<Rigidbody2D>();
            _speed = setting._speed;
            Destroy(gameObject, setting._lifeSpan);
            _ready = true;
        }

        private void FixedUpdate() {
            if (!_ready) return;
            _rb.velocity = transform.up * _speed;
        }
    }
}