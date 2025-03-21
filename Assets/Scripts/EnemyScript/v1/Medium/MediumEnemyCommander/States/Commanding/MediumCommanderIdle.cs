﻿using StateMachine;

namespace EnemyScript.v1.Medium.MediumEnemyCommander.States.Commanding {
    public class MediumCommanderIdle : MediumCommanderCommandState {
        public MediumCommanderIdle(MediumCommanderCommandStateMachine.EnemyState key, StateMachine<MediumCommanderCommandStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
        }

        public override void OnEnter() {
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {

        }
    }
}