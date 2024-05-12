using StateMachine;

namespace EnemyScript.Boss.States {
    public class BossCircleState : BossState {
        private float _lastHp;
        public BossCircleState(BossStateMachine.EnemyState key, StateMachine<BossStateMachine.EnemyState> stateMachine) : base(key, stateMachine) { }

        public override void OnEnter() {
            esm.enemy.currentSpeed = esm.enemy.speed;
            esm.enemy.currentAngularSpeed = esm.enemy.angularSpeed;
            _lastHp = esm.enemy.currentHp;
        }

        public override void OnExit() { }

        public override void OnUpdate() {
            var predictedPos = esm.PredictPlayerPosition(esm.predictedFrames);
            var predictedDot = esm.enemy.GetDotToPoint(predictedPos);
            
            esm.enemyBehaviors.LookAt(predictedPos, esm.enemy.angularSpeed); 
            if (esm.enemy.GetDistanceToPlayer >= 8f) {
                esm.enemy.currentSpeed = esm.enemy.speed;
                esm.enemyBehaviors.FlyForward(esm.enemy.currentSpeed);
            }
            else {
                esm.enemy.currentSpeed = esm.enemy.minimumSpeed + 5f;
                esm.enemyBehaviors.Fly(esm.enemy.currentSpeed, esm.transform.right);
            }

            if (predictedDot >= 0.98f) {
                esm.enemyWeapon.FireWeapon();
            }

            if (_lastHp > esm.enemy.currentHp) {
                esm.SwitchState(BossStateMachine.EnemyState.Reset);
            }
        }
    }
}