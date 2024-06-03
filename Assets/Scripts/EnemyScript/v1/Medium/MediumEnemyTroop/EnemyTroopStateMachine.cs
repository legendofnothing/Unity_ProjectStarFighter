using EnemyScript.v1.Medium.MediumEnemyTroop.States;
using Sirenix.OdinInspector;
using StateMachine;

namespace EnemyScript.v1.Medium.MediumEnemyTroop {
    public class EnemyTroopStateMachine : StateMachine<EnemyTroopStateMachine.EnemyState> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;
        
        [TitleGroup("Observing Config")] 
        public float minimumObservingDistance = 8f;
        public float maximumObservingDistance = 12f;
        public float dangerDistance = 5f;
        
        public enum EnemyState {
            Idle,
            Observing,
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new EnemyTroopIdle(EnemyState.Idle, this);
            states[EnemyState.Observing] = new EnemyTroopObserving(EnemyState.Observing, this);
            return EnemyState.Observing;
        }

        protected override void SetupRef() {
        }
    }
}
