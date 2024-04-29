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
            CommanderDead,
        }

        public MonoBehaviour attackState;
        public MonoBehaviour commandState;
        
        protected Troop commander;
        protected Enemy self;
        protected State currentState;

        private void Start() {
            self = GetComponent<Enemy>();
            OnStart();
        }

        private void Awake() {
            OnAwake();
        }

        protected abstract void OnStart();
        protected abstract void OnAwake();
        public abstract void OnDeath();
        public abstract void OnDamage();

        public void SwitchState(State state) {
            switch (state) {
                case State.Attack:
                    currentState = State.Attack;
                    if (!attackState.enabled) attackState.enabled = true;
                    commandState.enabled = false;
                    break;
                case State.Command:
                    currentState = State.Command;
                    if (!commandState.enabled) commandState.enabled = true;
                    attackState.enabled = false;
                    break;
            }
        }
    }
}