using StateMachine;

namespace EnemyScript.v2.StateMachine.States {
    public class EnemyIdleState : EnemyState {
        public EnemyIdleState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) { }
        
        public override void OnEnter() { }

        public override void OnExit() { }

        public override void OnUpdate() { }
    }
}