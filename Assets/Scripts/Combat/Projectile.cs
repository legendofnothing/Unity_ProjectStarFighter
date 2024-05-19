using System;
using Audio;
using Effect;
using Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat {
    [Serializable]
    public struct ProjectileSetting {
        [TitleGroup("Config")]
        public float damage;
        public float speed;
        public float lifeSpan;
        [Range(1, 999)]
        public float proxyModifier;
        public LayerMask interactLayer;
        [TitleGroup("Audio")] 
        public AudioClip activeSound;
        public AudioClip impactSound;
        
        [TitleGroup("Options")] 
        public bool useImpactEffect;
        [ShowIf(nameof(useImpactEffect))] [TitleGroup("Explosion Config")] 
        public GameObject impactEffect;
        [ShowIf(nameof(useImpactEffect))] 
        public Vector3 impactSize;
    }
    
    public abstract class Projectile : MonoBehaviour {
        private bool _ready;
        private float _speed;
        [ReadOnly] public MonoBehaviour owner;
        protected Rigidbody2D _rb;
        protected BoxCollider2D _col;
        protected ProjectileSetting _setting;

        public Vector2 Velocity => transform.up * _speed;
        
        public void Init(ProjectileSetting setting, MonoBehaviour owner) {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<BoxCollider2D>();
            if (setting.proxyModifier >= 1) _col.size *= setting.proxyModifier;
            _speed = setting.speed;
            Destroy(gameObject, setting.lifeSpan);
            _setting = setting;
            this.owner = owner;
            
            AudioManager.Instance.PlaySFX(setting.activeSound, transform, true, true, 2);
            
            _ready = true;
        }

        private void FixedUpdate() {
            OnUpdate();
        }

        protected virtual void OnUpdate() {
            if (!_ready) return;
            _rb.velocity = transform.up * _speed;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (CheckLayerMask.IsInLayerMask(other.gameObject, _setting.interactLayer)) {
                if (!other) return;
                OnImpact(other);
                AudioManager.Instance.PlaySFX(_setting.impactSound, transform, false, false, 3);
                
                if (_setting.useImpactEffect && _setting.impactEffect != null) {
                    var explosionInst = Instantiate(_setting.impactEffect, transform.position, Quaternion.identity);
                    explosionInst.GetComponent<EffectBase>().Init(_setting.impactSize);
                }
            }
        }

        protected abstract void OnImpact(Collider2D obj);
    }
}