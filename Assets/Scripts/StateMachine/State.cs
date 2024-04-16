using System;

namespace StateMachine {
    public abstract class State<EState> where EState : Enum {
        public EState key;

        public State(EState key, StateMachine<EState> stateMachine) {
            this.key = key;
        }
        
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate();
    }
}