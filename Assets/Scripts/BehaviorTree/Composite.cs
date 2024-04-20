using System.Collections.Generic;

namespace BehaviorTree {
    public abstract class Composite : Node {
        protected List<Node> children = new();
        
        public Composite(List<Node> children) {
            foreach (var child in children) {
                this.children.Add(child);
            }
        }
    }
}