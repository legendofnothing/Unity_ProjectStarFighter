using System.Collections.Generic;
using Core.Events;
using Sirenix.OdinInspector;

namespace EnemyScript.Commander.Variation {
    public abstract class TroopCommander : Troop {
        public List<Troop> troopsToSpawn = new();
        [ReadOnly] public List<Troop> troops = new();
        public int troopCount => troops.Count;

        protected void SendCommand(TroopCommand command) {
            this.FireEvent(EventType.SendCommand, command);
        }

        public void AddTroop(Troop troopToAdd) {
            if (troops.Contains(troopToAdd)) return;
            troops.Add(troopToAdd);
            OnTroopAdded();
            OnTroopAdded(troopToAdd);
        }

        public void RemoveTroop(Troop troopToRemove) {
            if (!troops.Contains(troopToRemove)) return;
            troops.Remove(troopToRemove);
            OnTroopRemoved();
            OnTroopRemoved(troopToRemove);
        }

        protected abstract void OnTroopAdded();
        protected abstract void OnTroopRemoved();

        protected virtual void OnTroopAdded(Troop troop) {
            
        }

        protected virtual void OnTroopRemoved(Troop troop) {
            
        }
    }
}