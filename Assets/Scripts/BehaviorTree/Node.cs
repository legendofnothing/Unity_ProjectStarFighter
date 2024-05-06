using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree {
    #region Behavior Tree
    
        public class BehaviorTree : Node {
            public BehaviorTree(List<Node> children) : base(children) { }

            public override State Update() {
                while (currentChild < children.Count) {
                    var status = children[currentChild].Update();
                    if (status != State.Success) {
                        return status;
                    }

                    GoNextNode();
                }

                Reset();
                return State.Success;
            }
            
            public override void OnEnter() { }
            public override void OnExit() { }
        }
        
    #endregion

    #region Composite Nodes
        public class Sequence : Node {
            private bool _abortSelf;
            public Sequence(List<Node> children, bool abortSelf = false) : base(children) {
                _abortSelf = abortSelf;
            }

            public override State Update() {
                if (currentChild < children.Count) {
                    switch (children[currentChild].Update()) {
                        case State.Running:
                            return State.Running;
                        case State.Failure:
                            if (_abortSelf) {
                                Reset();
                                return State.Success;
                            }
                            
                            Reset();
                            return State.Failure;   
                        case State.Success:
                            GoNextNode();
                            return currentChild == children.Count ? State.Success : State.Running;
                    }
                }
                
                Reset();
                return State.Success;
            }
            
            public override void OnEnter() { }
            public override void OnExit() { }
        }

        public class Selector : Node {
            public Selector(List<Node> children) : base(children) { }
            
            public override State Update() {
                if (currentChild < children.Count) {
                    switch (children[currentChild].Update()) {
                        case State.Running:
                            return State.Running;
                        case State.Success:
                            Reset();
                            return State.Success;
                        case State.Failure:
                            GoNextNode();
                            return State.Running;
                    }
                }
                
                Reset();
                return State.Success;
            }

            public override void OnEnter() { }
            public override void OnExit() { }
        }
        
    #endregion

    #region Node

    public class Decorator : Node {
        private readonly IExecution _execution;
        private readonly MonoBehaviour _monoBehaviour; // Assign this if want to execute coroutines

        public Decorator(IExecution execution, MonoBehaviour monoBehaviour = null) : base(null) {
            _execution = execution;
            _monoBehaviour = monoBehaviour;
        }

        public override State Update() {
            return _execution.Update();
        }

        public override void Reset() {
            _execution.Reset();
        }

        public override void OnEnter() {
            _execution.OnEnter();
        }

        public override void OnExit() {
            _execution.OnExit();
        }
    }

    public abstract class Node {
        public enum State {
            Success,
            Failure,
            Running
        }

        public readonly List<Node> children = new();
        protected int currentChild;

        public Node(List<Node> children) {
            this.children = children;
        }
        
        public virtual State Update() {
            return children[currentChild].Update();
        }

        public abstract void OnEnter();
        public abstract void OnExit();

        public virtual void Reset() {
            currentChild = 0;
            foreach (var child in children) {
                child.Reset();
            }
        }
        
        public void GoNextNode() {
            children[currentChild].OnExit();
            currentChild++;
            if (currentChild < children.Count) {
                children[currentChild].OnEnter();
            }
        }
    }

    #endregion
}