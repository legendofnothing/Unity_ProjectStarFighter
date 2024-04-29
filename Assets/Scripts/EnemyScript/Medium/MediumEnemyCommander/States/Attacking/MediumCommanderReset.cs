using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander.States.Attacking {
    public class MediumCommanderReset : MediumCommanderAttackState {
        private float _distanceThreshold;
        private int _circlingDirection;
        private float _timePassed;
        private bool _locked;

        public MediumCommanderReset(MediumCommanderAttackStateMachine.EnemyState key, StateMachine<MediumCommanderAttackStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _circlingDirection = Random.Range(0, 2);
            _esm.enemy.currentAngularSpeed = _esm.enemy.minimumAngularSpeed;
            _distanceThreshold = Random.Range(_esm.minimumSafeDistance, _esm.maximumSafeDistance);
            _esm.enemy.currentSpeed = _esm.enemy.speed;
            _timePassed = 0;
            _locked = false;
        }

        public override void OnExit() {
            
        }

        public override void OnUpdate() {
            if (_locked) return;
            
            var dir = _circlingDirection == 0 ? _esm.transform.right : -_esm.transform.right;
            _esm.enemyBehaviors.LookAt(dir + _esm.transform.position, _esm.enemy.currentAngularSpeed);
            _esm.enemyBehaviors.FlyForward(_esm.enemy.currentSpeed);

            _timePassed += Time.fixedDeltaTime;
            if (_timePassed >= _esm.timeUntilAccelerate) {
                _esm.enemy.currentSpeed = _esm.enemy.speed * 2;
            }
            
            if (_esm.enemy.GetDistanceToPlayer >= _distanceThreshold) {
                _locked = true;
                _esm.enemy.currentSpeed = _esm.enemy.speed;
                var choice = Random.Range(0, 2);
                _esm.SwitchState(choice == 0 ? MediumCommanderAttackStateMachine.EnemyState.Strafing : MediumCommanderAttackStateMachine.EnemyState.Circling);
            }
        }
    }
}