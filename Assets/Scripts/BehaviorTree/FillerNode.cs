namespace BehaviorTree {
    public class FillerNode : Action {
        protected override void OnEnter() {
        }

        protected override State OnUpdate() {
            return State.Running;
        }

        protected override void OnExit() {
        }
    }
}