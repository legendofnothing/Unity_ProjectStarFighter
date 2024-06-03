using StateMachine;

namespace EnemyScript.v1.Medium.MediumEnemyCommander {
    public abstract class MediumCommanderAttackState : State<MediumCommanderAttackStateMachine.EnemyState> {
        protected MediumCommanderAttackStateMachine _esm;

        protected MediumCommanderAttackState(MediumCommanderAttackStateMachine.EnemyState key, StateMachine<MediumCommanderAttackStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _esm = (MediumCommanderAttackStateMachine) stateMachine;
        }
    }
}