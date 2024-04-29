using PlayerScript;
using StateMachine;

namespace EnemyScript.Medium.MediumEnemyCommander.States.Commanding {
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
            switch (_esm.enemy.GetDistanceToPlayer) {
                case >= 12f:
                    _esm.enemyBehaviors.FlyForward(_esm.enemy.currentSpeed);
                    break;
                case <= 5f:
                    _esm.enemyBehaviors.Fly(_esm.enemy.currentSpeed, -_esm.transform.up);
                    break;
                default:
                    _esm.enemyBehaviors.Fly(_esm.enemy.currentSpeed, _esm.transform.right);
                    break;
            }
        }
    }
}