using EnemyScript.v1.Easy.EnemyShoot;
using StateMachine;
using UnityEngine;

namespace EnemyScript.v2.StateMachine.States {
    public class EnemyResetState : EnemyState {
        private int _circlingDirection;
        
        public EnemyResetState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) { }
        
        public override void OnEnter() {
            _circlingDirection = Random.Range(0, 2);
            esm.enemy.currentSpeed = esm.enemy.speed;
            esm.enemy.currentAngularSpeed = esm.enemy.minimumAngularSpeed;
        }

        public override void OnExit() { }

        public override void OnUpdate() {
            var dir = _circlingDirection == 0 ? esm.transform.right : -esm.transform.right;
            esm.enemyBehaviors.LookAt(dir + esm.transform.position, esm.enemy.currentAngularSpeed);
            esm.enemyBehaviors.FlyForward(esm.enemy.speed);
        }
    }
}