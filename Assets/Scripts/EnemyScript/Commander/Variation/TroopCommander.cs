using System.Collections.Generic;
using Core.Events;
using Sirenix.OdinInspector;

namespace EnemyScript.Commander.Variation {
    public abstract class TroopCommander : Troop {
        [ReadOnly] public List<Enemy> troops = new();
        public int troopCount => troops.Count;

        protected void SendCommand(TroopCommand command) {
            this.FireEvent(EventType.SendCommand, command);
        }

        public void AddTroop(Enemy troopToAdd) {
            if (troops.Contains(troopToAdd)) return;
            troops.Add(troopToAdd);
        }

        public void RemoveTroop(Enemy troopToRemove) {
            if (!troops.Contains(troopToRemove)) return;
            troops.Remove(troopToRemove);
        }
    }
}