using StateMachine;

namespace EnemyScript.Medium.MediumEnemyCommander.States {
    public class MediumCommanderIdle : MediumCommanderState {
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