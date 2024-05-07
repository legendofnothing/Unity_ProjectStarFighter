using StateMachine;

namespace EnemyScript.Easy.EnemyRam.States {
    public class EnemyRam : EnemyRamState {
        public EnemyRam(EnemyRamStateMachine.EnemyState key, StateMachine<EnemyRamStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _enemyStateMachine.enemy.currentSpeed = _enemyStateMachine.enemy.speed;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            if (!_enemyStateMachine.target) {
                _enemyStateMachine
                    .enemyBehaviors.LookAt(PlayerScript.Player.Instance.transform.position, _enemyStateMachine.enemy.angularSpeed); 
                _enemyStateMachine
                    .enemyBehaviors.FlyForward(_enemyStateMachine.enemy.currentSpeed); 
            }
        }
    }
}