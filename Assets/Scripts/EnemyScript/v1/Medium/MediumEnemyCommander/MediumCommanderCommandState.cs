using StateMachine;

namespace EnemyScript.v1.Medium.MediumEnemyCommander {
    public abstract class MediumCommanderCommandState : State<MediumCommanderCommandStateMachine.EnemyState> {
        protected MediumCommanderCommandStateMachine _esm;

        protected MediumCommanderCommandState(MediumCommanderCommandStateMachine.EnemyState key, StateMachine<MediumCommanderCommandStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _esm = (MediumCommanderCommandStateMachine) stateMachine;
        }
    }
}