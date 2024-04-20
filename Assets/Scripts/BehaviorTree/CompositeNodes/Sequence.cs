using System.Collections.Generic;

namespace BehaviorTree.CompositeNodes {
    public class Sequence : Composite {
        public Sequence(List<Node> children) : base(children) {
        }
        
        protected override State OnUpdate() {
            var isAnyChildRunning = false;
            
            foreach (var node in children) {
                switch (node.Update()) {
                    case State.Running:
                        isAnyChildRunning = true;
                        continue;
                    
                    case State.Success:
                        continue;
                    
                    case State.Failure:
                        state = State.Failure;
                        return state;
                        
                    default:
                        state = State.Success;
                        return state;
                }
            }

            state = isAnyChildRunning ? State.Running : State.Success;
            return state;
        }

        protected override void OnEnter() { }
        protected override void OnExit() { }
    }
}