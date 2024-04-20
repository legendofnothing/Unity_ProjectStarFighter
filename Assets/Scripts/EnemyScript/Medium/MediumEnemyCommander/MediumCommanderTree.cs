using System.Collections.Generic;
using BehaviorTree;
using BehaviorTree.CompositeNodes;
using Core.Logging;
using EnemyScript.Commander;
using EnemyScript.Medium.MediumEnemyCommander.Actions;

namespace EnemyScript.Medium.MediumEnemyCommander {
    public class MediumCommanderTree : Tree {
        public MediumCommanderAttackStateMachine attackSM;
        
        protected override Node SetupTree() {
            if (!attackSM) {
                NCLogger.Log($"Missing SM", LogLevel.ERROR);
                return null;
            }

            attackSM.CanRun = false;
            
            blackboard = new Blackboard(new List<string>() {
                "attackSM",
                "troopManager"
            });

            blackboard.SetData("attackSM", attackSM);
            blackboard.SetData("troopManager", GetComponent<TroopManager>());
            
            var root = new Selector(new List<Node>{
                new Sequence(new List<Node>() {
                    new MediumCommanderCommandAction(blackboard),
                }),

                new Sequence(new List<Node>() {
                    new MediumCommanderCommandAction2(blackboard),
                })
            });
            return root;
        }
    }
}