using StateMachine;
using UnityEngine;

namespace EnemyScript.Easy.EnemyShoot.States {
    public class EnemyAttack : EnemyState {
        public EnemyAttack(EnemyStateMachine.EnemyState key, StateMachine<EnemyStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            //1 full on
            //0.5 side profile
            var dot = _enemyStateMachine.enemy.GetDotToPlayer;
            
            if (dot > 0.98f) {
                _enemyStateMachine.enemyBehaviors.FireWeapon();
            }

            if (dot > 0.98f && Vector3.Distance(_enemyStateMachine.transform.position,
                    PlayerScript.Player.Instance.transform.position) <= _enemyStateMachine.minimumDistance) {
                _enemyStateMachine.SwitchState(EnemyStateMachine.EnemyState.Circling);
            }
            else {
                _enemyStateMachine
                    .enemyBehaviors.LookAt(PlayerScript.Player.Instance.transform.position, _enemyStateMachine.enemy.angularSpeed); 
            }
            
            var lerpedSpeed = Mathf.Lerp(_enemyStateMachine.enemy.minimumSpeed, _enemyStateMachine.enemy.speed, Mathf.Clamp(dot, 0f, 1f));
            _enemyStateMachine.enemy.currentSpeed = lerpedSpeed;
            _enemyStateMachine
                .enemyBehaviors.FlyForward(_enemyStateMachine.enemy.currentSpeed); 
        }
    }
}