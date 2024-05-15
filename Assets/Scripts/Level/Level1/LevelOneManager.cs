using System.Collections.Generic;
using System.Linq;
using Core.Events;
using DG.Tweening;
using EnemyScript;
using EnemyScript.TowerScript;
using SO;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Level.Level1 {
    public class LevelOneManager : MonoBehaviour {
        public List<Tower> towers = new();
        [Space] 
        public List<Transform> commanderSpawnPoints = new();
        public List<GameObject> commandersToSpawn = new();
        [Space] 
        public Dialogues startingDialogues;
        public Dialogues towerDestroyedDialogues;
        public Dialogues commanderAppearsDialogues;
        public Dialogues enemyCommanderDialogues;
        public Dialogues endOfMissionDialogues;

        private List<Enemy> _enemies = new();

        private void Start() {
            this.AddListener(EventType.OnTowerDestroyed, param => OnTowerDestroy((Tower) param));
            this.AddListener(EventType.OnEnemyKilled, param => OnTaskForceDown((Enemy)param));
            
            this.FireEvent(EventType.OnDialoguesChange, startingDialogues);
        }

        private void OnTowerDestroy(Tower tower) {
            if (towers.Contains(tower)) {
                towers.Remove(tower);
                this.FireEvent(EventType.OnDialoguesChange, towerDestroyedDialogues);
                if (towers.Count <= 0) {
                    SpawnCommanders();
                }
            }
        }

        private void OnTaskForceDown(Enemy enemy) {
            if (towers.Count > 0) return;
            if (_enemies.Contains(enemy)) {
                _enemies.Remove(enemy);
                if (_enemies.Count <= 0) {
                    this.FireEvent(EventType.OnDialoguesChange, endOfMissionDialogues);
                    var delay = endOfMissionDialogues.dialogues.main.Sum(t => t.readingTime);
                    DOVirtual.DelayedCall(delay + 1f, () => {
                        this.FireEvent(EventType.OnGameStateChange, GameState.Win);
                    });
                }
            }
        }

        private void SpawnCommanders() {
            this.FireEvent(EventType.OnDialoguesChange, commanderAppearsDialogues);
            this.FireEvent(EventType.OnDialoguesChange, enemyCommanderDialogues);
            Vector3 lastPos;
            var furthestPoint = commanderSpawnPoints
                .OrderByDescending(x => Vector2.Distance(x.position, PlayerScript.Player.Instance.PlayerPos))
                .ToArray()[0];
            lastPos = furthestPoint.position;
            foreach (var e in commandersToSpawn) {
                var inst = Instantiate(e, lastPos, Quaternion.identity);
                _enemies.Add(inst.GetComponent<Enemy>());
                lastPos += new Vector3(1.5f, 0);
            }
        }
    }
}