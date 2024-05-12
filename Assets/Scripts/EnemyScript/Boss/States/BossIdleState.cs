using StateMachine;

namespace EnemyScript.Boss.States {
    public class BossIdleState : BossState {
        public BossIdleState(BossStateMachine.EnemyState key, StateMachine<BossStateMachine.EnemyState> stateMachine) : base(key, stateMachine) { }

        public override void OnEnter() { }

        public override void OnExit() { }

        public override void OnUpdate() { }
    }
}