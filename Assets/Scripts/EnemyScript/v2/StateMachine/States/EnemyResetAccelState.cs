using DG.Tweening;
using EnemyScript.v1.Easy.EnemyShoot;
using StateMachine;
using UnityEngine;

namespace EnemyScript.v2.StateMachine.States {
    public class EnemyResetAccelState : EnemyState {
        private int _circlingDirection;
        private Tween _accelTween;

        public EnemyResetAccelState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) { }
        
        public override void OnEnter() {
            _circlingDirection = Random.Range(0, 2);
            esm.enemy.currentSpeed = esm.enemy.speed;
            esm.enemy.currentAngularSpeed = esm.enemy.minimumAngularSpeed;

            var currSpeed = esm.enemy.currentSpeed;
            _accelTween = DOVirtual.Float(currSpeed, currSpeed + currSpeed * esm.percentAccelAdded, esm.accelTime, value => {
                esm.enemy.currentSpeed = value;
            });
        }

        public override void OnExit() {
            _accelTween.Kill();
            esm.enemy.currentSpeed = esm.enemy.speed;
        }

        public override void OnUpdate() {
            var dir = _circlingDirection == 0 ? esm.transform.right : -esm.transform.right;
            esm.enemyBehaviors.LookAt(dir + esm.transform.position, esm.enemy.currentAngularSpeed);
            esm.enemyBehaviors.FlyForward(esm.enemy.speed);
        }
    }
}