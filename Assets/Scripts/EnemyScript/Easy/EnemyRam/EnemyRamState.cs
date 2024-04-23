using StateMachine;

namespace EnemyScript.Easy.EnemyRam {
    public abstract class EnemyRamState : State<EnemyRamStateMachine.EnemyState> {
        protected EnemyRamStateMachine _enemyStateMachine;
        
        public EnemyRamState(EnemyRamStateMachine.EnemyState key, StateMachine<EnemyRamStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _enemyStateMachine = (EnemyRamStateMachine)stateMachine;
        }
    }
}