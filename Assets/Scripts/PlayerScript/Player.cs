using Core;
using Core.Events;
using Core.Patterns;
using DG.Tweening;
using Level;
using Sirenix.OdinInspector;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace PlayerScript {
    public class Player : Singleton<Player> {
        public Camera mainCamera;
        [Space]
        public float hp;
        [Space] 
        public float shieldChargeRate;
        public float shieldBlockDamage;
        
        [ReadOnly] 
        public float currentHp;
        [ReadOnly] 
        public float currentShield;

        private bool _canGenerateShield = true;

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

        public bool IsInCamera(Vector3 worldPoint) => mainCamera.IsInFrustum(worldPoint);

        private bool _hasDied;
        private Rigidbody2D _rb;
        private bool _isRunning;
        private Tween _resetShieldTween;

        private void Start() {
            _rb = GetComponent<Rigidbody2D>();
            currentHp = hp;
            currentShield = shieldBlockDamage;
            _isRunning = true;
            UpdateUI();
        }

        private void FixedUpdate() {
            if (_canGenerateShield && currentShield < shieldBlockDamage) {
                currentShield += Time.fixedDeltaTime * shieldChargeRate;
                UpdateUI();
            }
        }

        public void TakeDamage(float amount) {
            if (_hasDied) return;
            currentShield -= amount;
            if (currentShield < 0) {
                var deltaDiff = -currentShield;
                currentShield = 0;
                
                currentHp -= deltaDiff;
                if (currentHp <= 0) {
                    currentHp = 0;
                    Death();
                }
            }
            
            _resetShieldTween?.Kill();
            _canGenerateShield = false;
            _resetShieldTween = DOVirtual.DelayedCall(5f, () => _canGenerateShield = true);
            
            UpdateUI();
        }

        public void Death() {
            this.FireEvent(EventType.OnGameStateChange, GameState.GameOver);
            _hasDied = true;
        }

        private void OnDrawGizmos() {
            if (_isRunning) {
                DebugExtension.DrawString($"velocity {_rb.velocity}", transform.position + new Vector3(1.5f, 0, 0), Color.red, Vector2.zero);
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(_rb.position + _rb.velocity * (Time.fixedDeltaTime * 5), Vector3.one);
            }
        }

        private void UpdateUI() {
            this.FireEvent(EventType.OnPlayerHpChangeBar, currentHp/hp);
            this.FireEvent(EventType.OnPlayerHpChangeText, currentHp.ToString("0"));
            this.FireEvent(EventType.OnShieldChange, currentShield / shieldBlockDamage);
        }
    }
}
