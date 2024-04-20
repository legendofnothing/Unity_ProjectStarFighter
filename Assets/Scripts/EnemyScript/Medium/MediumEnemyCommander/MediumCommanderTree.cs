using System.Collections.Generic;
using BehaviorTree;
using BehaviorTree.CompositeNodes;
using EnemyScript.Medium.MediumEnemyCommander.Actions;

namespace EnemyScript.Medium.MediumEnemyCommander {
    public class MediumCommanderTree : Tree {
        protected override Node SetupTree() {
            blackboard = new Blackboard(new List<string>() {
                "enemy",
                "enemyBehavior"
            });

            blackboard.SetData("enemy", GetComponent<Enemy>());
            blackboard.SetData("enemyBehavior", GetComponent<EnemyBehaviors>());
            
            var root = new Selector(new List<Node>{
                new Sequence(new List<Node> {
                    new MediumCommanderCommandAction(blackboard),
                    new MediumCommanderAttackAction(blackboard)
                })
            });
            return root;
        }
    }
}