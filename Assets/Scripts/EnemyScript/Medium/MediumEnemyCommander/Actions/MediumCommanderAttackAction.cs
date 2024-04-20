using BehaviorTree;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander.Actions {
    public class MediumCommanderAttackAction : Action {
        public MediumCommanderAttackStateMachine attackSM;
        
        public MediumCommanderAttackAction(Blackboard blackboard) : base(blackboard) {
            attackSM = (MediumCommanderAttackStateMachine)base.blackboard.GetData("attackSM");
        }
        
        protected override void OnEnter() {
            attackSM.CanRun = true;
        }

        protected override State OnUpdate() {
            Debug.Log($"Running Attack / state success");
            return State.Failure;
        }

        protected override void OnExit() {
        }
    }
}