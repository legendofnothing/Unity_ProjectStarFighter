using PlayerScript;
using StateMachine;
using UnityEngine;

namespace EnemyScript.v2.StateMachine.States {
    public class EnemyCircleState : EnemyState {
        public EnemyCircleState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) { }
        
        public override void OnEnter() {
            esm.enemy.currentSpeed = esm.enemy.speed;
            esm.enemy.currentAngularSpeed = esm.enemy.angularSpeed;
        }

        public override void OnExit() {
            
        }

        public override void OnUpdate() {

            var predictedPos = esm.GetPredictedPos();
            var predictedDot = esm.enemy.GetDotToPoint(predictedPos);
            
            esm.enemyBehaviors.LookAt(predictedPos, esm.enemy.angularSpeed); 
            
            if (esm.enemy.GetDistanceToPlayer >= esm.minCirclingDistance) {
                esm.enemy.currentSpeed = esm.enemy.speed;
                esm.enemyBehaviors.FlyForward(esm.enemy.currentSpeed);
            }
            else {
                esm.enemy.currentSpeed = esm.enemy.minimumSpeed;
                esm.enemyBehaviors.Fly(esm.enemy.currentSpeed, esm.transform.right);
            }

            if (predictedDot >= UniversalSetting.DotToShoot) {
                esm.enemyBehaviors.FireWeapon();
            }
        }
    }
}