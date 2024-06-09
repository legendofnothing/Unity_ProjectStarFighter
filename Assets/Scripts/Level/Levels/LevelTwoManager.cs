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

namespace Level.Levels {
    public class LevelTwoManager : Levels.Level {
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
        private List<Tower> _towers;
        private GameObject _enemyToLook;
        private int _enemyCount;

        private void Awake() {
            _towers = FindObjectsOfType<Tower>().ToList();
            this.AddListener(EventType.OnTowerDestroyed, param => OnTowerDestroy((Tower) param));
            this.AddListener(EventType.OnEnemySpawned, _ => {
                _enemyCount++;
            });
            this.AddListener(EventType.OnEnemyKilled, _ => {
                _enemyCount--;
            });
            
        }

        private void Start() {
            this.FireEvent(EventType.OnDialoguesChange, DialoguesList[0]);
        }

        private void OnTowerDestroy(Tower tower) {
            if (_towers.Contains(tower)) {
                _towers.Remove(tower);
                this.FireEvent(EventType.OnDialoguesChange, DialoguesList[1]);
                if (_towers.Count <= 0) {
                    StartCoroutine(SpawnCommanders());
                }
            }
        }

        private IEnumerator OnTaskForceDown() {
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => _enemyCount <= 0);
            yield return new WaitForSeconds(2.5f);
            
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
                DOVirtual.DelayedCall(0.2f, () => {
                    StartCoroutine(SpawnCommanders());
                });
            }
        }

        private void SpawnLogic() {
            this.FireEvent(EventType.ClearDialogues);
            if (_spawnCount == 0) {
                foreach (var dialogue in TaskForceDialogues) {
                    this.FireEvent(EventType.OnDialoguesChange, dialogue);
                }
            }
            else {
                this.FireEvent(EventType.OnDialoguesChange, DialoguesList[2]);
            }
            
            var furthestPoint = commanderSpawnPoints
                .OrderByDescending(x => Vector2.Distance(x.position, PlayerScript.Player.Instance.PlayerPos))
                .ToArray()[0];

            var randomEnemy = commandersToSpawn[Random.Range(0, commandersToSpawn.Count)];
            _enemyToLook = Instantiate(randomEnemy, furthestPoint.position, Quaternion.identity);

            StartCoroutine(CameraCinematic());
            _spawnCount++;
        }

        private IEnumerator SpawnCommanders() {
            yield return new WaitUntil(() => _enemyCount <= 0);
            yield return new WaitForSeconds(2f);
            SpawnLogic();
            StartCoroutine(OnTaskForceDown());
        }

        private IEnumerator CameraCinematic() {
            enemyTrackingCam.LookAt = _enemyToLook.transform;
            enemyTrackingCam.Follow = _enemyToLook.transform;
            yield return new WaitForSeconds(2f);
            playerCam.enabled = false;
            yield return new WaitUntil(() =>
                Vector2.Distance(Camera.main.transform.position, _enemyToLook.transform.position) < 0.1f);
            yield return new WaitForSeconds(3f);
            playerCam.enabled = true;
            enemyTrackingCam.LookAt = null;
            enemyTrackingCam.Follow = null;
        }
    }
}
