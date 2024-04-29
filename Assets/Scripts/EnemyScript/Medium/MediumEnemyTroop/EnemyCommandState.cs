using StateMachine;

namespace EnemyScript.Medium.MediumEnemyTroop {
    public abstract class EnemyCommandState : State<EnemyTroopStateMachine.EnemyState> {
        protected EnemyTroopStateMachine esm;
        
        public EnemyCommandState(EnemyTroopStateMachine.EnemyState key, StateMachine<EnemyTroopStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            esm = (EnemyTroopStateMachine)stateMachine;
        }
    }
}