namespace BehaviorTree {
    public abstract class Action : Node {
        protected object param;
        protected Blackboard blackboard;
        
        public Action() { }
        
        public Action(object param) {
            this.param = param;
        }

        public Action(Blackboard blackboard) {
            this.blackboard = blackboard;
        }
    }
}