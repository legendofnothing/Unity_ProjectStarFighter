using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander.States {
    public class MediumCommanderStrafe : MediumCommanderState {
        public MediumCommanderStrafe(MediumCommanderAttackStateMachine.EnemyState key, StateMachine<MediumCommanderAttackStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
            _esm.enemy.angularSpeed = _esm.enemy.minimumAngularSpeed;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            var dot = _esm.enemy.GetDotToPlayer;
            if (dot > 0.98f && Vector3.Distance(_esm.transform.position,
                    PlayerScript.Player.Instance.transform.position) <= _esm.minimumSafeDistance) {
                //Change State
            }
            else {
                _esm.enemyBehaviors.LookAt(PlayerScript.Player.Instance.transform.position, _esm.enemy.angularSpeed); 
            }
        }
    }
}