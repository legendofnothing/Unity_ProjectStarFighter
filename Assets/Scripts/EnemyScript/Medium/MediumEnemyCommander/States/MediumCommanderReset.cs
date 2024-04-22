using DG.Tweening;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander.States {
    public class MediumCommanderReset : MediumCommanderState {
        private float _distanceThreshold;
        private int _circlingDirection;
        private float _timePassed;
        private Tween _speedIncreaseTween;
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
            _esm.enemyBehaviors.LookAt(dir + _esm.transform.position, _esm.enemy.minimumAngularSpeed);
            _esm.enemyBehaviors.FlyForward(_esm.enemy.currentSpeed);

            _timePassed += Time.fixedDeltaTime;
            if (_timePassed >= _esm.timeUntilAccelerate) {
                if (_speedIncreaseTween == null) {
                    _speedIncreaseTween = DOVirtual.Float(_esm.enemy.currentSpeed, _esm.enemy.speed * 2, _esm.accelerateTime,
                        value => {
                            _esm.enemy.currentSpeed = value;
                        });
                }
            }
            
            if (_esm.enemy.GetDistanceToPlayer >= _distanceThreshold) {
                _locked = true;
                DOVirtual.Float(_esm.enemy.currentSpeed, _esm.enemy.speed, _esm.deAccelerateTime, value => {
                    _esm.enemy.currentSpeed = value;
                }).OnComplete(() => {
                    _speedIncreaseTween?.Kill();
                    _esm.SwitchState(MediumCommanderAttackStateMachine.EnemyState.Strafing);
                });
            }
        }
    }
}