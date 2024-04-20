using System.Collections.Generic;

namespace BehaviorTree {
    public abstract class Node {
        public enum State {
            Running,
            Success,
            Failure 
        }
        
        protected State state = State.Running;
        protected bool hasNodeStarted; 
        
        public State Update() {
            if (!hasNodeStarted) {
                OnEnter();
                hasNodeStarted = true;
            }

            state = OnUpdate();

            if (state is State.Failure or State.Success) {
                OnExit();
                hasNodeStarted = false;
            }

            return state;
        }

        protected abstract void OnEnter();
        protected abstract State OnUpdate();
        protected abstract void OnExit();
    }
}