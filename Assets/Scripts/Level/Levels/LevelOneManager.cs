using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Core.Events;
using DG.Tweening;
using EnemyScript;
using EnemyScript.TowerScript;
using EnemyScript.v2.Commander;
using Sirenix.OdinInspector;
using SO;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Level.Level1 {
    public class LevelOneManager : Levels.Level {
        [TitleGroup("Spawning")] 
        public List<Transform> commanderSpawnPoints = new();
        public List<GameObject> commandersToSpawn = new();
        [TitleGroup("Cinematic")] 
        public CinemachineVirtualCamera enemyTrackingCam;
        public CinemachineVirtualCamera playerCam;

        [TitleGroup("Dialogues")] 
        public List<Dialogues> DialoguesList = new();

        private int _enemyCount;
        private GameObject _enemyToLook;
        private List<Tower> _towers = new();

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
                this.FireEvent(EventType.ClearDialogues);
                this.FireEvent(EventType.OnDialoguesChange, DialoguesList[1]);
                if (_towers.Count <= 0) {
                    StartCoroutine(SpawnCommanders());
                }
            }
        }

        private IEnumerator OnTaskForceDown() {
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => _enemyCount <= 0);
            yield return new WaitForSeconds(1.5f);
            this.FireEvent(EventType.ClearDialogues);
            this.FireEvent(EventType.OnDialoguesChange, DialoguesList[4]);
                    
            var delay = DialoguesList[4].dialogues.main.Sum(t => t.readingTime);
            DOVirtual.DelayedCall(1.2f, () => {
                this.FireEvent(EventType.OnGameStateChange, GameState.Win);
            });

            DOVirtual.DelayedCall(delay, () => {
                this.FireEvent(EventType.OpenActualWinUI);
            }); 
        }

        private IEnumerator SpawnCommanders() {
            yield return new WaitUntil(() => _enemyCount <= 0);

            yield return new WaitForSeconds(1.5f);
            this.FireEvent(EventType.OnDialoguesChange, DialoguesList[2]);
            this.FireEvent(EventType.OnDialoguesChange, DialoguesList[3]);
            
            var furthestPoint = commanderSpawnPoints
                .OrderByDescending(x => Vector2.Distance(x.position, PlayerScript.Player.Instance.PlayerPos))
                .ToArray()[0];

            var randomEnemy = commandersToSpawn[Random.Range(0, commandersToSpawn.Count)];
            _enemyToLook = Instantiate(randomEnemy, furthestPoint.position, Quaternion.identity);

            StartCoroutine(CameraCinematic());
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