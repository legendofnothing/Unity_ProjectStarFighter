using System.Collections.Generic;
using Core.Events;
using Unity.Collections;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace EnemyScript.Commander {
    public class TroopManager : MonoBehaviour {
        [ReadOnly] public List<Enemy> troops = new();

        private void Start() {
            this.AddListener(EventType.JoinCommander, param => ReceiveTroop((Enemy) param)); 
        }

        public void LookingForAvailableTroop() {
            this.FireEvent(EventType.LookForTroop);
        }

        private void ReceiveTroop(Enemy troop) {
            troops.Add(troop);
        }
    }
}