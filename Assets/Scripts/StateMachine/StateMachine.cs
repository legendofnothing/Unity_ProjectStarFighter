using System;
using System.Collections.Generic;
using Core;
using Core.Logging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StateMachine {
    public abstract class StateMachine<TEState> : MonoBehaviour where TEState : Enum {
        protected Dictionary<TEState, State<TEState>> states = new();
        [ShowInInspector] [ReadOnly] protected State<TEState> currentState;
        public TEState CurrentState => _currentStateKey;
        private TEState _currentStateKey;
        
        protected TEState entryStateKey;
        protected bool canRun = true;
        protected bool hasDisabled = false;
        public bool CanRun {
            get => canRun;
            set => canRun = value;
        }

        private void Awake() {
            SetupRef();
            
            entryStateKey = SetupState();
            try {
                currentState = states[entryStateKey];
                _currentStateKey = entryStateKey;
            }
            catch (Exception e) {
                NCLogger.Log(e.Message);
            }
        }

        private void Start() {
            currentState?.OnEnter();
        }

        private void Update() {
            if (!canRun) return;
            currentState?.OnUpdate();
        }

        public void SwitchState(TEState stateToSwitch) {
            var nextState = states[stateToSwitch];
            if (currentState == nextState) return;
            currentState.OnExit();
            currentState = nextState;
            currentState = nextState;
            currentState.OnEnter();
            _currentStateKey = stateToSwitch;
        }

        /// <summary>
        /// Setup tree, return the state that will be the entry
        /// </summary>
        /// <returns></returns>
        protected abstract TEState SetupState();

        protected abstract void SetupRef();
    }
}