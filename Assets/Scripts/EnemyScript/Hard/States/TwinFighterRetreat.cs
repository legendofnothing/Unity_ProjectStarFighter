using PlayerScript;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Hard.States {
    public class TwinFighterRetreat : TwinFighterState {
        private float _timePassed;
        public TwinFighterRetreat(TwinFighterStateMachine.EnemyState key, StateMachine<TwinFighterStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _esm.enemy.currentAngularSpeed = _esm.enemy.angularSpeed;
            _timePassed = 0;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            _esm.enemyBehaviors.LookAt(Player.Instance.transform.position, _esm.enemy.angularSpeed);
            var dist = _esm.enemy.GetDistanceToPlayer;
            
            _timePassed += Time.fixedDeltaTime;
            if (_timePassed >= 4f && dist <= _esm.safeRetreatingDistance) {
                _esm.enemy.currentSpeed = _esm.enemy.speed * 2;
            }

            if (dist <= _esm.safeRetreatingDistance) {
                _esm.enemyBehaviors.Fly(_esm.enemy.currentSpeed, -_esm.transform.up);
            }
            
            else {
                _esm.SwitchState(TwinFighterStateMachine.EnemyState.Idle);
            }
        }
    }
}