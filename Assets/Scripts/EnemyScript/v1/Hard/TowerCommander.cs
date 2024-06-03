using System.Collections.Generic;
using BehaviorTree;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using EnemyScript.TowerScript;

namespace EnemyScript.v1.Hard {
    public class TowerCommander : TroopCommander {
        private TowerObjective _tower;
        private global::BehaviorTree.BehaviorTree _bt;
        private bool _isDamageTaken;

        #region Unused
        protected override void OnAwake() { }
        public override void OnDeath() { }
        protected override void OnTroopAdded() { }
        protected override void OnTroopRemoved() { }
        #endregion

        protected override void OnStart() {
            _tower = GetComponent<TowerObjective>();
            _bt = new global::BehaviorTree.BehaviorTree(new List<Node> {
                new Selector(new List<Node> {
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => troops.Count <= 0 && _tower.isPlayerInRange)),
                        new Decorator(new Actions(() => {
                            var list = _tower.SpawnTroopDirectly();
                            foreach (var troop in list) {
                                AddTroop(troop);
                                troop.commander = this;
                            }
                        }))
                    }),
                    
                   new Sequence(new List<Node> {
                       new Decorator(new Condition(() => _isDamageTaken)),
                       new Decorator(new Actions(() => {
                           _isDamageTaken = false;
                           SendCommand(new TroopCommand {
                               commander = this,
                               command = Commands.Attack
                           });
                       }))
                   }) 
                })
            });
        }

        public override void OnDamage() {
            _isDamageTaken = true;  
        }
        
        private void Update() {
            _bt.Update();
        }
    }
}
