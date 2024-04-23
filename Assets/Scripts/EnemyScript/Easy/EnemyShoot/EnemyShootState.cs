using StateMachine;

namespace EnemyScript.Easy.EnemyShoot {
    public abstract class EnemyState : State<EnemyShootStateMachine.EnemyState> {
        protected EnemyShootStateMachine _enemyStateMachine;
        
        public EnemyState(EnemyShootStateMachine.EnemyState key, StateMachine<EnemyShootStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _enemyStateMachine = (EnemyShootStateMachine)stateMachine;
        }
    }
}