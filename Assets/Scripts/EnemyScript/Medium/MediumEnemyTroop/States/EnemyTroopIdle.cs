using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyTroop.States {
    public class EnemyTroopIdle : EnemyCommandState {
        public EnemyTroopIdle(EnemyTroopStateMachine.EnemyState key, StateMachine<EnemyTroopStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            
        }
    }
}