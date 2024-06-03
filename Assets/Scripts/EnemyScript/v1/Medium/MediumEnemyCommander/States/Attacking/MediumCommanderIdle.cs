using StateMachine;

namespace EnemyScript.v1.Medium.MediumEnemyCommander.States.Attacking {
    public class MediumCommanderIdle : MediumCommanderAttackState {
        public MediumCommanderIdle(MediumCommanderAttackStateMachine.EnemyState key, StateMachine<MediumCommanderAttackStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            
        }

        public override void OnExit() {
            
        }

        public override void OnUpdate() {
        }
    }
}