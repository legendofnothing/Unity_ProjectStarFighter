using PlayerScript;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyTroop.States {
    public class EnemyTroopObserving : EnemyCommandState {
        public EnemyTroopObserving(EnemyTroopStateMachine.EnemyState key, StateMachine<EnemyTroopStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _esm.enemy.currentSpeed = _esm.enemy.minimumSpeed;
            _esm.enemy.currentAngularSpeed = _esm.enemy.angularSpeed;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            _esm.enemyBehaviors.LookAt(Player.Instance.transform.position, _esm.enemy.angularSpeed);
            var dist = _esm.enemy.GetDistanceToPlayer;

            if (dist >= _esm.maximumObservingDistance) {
                _esm.enemyBehaviors.FlyForward(_esm.enemy.currentSpeed);
            }
            else if (dist <= _esm.minimumObservingDistance && dist > _esm.dangerDistance) {
                _esm.enemyBehaviors.Fly(_esm.enemy.currentSpeed, -_esm.transform.up);
            } 
            
            if (dist <= _esm.dangerDistance) {
                _esm.enemy.currentSpeed = _esm.enemy.speed;
            } else _esm.enemy.currentSpeed = _esm.enemy.minimumSpeed;
        }
    }
}