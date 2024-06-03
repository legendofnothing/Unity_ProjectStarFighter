using StateMachine;
using UnityEngine;

namespace EnemyScript.v1.Medium.MediumEnemyCommander.States.Attacking {
    public class MediumCommanderCircling : MediumCommanderAttackState {
        private float _lastHp;
        
        public MediumCommanderCircling(MediumCommanderAttackStateMachine.EnemyState key, StateMachine<MediumCommanderAttackStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _esm.enemy.currentSpeed = _esm.enemy.speed;
            _esm.enemy.currentAngularSpeed = _esm.enemy.angularSpeed;
            _lastHp = _esm.enemy.currentHp;
        }

        public override void OnExit() {
            
        }

        public override void OnUpdate() {
            var predictedPos = _esm.PredictPlayerPosition(_esm.predictedFrames);
            var predictedDot = _esm.enemy.GetDotToPoint(predictedPos);
            
            _esm.enemyBehaviors.LookAt(predictedPos, _esm.enemy.angularSpeed); 
            if (_esm.enemy.GetDistanceToPlayer >= 8f) {
                _esm.enemy.currentSpeed = _esm.enemy.speed;
                _esm.enemyBehaviors.FlyForward(_esm.enemy.currentSpeed);
            }
            else {
                _esm.enemy.currentSpeed = _esm.enemy.minimumSpeed + 5f;
                _esm.enemyBehaviors.Fly(_esm.enemy.currentSpeed, _esm.transform.right);
            }

            if (predictedDot >= 0.98f) {
                _esm.enemyBehaviors.FireWeapon();
            }

            if (_lastHp > _esm.enemy.currentHp) {
                Debug.Log("switched state");
                _esm.SwitchState(MediumCommanderAttackStateMachine.EnemyState.Resetting);
            }
        }
    }
}