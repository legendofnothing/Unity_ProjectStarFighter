using StateMachine;
using UnityEngine;

namespace EnemyScript.Hard.States {
    public class TwinFighterIdle : TwinFighterState {
        public TwinFighterIdle(TwinFighterStateMachine.EnemyState key, StateMachine<TwinFighterStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _esm.enemy.currentSpeed = 0;
            _esm.enemyBehaviors.ResetVelocity();
        }

        public override void OnExit() {
            
        }

        public override void OnUpdate() {
            var dist = _esm.enemy.GetDistanceToPlayer;
            if (dist <= _esm.distanceInView) {
                _esm.SwitchState(TwinFighterStateMachine.EnemyState.Attack);
            } else if (!_esm.twinFighter.commander) {
                _esm.SwitchState(TwinFighterStateMachine.EnemyState.Attack);
            }
        }
    }
}