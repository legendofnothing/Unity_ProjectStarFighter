using BehaviorTree;
using EnemyScript.Commander;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander.Actions {
    public class MediumCommanderCommandAction2 : Action {
        protected TroopManager _troopManager;
        
        public MediumCommanderCommandAction2(Blackboard blackboard) : base(blackboard) {
            _troopManager = (TroopManager) base.blackboard.GetData("troopManager");
        }
        
        protected override void OnEnter() {
            state = State.Running;
            if (_troopManager.troopCount <= 0) {
                _troopManager.LookingForAvailableTroop();
                if (_troopManager.troopCount <= 0) {
                    state = State.Success;
                }
            }
        }

        protected override State OnUpdate() {
            Debug.Log($"Running Command2 / state: {state}");
            return state;
        }

        protected override void OnExit() {
        }
    }
}