using System;
using EnemyScript.Commander.Variation;
using UnityEngine;

namespace EnemyScript.Commander {
    public struct TroopCommand {
        public TroopCommander.Commands command;
        public Troop commander;
    }
    
    public abstract class Troop : MonoBehaviour {
        public enum State {
            Attack,
            Command
        }
        
        public enum Commands {
            LookForTroop,
            Attack,
            Defend,
        }

        public MonoBehaviour attackState;
        public MonoBehaviour commandState;
        
        protected Troop commander;
        protected Enemy self;

        private void Start() {
            attackState.enabled = false;
            commandState.enabled = false;
            self = GetComponent<Enemy>();
            OnStart();
        }

        protected abstract void OnStart();
        public abstract void OnDeath();

        protected void SwitchState(State state) {
            switch (state) {
                case State.Attack:
                    if (!attackState.enabled) attackState.enabled = true;
                    commandState.enabled = false;
                    break;
                case State.Command:
                    if (!commandState.enabled) commandState.enabled = true;
                    attackState.enabled = false;
                    break;
            }
        }
    }
}