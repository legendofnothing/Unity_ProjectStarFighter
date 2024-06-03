using PlayerScript;
using StateMachine;
using UnityEngine;

namespace EnemyScript.v2.StateMachine.States {
    public class EnemyStrafeState : EnemyState {
        public EnemyStrafeState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) { }

        public override void OnEnter() {
            esm.enemy.currentSpeed = esm.enemy.speed;
            esm.enemy.currentAngularSpeed = esm.enemy.angularSpeed;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            //1 full on
            //0.5 side profile
            var predictedPos = esm.GetPredictedPos();
            var dot = esm.enemy.GetDotToPoint(predictedPos);
            
                
            if (dot >= UniversalSetting.DotToShoot && esm.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) <= esm.minEngageDistance) {
                esm.enemyBehaviors.FireWeapon();
            }

            esm.enemyBehaviors.LookAt(predictedPos, esm.enemy.angularSpeed);
            var lerpedSpeed = Mathf.Lerp(esm.enemy.minimumSpeed, esm.enemy.speed, Mathf.Clamp(dot, 0f, 1f));
            esm.enemy.currentSpeed = lerpedSpeed;
            esm.enemyBehaviors.FlyForward(esm.enemy.currentSpeed);
        }
    }
}