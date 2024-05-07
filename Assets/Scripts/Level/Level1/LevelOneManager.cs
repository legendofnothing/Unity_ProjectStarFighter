using System.Collections.Generic;
using System.Linq;
using Core.Events;
using EnemyScript.TowerScript;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Level.Level1 {
    public class LevelOneManager : MonoBehaviour {
        public List<Tower> towers = new();

        private void Start() {
            this.AddListener(EventType.OnTowerDestroyed, param => OnTowerDestroy((Tower) param));
        }

        private void OnTowerDestroy(Tower tower) {
            if (towers.Contains(tower)) {
                towers.Remove(tower);
                if (towers.Count <= 0) {
                    this.FireEvent(EventType.OnGameStateChange, GameState.Win);
                }
            }
        }
    }
}