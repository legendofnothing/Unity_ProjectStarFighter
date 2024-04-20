using System.Collections.Generic;

namespace BehaviorTree.CompositeNodes {
    public class Selector : Composite {
        public Selector(List<Node> children) : base(children) { }
        
        protected override State OnUpdate() {
            state = State.Failure;
            
            foreach (var node in children) {
                switch (node.Update()) {
                    case State.Running:
                        state = State.Running;
                        return state;
                    
                    case State.Success:
                        state = State.Success;
                        return state;
                    
                    case State.Failure:
                        continue;
                        
                    default:
                        continue;
                }
            }
            
            return state;
        }
        
        protected override void OnEnter() { }
        protected override void OnExit() { }
    }
}