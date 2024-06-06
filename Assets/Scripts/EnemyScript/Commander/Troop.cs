using System;
using EnemyScript.Commander.Variation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.Commander {
    public struct TroopCommand {
        public TroopCommander.Commands command;
        public Troop commander;
    }
    
    public abstract class Troop : MonoBehaviour {
        public enum State {
            None,
            Attack,
            Command
        }
        
        public enum Commands {
            None,
            LookForTroop,
            Attack,
            Command,
            CommanderDead,
        }

        public MonoBehaviour attackState;
        public MonoBehaviour commandState;
        public float powerValue;
        [Space]
        [ReadOnly] public Troop commander;
        protected Enemy self;
        [ReadOnly] public State currentState;

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
            if (!attackState && !commandState) return;
            switch (state) {
                case State.Attack:
                    currentState = State.Attack;
                    if (!attackState.enabled) attackState.enabled = true;
                    if (commandState) commandState.enabled = false;
                    break;
                case State.Command:
                    currentState = State.Command;
                    if (!commandState.enabled) commandState.enabled = true;
                    if (attackState) attackState.enabled = false;
                    break;
            }
        }

        public void DisableAllState() {
            currentState = State.None;
            if (attackState.enabled) attackState.enabled = false;
            if (commandState.enabled) commandState.enabled = false;
        }
    }
}