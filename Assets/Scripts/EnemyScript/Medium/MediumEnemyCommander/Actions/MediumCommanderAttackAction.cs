using BehaviorTree;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander.Actions {
    public class MediumCommanderAttackAction : Action {
        public MediumCommanderAttackAction(Blackboard blackboard) : base(blackboard) {
            
        }
        
        protected override void OnEnter() {
        }

        protected override State OnUpdate() {
            Debug.Log("Running Attack");
            return State.Running;
        }

        protected override void OnExit() {
        }
    }
}