using Audio;
using Core.Events;
using DG.Tweening;
using Effect;
using EnemyScript.Commander;
using PlayerScript;
using Sirenix.OdinInspector;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace EnemyScript {
    public class Enemy : MonoBehaviour {
        [TitleGroup("Refs")] 
        public GameObject uiPrefab;
        public AudioClip deathSound;
        
        [TitleGroup("Config")] public float hp = 100;
        [ReadOnly] public float currentHp;
        [ReadOnly] public bool hasDied;
        [Space] 
        public float speed;
        public float minimumSpeed;
        public float speedDampValue = 0.7f;
        [Space] 
        public float minimumAngularSpeed;
        public float angularSpeed;

        [ReadOnly] public float currentSpeed;
        [ReadOnly] public float currentAngularSpeed;

        [TitleGroup("Options")] public bool useExplosionEffect = true;

        [ShowIf(nameof(useExplosionEffect))] [TitleGroup("Explosion Config")]
        public GameObject explosionEffect;

        [ShowIf(nameof(useExplosionEffect))] public Vector3 explosionSize = Vector3.one;
        protected EnemyBehaviors enemyBehaviors;
        private Troop _troop;
        private EnemyUI _ui;
        private bool _isInCamera;
        private Tween _alphaUiTween;
        public EnemyUI Ui => _ui == null ? null : _ui;
        [ReadOnly] public bool stopUpdatingUI;
        [ReadOnly] public bool canDamage = true;

        private void Awake() {
            currentHp = hp;
            currentSpeed = speed;
            enemyBehaviors = GetComponent<EnemyBehaviors>();
            enemyBehaviors.speedDampValue = speedDampValue;

            minimumAngularSpeed -= Random.Range(-0.1f, 0.1f);
            minimumSpeed -= Random.Range(-0.1f, 0.1f);
            angularSpeed -= Random.Range(-0.1f, 0.1f);
            speed -= Random.Range(-0.1f, 0.1f);
            
            if (gameObject.TryGetComponent<Troop>(out var troop)) {
                _troop = troop;
            }
            
            if (!uiPrefab) return;
            var inst = Instantiate(uiPrefab, transform.position, Quaternion.identity);
            inst.transform.SetParent(gameObject.transform);
            _ui = inst.GetComponent<EnemyUI>();
            _ui.SetValue(currentHp / hp);
            _ui.canvasGroup.alpha = Player.Instance.IsInCamera(transform.position) ? 1 : 0;
        }

        private void Start() {
            this.FireEvent(EventType.OnEnemySpawned);
        }

        public void TakeDamage(float amount) {
            if (hasDied) return;
            if (!canDamage) return;
            currentHp -= amount;
            if (_troop) _troop.OnDamage();
            if (_ui) _ui.SetValue(currentHp / hp);
            if (!(currentHp <= 0)) return;
            if (_ui) _ui.SetValue(currentHp / hp);
            hasDied = true;
            Death();
        }

        protected virtual void Update() {
            if (!_ui) return;
            if ((Player.Instance.IsInCamera(transform.position) && _ui.canvasGroup.alpha <= 0) && !stopUpdatingUI) {
                _alphaUiTween?.Kill();
                _alphaUiTween = DOVirtual.Float(0, 1, 2f, value => {
                    _ui.canvasGroup.alpha = value;
                });
            }
            else if ((!Player.Instance.IsInCamera(transform.position) && _ui.canvasGroup.alpha > 0) || stopUpdatingUI) {
                _alphaUiTween?.Kill();
                _alphaUiTween = DOVirtual.Float(1, 0, 0.5f, value => {
                    _ui.canvasGroup.alpha = value;
                });
            }
        }

        protected virtual void Death() {
            if (useExplosionEffect && explosionEffect != null) {
                var explosionInst = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosionInst.GetComponent<EffectBase>().Init(explosionSize);
            }
            
            if (_troop) _troop.OnDeath();
            _alphaUiTween?.Kill();
            this.FireEvent(EventType.OnEnemyKilled, this);
            if (deathSound) AudioManager.Instance.PlaySFX(deathSound, transform, false, false, 5);
            Destroy(gameObject);
        }

        public float GetDotToPlayer => Vector2.Dot(transform.up,
            (PlayerScript.Player.Instance.transform.position - transform.position).normalized);

        public float GetDotToPoint(Vector2 pos) {
            return  Vector2.Dot(transform.up,
                (pos - (Vector2) transform.position).normalized);
        }
        
        public float GetDotToPoint(GameObject pos) {
            return  Vector2.Dot(transform.up,
                (pos.transform.position - transform.position).normalized);
        }
        
        public float GetDistanceToTarget(Vector3 position) => Vector2.Distance(transform.position,
            position);
        
        public float GetDistanceToTarget(GameObject gb) => Vector2.Distance(transform.position,
            gb.transform.position);
        
        public float GetDistanceToPlayer => Vector2.Distance(transform.position,
            PlayerScript.Player.Instance.transform.position);
    }
}


