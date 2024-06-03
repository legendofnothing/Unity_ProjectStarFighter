using StateMachine;

namespace EnemyScript.v2.StateMachine {
    public abstract class EnemyState : State<EnemyStates> {
        protected EnemyStateMachine esm;

        public EnemyState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) {
            esm = (EnemyStateMachine)stateMachine;
        }
    }
}