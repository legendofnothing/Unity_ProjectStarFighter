using StateMachine;
using UnityEngine;

namespace EnemyScript.v1.Easy.EnemyShoot.States {
    public class EnemyCircle : EnemyState {
        private float _distanceThreshold;
        private int _circlingDirection;
        
        public EnemyCircle(EnemyShootStateMachine.EnemyState key, StateMachine<EnemyShootStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _circlingDirection = Random.Range(0, 2);
            _distanceThreshold = Random.Range(_enemyStateMachine.minimumCirclingDistance,
                _enemyStateMachine.maximumCirclingDistance);
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            var dir = _circlingDirection == 0 ? _enemyStateMachine.transform.right : -_enemyStateMachine.transform.right;
            _enemyStateMachine
                .enemyBehaviors
                .LookAt(dir + _enemyStateMachine.transform.position, _enemyStateMachine.enemy.minimumAngularSpeed);
            _enemyStateMachine.enemyBehaviors.FlyForward(_enemyStateMachine.enemy.speed);

            if (_enemyStateMachine.enemy.GetDistanceToPlayer >= _distanceThreshold) {
                _enemyStateMachine.SwitchState(EnemyShootStateMachine.EnemyState.Attacking);
            }
        }
    }
}