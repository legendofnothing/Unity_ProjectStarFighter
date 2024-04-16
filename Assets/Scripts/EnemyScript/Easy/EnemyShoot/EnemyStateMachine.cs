using EnemyScript.Easy.EnemyShoot.States;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScript.Easy.EnemyShoot {
    public class EnemyStateMachine : StateMachine<EnemyStateMachine.EnemyState> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;
        
        [TitleGroup("Config")] 
        public float minimumDistance;
        
        public enum EnemyState {
            Idle,
            Maneuvering,
            Circling,
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new EnemyIdle(EnemyState.Idle, this);
            states[EnemyState.Maneuvering] = new EnemyAttack(EnemyState.Maneuvering, this);
            states[EnemyState.Circling] = new EnemyCircle(EnemyState.Circling, this);
            return EnemyState.Maneuvering;
        }

        protected override void SetupRef() {
            enemy = GetComponent<Enemy>();
            enemyBehaviors = GetComponent<EnemyBehaviors>();
        }
    }
}
