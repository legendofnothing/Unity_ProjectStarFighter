using EnemyScript.v1.Easy.EnemyShoot.States;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.v1.Easy.EnemyShoot {
    public class EnemyShootStateMachine : StateMachine<EnemyShootStateMachine.EnemyState> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;
        
        [TitleGroup("Config")] 
        public float minimumDistance;

        [Space] 
        public float minimumCirclingDistance;
        public float maximumCirclingDistance;
        
        [ReadOnly] public GameObject target;
        
        public enum EnemyState {
            Idle,
            Attacking,
            Circling,
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new EnemyIdle(EnemyState.Idle, this);
            states[EnemyState.Attacking] = new EnemyAttack(EnemyState.Attacking, this);
            states[EnemyState.Circling] = new EnemyCircle(EnemyState.Circling, this);
            return EnemyState.Attacking;
        }

        protected override void SetupRef() {
        }
    }
}
