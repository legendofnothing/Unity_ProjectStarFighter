using PlayerScript;
using Scripts.Core;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.v1.Easy.EnemyRam {
    public class EnemyRamStateMachine : StateMachine<EnemyRamStateMachine.EnemyState> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;

        [TitleGroup("Config")] 
        public LayerMask playerLayer;
        public float ramDamage = 10;
        [ReadOnly] public GameObject target;
        
        public enum EnemyState {
            Ramming
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Ramming] = new States.EnemyRam(EnemyState.Ramming, this);
            return EnemyState.Ramming;
        }

        protected override void SetupRef() {
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (CheckLayerMask.IsInLayerMask(other.gameObject, playerLayer)) {
                Player.Instance.TakeDamage(ramDamage);
                enemy.TakeDamage(999999);
            }
        }
    }
}
