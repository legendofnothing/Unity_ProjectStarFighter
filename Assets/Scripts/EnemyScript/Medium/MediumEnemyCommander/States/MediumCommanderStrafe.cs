using PlayerScript;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander.States {
    public class MediumCommanderStrafe : MediumCommanderState {
        public MediumCommanderStrafe(MediumCommanderAttackStateMachine.EnemyState key, StateMachine<MediumCommanderAttackStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _esm.enemy.currentAngularSpeed = _esm.enemy.angularSpeed;
            _esm.enemy.currentSpeed = _esm.enemy.minimumSpeed;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            //this for weapon
            var predictedDot = _esm.enemy.GetDotToPoint(_esm.PredictPlayerPosition(_esm.predictedFrames));
            if (predictedDot > 0.99f) {
                _esm.enemyBehaviors.FireWeapon();
            }
            
            //this for mvoement
            var dot = _esm.enemy.GetDotToPlayer;
            if (dot > 0.97f && _esm.enemy.GetDistanceToPlayer <= _esm.minimumDistance) {
                _esm.SwitchState(MediumCommanderAttackStateMachine.EnemyState.Resetting);
            }
            else {
                _esm.enemyBehaviors.LookAt(_esm.PredictPlayerPosition(_esm.predictedFrames), _esm.enemy.currentAngularSpeed); 
            }
            
            var lerpedSpeed = Mathf.Lerp(_esm.enemy.minimumSpeed, _esm.enemy.speed - _esm.enemy.minimumSpeed, Mathf.Clamp(dot, 0f, 1f));
            _esm.enemy.currentSpeed = lerpedSpeed;
            _esm
                .enemyBehaviors.FlyForward(_esm.enemy.currentSpeed); 
        }
    }
}