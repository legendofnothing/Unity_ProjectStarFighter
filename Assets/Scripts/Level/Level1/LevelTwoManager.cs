using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Core.Events;
using DG.Tweening;
using EnemyScript;
using EnemyScript.TowerScript;
using Sirenix.OdinInspector;
using SO;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Level.Level1 {
    public class LevelTwoManager : MonoBehaviour {
        [ReadOnly] public List<TowerObjective> towerObjectives;
        public List<Dialogues> DialoguesList = new();
        public List<Dialogues> TaskForceDialogues = new();
        public Dialogues EndDialogues;
        [TitleGroup("Spawning")] 
        public List<Transform> commanderSpawnPoints = new();
        public List<GameObject> commandersToSpawn = new();
        [TitleGroup("Cinematic")] 
        public CinemachineVirtualCamera enemyTrackingCam;
        public CinemachineVirtualCamera playerCam;

        private int _spawnCount;
        private List<Enemy> _enemies = new();

        private void Start() {
            towerObjectives = FindObjectsOfType<TowerObjective>().ToList();
            this.FireEvent(EventType.OnDialoguesChange, DialoguesList[0]);
            this.AddListener(EventType.OnTowerDestroyed, param => OnTowerDestroy((TowerObjective)param));
            this.AddListener(EventType.OnEnemyKilled, param => OnTaskForceDown((Enemy)param));
        }
        
        private void OnTowerDestroy(TowerObjective tower) {
            if (towerObjectives.Contains(tower)) {
                towerObjectives.Remove(tower);
                this.FireEvent(EventType.OnDialoguesChange, DialoguesList[1]);
                if (towerObjectives.Count <= 0) {
                    SpawnCommanders();
                }
            }
        }

        private void SpawnCommanders() {
            this.FireEvent(EventType.ClearDialogues);

            if (_spawnCount == 0) {
                foreach (var dialogue in TaskForceDialogues) {
                    this.FireEvent(EventType.OnDialoguesChange, dialogue);
                }
            }
            else {
                this.FireEvent(EventType.OnDialoguesChange, DialoguesList[2]);
            }
            
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

            _spawnCount++;
            
            StartCoroutine(CameraCinematic());
        }
        
        private void OnTaskForceDown(Enemy enemy) {
            if (towerObjectives.Count > 0) return;
            if (_enemies.Contains(enemy)) {
                _enemies.Remove(enemy);
                if (_enemies.Count <= 0) {

                    if (_spawnCount >= 4) {
                        this.FireEvent(EventType.OnDialoguesChange, EndDialogues);
                        
                        var delay = EndDialogues.dialogues.main.Sum(t => t.readingTime);
                        DOVirtual.DelayedCall(2f, () => {
                            this.FireEvent(EventType.OnGameStateChange, GameState.Win);
                        });
                        
                        DOVirtual.DelayedCall(delay, () => {
                            this.FireEvent(EventType.OpenActualWinUI);
                        }); 
                    }

                    else {
                        DOVirtual.DelayedCall(0.2f, SpawnCommanders);
                    }
                }
            }
        }
        
        private IEnumerator CameraCinematic() {
            enemyTrackingCam.LookAt = _enemies[0].transform;
            enemyTrackingCam.Follow = _enemies[0].transform;
            yield return new WaitForSeconds(2f);
            playerCam.enabled = false;
            yield return new WaitUntil(() =>
                Vector2.Distance(Camera.main.transform.position, _enemies[0].transform.position) < 0.1f);
            yield return new WaitForSeconds(3f);
            playerCam.enabled = true;
            enemyTrackingCam.LookAt = null;
            enemyTrackingCam.Follow = null;
        }
    }
}
