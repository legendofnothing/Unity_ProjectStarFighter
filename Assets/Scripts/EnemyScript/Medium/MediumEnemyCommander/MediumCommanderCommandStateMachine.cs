using EnemyScript.Commander.Variation;
using EnemyScript.Medium.MediumEnemyCommander.States.Commanding;
using EnemyScript.Medium.Troop;
using Sirenix.OdinInspector;
using StateMachine;

namespace EnemyScript.Medium.MediumEnemyCommander {
    public class MediumCommanderCommandStateMachine : StateMachine<MediumCommanderCommandStateMachine.EnemyState> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;
        public MediumCommander commander;

        [TitleGroup("Observing Config")] 
        public float minimumObservingDistance = 8f;
        public float maximumObservingDistance = 12f;
        public float dangerDistance = 5f;
        
        
        public enum EnemyState {
            Idle,
            Observing,
        }
        
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new MediumCommanderIdle(EnemyState.Idle, this);
            states[EnemyState.Observing] = new MediumCommanderObserving(EnemyState.Observing, this);
            return EnemyState.Observing;
        }

        protected override void SetupRef() {
            enemy = GetComponentInParent<Enemy>();
            enemyBehaviors = GetComponentInParent<EnemyBehaviors>();
        }
    }
}
