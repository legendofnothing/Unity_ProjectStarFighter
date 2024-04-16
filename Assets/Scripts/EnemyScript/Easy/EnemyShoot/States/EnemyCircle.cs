using StateMachine;

namespace EnemyScript.Easy.EnemyShoot.States {
    public class EnemyCircle : EnemyState {
        public EnemyCircle(EnemyStateMachine.EnemyState key, StateMachine<EnemyStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            _enemyStateMachine.enemyBehaviors.LookAt(_enemyStateMachine.transform.right + _enemyStateMachine.transform.position, _enemyStateMachine.enemy.angularSpeed);
            _enemyStateMachine.enemyBehaviors.FlyForward(1f);
        }
    }
}