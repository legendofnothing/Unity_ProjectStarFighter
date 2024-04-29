using System.Collections.Generic;
using Core.Events;
using Sirenix.OdinInspector;

namespace EnemyScript.Commander.Variation {
    public abstract class TroopCommander : Troop {
        [ReadOnly] public List<Troop> troops = new();
        public int troopCount => troops.Count;

        protected void SendCommand(TroopCommand command) {
            this.FireEvent(EventType.SendCommand, command);
        }

        public void AddTroop(Troop troopToAdd) {
            if (troops.Contains(troopToAdd)) return;
            troops.Add(troopToAdd);
            OnTroopAdded();
        }

        public void RemoveTroop(Troop troopToRemove) {
            if (!troops.Contains(troopToRemove)) return;
            troops.Remove(troopToRemove);
            OnTroopRemoved();
        }

        protected abstract void OnTroopAdded();
        protected abstract void OnTroopRemoved();
    }
}