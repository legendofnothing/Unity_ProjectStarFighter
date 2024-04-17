using StateMachine;

namespace EnemyScript.Easy.EnemyRam.States {
    public class EnemyRam : EnemyState {
        public EnemyRam(EnemyStateMachine.EnemyState key, StateMachine<EnemyStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _enemyStateMachine.enemy.currentSpeed = _enemyStateMachine.enemy.speed;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            _enemyStateMachine
                .enemyBehaviors.LookAt(PlayerScript.Player.Instance.transform.position, _enemyStateMachine.enemy.angularSpeed); 
            _enemyStateMachine
                .enemyBehaviors.FlyForward(_enemyStateMachine.enemy.currentSpeed); 
        }
    }
}