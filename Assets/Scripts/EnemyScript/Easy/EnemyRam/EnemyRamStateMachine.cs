using PlayerScript;
using Scripts.Core;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Easy.EnemyRam {
    public class EnemyRamStateMachine : StateMachine<EnemyRamStateMachine.EnemyState> {
        [ReadOnly] public EnemyBehaviors enemyBehaviors;
        [ReadOnly] public Enemy enemy;

        [TitleGroup("Config")] 
        public LayerMask playerLayer;
        public float ramDamage = 10;
        
        public enum EnemyState {
            Ramming
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Ramming] = new States.EnemyRam(EnemyState.Ramming, this);
            return EnemyState.Ramming;
        }

        protected override void SetupRef() {
            enemy = GetComponent<Enemy>();
            enemyBehaviors = GetComponent<EnemyBehaviors>();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (CheckLayerMask.IsInLayerMask(other.gameObject, playerLayer)) {
                Player.Instance.TakeDamage(ramDamage);
                enemy.TakeDamage(999999);
            }
        }
    }
}
