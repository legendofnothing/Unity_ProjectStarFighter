using StateMachine;

namespace EnemyScript.Medium.MediumEnemyCommander.States {
    public class MediumCommanderCircling : MediumCommanderState {
        public MediumCommanderCircling(MediumCommanderAttackStateMachine.EnemyState key, StateMachine<MediumCommanderAttackStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _esm.enemy.currentSpeed = _esm.enemy.speed;
            _esm.enemy.currentAngularSpeed = _esm.enemy.minimumAngularSpeed;
        }

        public override void OnExit() {
            
        }

        public override void OnUpdate() {
            var predictedPos = _esm.PredictPlayerPosition(_esm.predictedFrames);
            var predictedDot = _esm.enemy.GetDotToPoint(predictedPos);
            _esm.enemyBehaviors.LookAt(predictedPos, _esm.enemy.angularSpeed); 
            if (_esm.enemy.GetDistanceToPlayer >= 4f) {
                _esm.enemyBehaviors.FlyForward(_esm.enemy.currentSpeed);
            }
            else {
                _esm.enemyBehaviors.Fly(_esm.enemy.currentSpeed, _esm.transform.right);
            }
        }
    }
}