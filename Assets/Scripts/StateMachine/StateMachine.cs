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
        protected TEState entryStateKey;
        protected bool canRun = true;
        public bool CanRun {
            get => canRun;
            set => canRun = value;
        }

        private void Start() {
            SetupRef();
            
            entryStateKey = SetupState();
            try {
                currentState = states[entryStateKey];
                currentState?.OnEnter();
            }
            catch (Exception e) {
                NCLogger.Log(e.Message);
            }
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
        }
        
        /// <summary>
        /// Setup tree, return the state that will be the entry
        /// </summary>
        /// <returns></returns>
        protected abstract TEState SetupState();

        protected abstract void SetupRef();
    }
}