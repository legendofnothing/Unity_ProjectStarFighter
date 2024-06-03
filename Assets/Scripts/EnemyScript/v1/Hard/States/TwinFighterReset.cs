using StateMachine;
using UnityEngine;

namespace EnemyScript.v1.Hard.States {
    public class TwinFighterReset : TwinFighterState {
        private float _distanceThreshold;
        private int _circlingDirection;
        
        public TwinFighterReset(TwinFighterStateMachine.EnemyState key, StateMachine<TwinFighterStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _circlingDirection = Random.Range(0, 2);
            _distanceThreshold = Random.Range(_esm.minimumSafeDistance, _esm.maximumSafeDistance);
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            var dir = _circlingDirection == 0 ? _esm.transform.right : -_esm.transform.right;
            _esm
                .enemyBehaviors
                .LookAt(dir + _esm.transform.position, _esm.enemy.minimumAngularSpeed);
            _esm.enemyBehaviors.FlyForward(_esm.enemy.speed);
            
            if (_esm.enemy.GetDistanceToPlayer >= _distanceThreshold && !_esm.overrideResettingState) {
                _esm.SwitchState(TwinFighterStateMachine.EnemyState.Attack);
            }
        }
    }
}