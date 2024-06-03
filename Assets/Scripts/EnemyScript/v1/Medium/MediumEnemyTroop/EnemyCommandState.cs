using StateMachine;

namespace EnemyScript.v1.Medium.MediumEnemyTroop {
    public abstract class EnemyCommandState : State<EnemyTroopStateMachine.EnemyState> {
        protected EnemyTroopStateMachine _esm;
        
        public EnemyCommandState(EnemyTroopStateMachine.EnemyState key, StateMachine<EnemyTroopStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _esm = (EnemyTroopStateMachine)stateMachine;
        }
    }
}