using EnemyScript.Boss.States;

namespace EnemyScript.Boss.StateMachines {
    public class BossMainStateMachine : BossStateMachine {
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new BossIdleState(EnemyState.Idle, this);
            states[EnemyState.Strafe] = new BossStrafeState(EnemyState.Strafe, this);
            states[EnemyState.Circle] = new BossCircleState(EnemyState.Circle, this);
            states[EnemyState.Reset] = new BossResetState(EnemyState.Reset, this);
            return EnemyState.Strafe;
        }
    }
}