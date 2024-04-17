using StateMachine;

namespace EnemyScript.Easy.EnemyRam {
    public abstract class EnemyState : State<EnemyStateMachine.EnemyState> {
        protected EnemyStateMachine _enemyStateMachine;
        
        public EnemyState(EnemyStateMachine.EnemyState key, StateMachine<EnemyStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _enemyStateMachine = (EnemyStateMachine)stateMachine;
        }
    }
}