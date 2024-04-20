using BehaviorTree;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander.Actions {
    public class MediumCommanderCommandAction : Action {
        public MediumCommanderCommandAction(Blackboard blackboard) : base(blackboard) {
            
        }
        
        protected override void OnEnter() {
        }

        protected override State OnUpdate() {
            Debug.Log("Running Command");
            return State.Failure;
        }

        protected override void OnExit() {
        }
    }
}