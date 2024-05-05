using System;

namespace BehaviorTree {
    public interface IExecution {
        public Node.State Update();
        public void Reset() { }
        public void OnEnter() { }
        public void OnExit() { }
    }
    
    //Single Condition 
    public class Condition : IExecution {
        private readonly Func<bool> _predicate;

        public Condition(Func<bool> predicate) {
            _predicate = predicate; 
        }

        public Node.State Update() {
            return _predicate() ? Node.State.Success : Node.State.Failure;
        }
    }
    
    //Fire and Forget
    public class Action : IExecution {
        private readonly System.Action _action;
            
        public Action(System.Action action) {
            _action = action;
        }
            
        public Node.State Update() {
            _action();
            return Node.State.Success;
        }
    }
}