using DG.Tweening;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Boss.States {
    public class BossResetState : BossState {
        private float _distanceThreshold;
        private int _circlingDirection;
        private float _timePassed;
        private bool _locked;
        
        public BossResetState(BossStateMachine.EnemyState key, StateMachine<BossStateMachine.EnemyState> stateMachine) : base(key, stateMachine) { }

        public override void OnEnter() {
            _circlingDirection = Random.Range(0, 2);
            esm.enemy.currentAngularSpeed = esm.enemy.minimumAngularSpeed;
            _distanceThreshold = Random.Range(esm.minimumSafeDistance, esm.maximumSafeDistance);
            esm.enemy.currentSpeed = esm.enemy.speed;
            _timePassed = 0;
            _locked = false;
        }

        public override void OnExit() { }

        public override void OnUpdate() {
            if (_locked) return;
            
            var dir = _circlingDirection == 0 ? esm.transform.right : -esm.transform.right;
            esm.enemyBehaviors.LookAt(dir + esm.transform.position, esm.enemy.currentAngularSpeed);
            esm.enemyBehaviors.FlyForward(esm.enemy.currentSpeed);

            _timePassed += Time.fixedDeltaTime;
            if (_timePassed >= esm.timeUntilAccelerate) {
                esm.enemy.currentSpeed = esm.enemy.speed * 2;
            }
            
            if (esm.enemy.GetDistanceToPlayer >= _distanceThreshold) {
                _locked = true;
                esm.enemy.currentSpeed = esm.enemy.speed;
                var choice = Random.Range(0, 2);
                DOVirtual.DelayedCall(0.8f, () => {
                    esm.SwitchState(choice == 0 ? BossStateMachine.EnemyState.Strafe : BossStateMachine.EnemyState.Circle);
                });
            }
        }
    }
}