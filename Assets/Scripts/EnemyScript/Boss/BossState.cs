using StateMachine;

namespace EnemyScript.Boss {
    public abstract class BossState : State<BossStateMachine.EnemyState> {
        protected BossStateMachine esm;
        protected BossState(BossStateMachine.EnemyState key, StateMachine<BossStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            esm = (BossStateMachine)stateMachine;
        }
    }
}