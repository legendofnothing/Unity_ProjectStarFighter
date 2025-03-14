﻿using StateMachine;

namespace EnemyScript.v1.Hard.States {
    public abstract class TwinFighterState : State<TwinFighterStateMachine.EnemyState> {
        protected TwinFighterStateMachine _esm;
        protected TwinFighterState(TwinFighterStateMachine.EnemyState key, StateMachine<TwinFighterStateMachine.EnemyState> stateMachine) : base(key, stateMachine) {
            _esm = (TwinFighterStateMachine)stateMachine;
        }
    }
}