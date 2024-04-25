using StateMachine;

namespace EnemyScript.Medium.MediumEnemyTroop {
    public abstract class EnemyCommandState : State<EnemyTroopStateMachine.EnemyState> {
        protected EnemyTroopStateMachine _enemyStateMachine;
        
        public EnemyCommandState(EnemyTroopStateMachine.EnemyState key, StateMachine<EnemyTroopStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _enemyStateMachine = (EnemyTroopStateMachine)stateMachine;
        }
    }
}