using PlayerScript;
using StateMachine;

namespace EnemyScript.v2.StateMachine.States {
    public class EnemyObservingState : EnemyState {
        public EnemyObservingState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            esm.enemy.currentSpeed = 0.1f;
            esm.enemy.currentAngularSpeed = esm.enemy.angularSpeed;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            var predictedPos = esm.GetPredictedPos();
            var predictedDot = esm.enemy.GetDotToPoint(predictedPos);
            
            esm.enemyBehaviors.LookAt(predictedPos, esm.enemy.angularSpeed); 
            
            if (esm.enemy.GetDistanceToPlayer >= esm.minObservingDistance) {
                esm.enemy.currentSpeed = esm.enemy.speed;
                esm.enemyBehaviors.FlyForward(esm.enemy.currentSpeed);
            } 
            else if (esm.enemy.GetDistanceToPlayer < esm.minObservingDistance - 0.5f) {
                esm.enemy.currentSpeed = esm.enemy.minimumSpeed;
                esm.enemyBehaviors.Fly(esm.enemy.currentSpeed,-esm.transform.up);
            }
            else {
                esm.enemy.currentSpeed = 0.1f;
                esm.enemyBehaviors.Fly(esm.enemy.currentSpeed, esm.transform.right);
            }

            if (predictedDot >= UniversalSetting.DotToShoot && esm.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) <= esm.minEngageDistance) {
                esm.enemyBehaviors.FireWeapon();
            }
        }
    }
}