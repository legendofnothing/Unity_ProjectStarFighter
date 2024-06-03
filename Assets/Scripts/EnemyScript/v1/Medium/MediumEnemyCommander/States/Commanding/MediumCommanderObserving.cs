using PlayerScript;
using StateMachine;

namespace EnemyScript.v1.Medium.MediumEnemyCommander.States.Commanding {
    public class MediumCommanderObserving : MediumCommanderCommandState{
        public MediumCommanderObserving(MediumCommanderCommandStateMachine.EnemyState key, StateMachine<MediumCommanderCommandStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _esm.enemy.currentSpeed = _esm.enemy.minimumSpeed;
            _esm.enemy.currentAngularSpeed = _esm.enemy.angularSpeed;
        }

        public override void OnExit() {
            
        }

        public override void OnUpdate() {
            _esm.enemy.currentSpeed = _esm.enemy.minimumSpeed;
            _esm.enemyBehaviors.LookAt(Player.Instance.transform.position, _esm.enemy.angularSpeed);
            var dist = _esm.enemy.GetDistanceToPlayer;

            if (dist >= _esm.maximumObservingDistance) {
                _esm.enemyBehaviors.FlyForward(_esm.enemy.speed - _esm.enemy.speed * 0.3f);
            }
            else if (dist <= _esm.minimumObservingDistance && dist > _esm.dangerDistance) {
                _esm.enemyBehaviors.Fly(_esm.enemy.currentSpeed, -_esm.transform.up);
            } 
        }
    }
}