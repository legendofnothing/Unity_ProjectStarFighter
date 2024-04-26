using System;
using EnemyScript.Commander.Variation;
using UnityEngine;

namespace EnemyScript.Commander {
    public struct TroopCommand {
        public TroopCommander.Commands command;
        public Enemy commander;
    }
    
    public abstract class Troop : MonoBehaviour {
        public enum State {
            Attack,
            Command
        }
        
        public enum Commands {
            LookForTroop,
            Attack,
        }

        public MonoBehaviour attackState;
        public MonoBehaviour commandState;
        
        protected Enemy commander;
        protected Enemy self;

        private void Start() {
            attackState.enabled = false;
            commandState.enabled = false;
            OnStart();
            self = GetComponent<Enemy>();
        }

        protected abstract void OnStart();

        protected void SwitchState(State state) {
            switch (state) {
                case State.Attack:
                    attackState.enabled = true;
                    commandState.enabled = false;
                    break;
                case State.Command:
                    commandState.enabled = true;
                    attackState.enabled = false;
                    break;
            }
        }
    }
}