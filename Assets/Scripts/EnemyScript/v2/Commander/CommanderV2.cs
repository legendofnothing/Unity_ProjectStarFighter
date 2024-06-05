using System.Collections.Generic;
using Core.Events;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace EnemyScript.v2.Commander {
    public class CommanderV2 : TroopCommander {
        protected override void OnStart() {
            SendCommand(new TroopCommand {
                command = Commands.Command,
                commander = this
            });
        }

        protected override void OnAwake() {
            if (troopsToSpawn.Count <= 0) return;
            var ogPosition = (Vector2)transform.position + Vector2.one;
            foreach (var troop in troopsToSpawn) {
                var inst = Instantiate(troop, ogPosition, transform.rotation);
                var tr = inst.GetComponent<Troop>();
                
                AddTroop(tr);
                tr.commander = this;
            }
        }
        public override void OnDeath() { }
        public override void OnDamage() { }
        protected override void OnTroopAdded() { }
        protected override void OnTroopRemoved() { }
    }
}