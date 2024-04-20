using StateMachine;

namespace EnemyScript.Medium.MediumEnemyCommander.States {
    public abstract class MediumCommanderState : State<MediumCommanderAttackStateMachine.EnemyState> {
        protected MediumCommanderAttackStateMachine _esm;

        protected MediumCommanderState(MediumCommanderAttackStateMachine.EnemyState key, StateMachine<MediumCommanderAttackStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _esm = (MediumCommanderAttackStateMachine) stateMachine;
        }
    }
}