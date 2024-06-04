using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using EnemyScript.TowerScript;
using EnemyScript.v2.BehaviorTree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.v2.TwinFighter {
    public class TwinFighterV2 : TroopGoon {
        [TitleGroup("Twin Fighter Refs")]
        public DefaultBehavior AttackNoCommanderBehavior;
        public DefaultBehavior AttackTowerCommanderBehavior;
        
        protected override void OnStart() {
            AttackNoCommanderBehavior.canRun = false;
            AttackTowerCommanderBehavior.canRun = false;
            if (commander) {
                if (commander.gameObject.TryGetComponent<Tower>(out _)) {
                    AttackTowerCommanderBehavior.canRun = true;
                }
            }
            else {
                AttackNoCommanderBehavior.canRun = true;
            }
        }

        public override void OnDamage() {
            
        }

        protected override void RespondToCall(TroopCommand command) {
            
        }
    }
}