using StateMachine;
using UnityEngine;

namespace EnemyScript.Boss.States {
    public class BossStrafeState : BossState {
        private float _lastHp;
        
        public BossStrafeState(BossStateMachine.EnemyState key, StateMachine<BossStateMachine.EnemyState> stateMachine) : base(key, stateMachine) { }

        public override void OnEnter() {
            esm.enemy.currentAngularSpeed = esm.enemy.angularSpeed;
            esm.enemy.currentSpeed = esm.enemy.minimumSpeed;
            _lastHp = esm.enemy.currentHp;
        }

        public override void OnExit() { }

        public override void OnUpdate() {
            //this for weapon
            var predictedDot = esm.enemy.GetDotToPoint(esm.PredictPlayerPosition(esm.predictedFrames));
            if (predictedDot > 0.99f && esm.enemy.GetDistanceToPlayer <= 30f) {
                esm.enemyWeapon.FireWeapon();
            }
            
            //this for mvoement
            var dot = esm.enemy.GetDotToPlayer;
            if (dot > 0.97f && esm.enemy.GetDistanceToPlayer <= esm.minimumDistance) {
                esm.SwitchState(BossStateMachine.EnemyState.Reset);
            }
            else {
                esm.enemyBehaviors.LookAt(esm.PredictPlayerPosition(esm.predictedFrames), esm.enemy.currentAngularSpeed); 
            }
            
            var lerpedSpeed = Mathf.Lerp(esm.enemy.minimumSpeed, esm.enemy.speed - esm.enemy.minimumSpeed, Mathf.Clamp(dot, 0f, 1f));
            esm.enemy.currentSpeed = lerpedSpeed;
            esm
                .enemyBehaviors.FlyForward(esm.enemy.currentSpeed); 
            
            if (_lastHp > esm.enemy.currentHp) {
                esm.SwitchState(BossStateMachine.EnemyState.Reset);
            }
        }
    }
}