using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Core.Events;
using DG.Tweening;
using EnemyScript;
using Sirenix.OdinInspector;
using SO;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Level.Levels {
    public class LevelThreeManager : Levels.Level {
        [ReadOnly] public Enemy boss;
        public List<Dialogues> startingDialogues = new();
        public Dialogues mainDialogues;
        public List<Dialogues> winDialogues = new();
        public List<Transform> spawnPoints = new();
        [TitleGroup("Cinematic")] 
        public CinemachineVirtualCamera enemyTrackingCam;
        public CinemachineVirtualCamera playerCam;

        private bool _startedDialogue;
        private bool _hasWon;
        
        private void Start() {
            boss = FindObjectOfType<Enemy>();
            boss.gameObject.SetActive(false);
            var delay = 0f;
            foreach (var dialogue in startingDialogues) {
                this.FireEvent(EventType.OnDialoguesChange, dialogue);
                delay += dialogue.dialogues.main.Sum(t => t.readingTime);
            }
            
            DOVirtual.DelayedCall(delay, () => {
                var furthestPoint = spawnPoints
                    .OrderByDescending(x => Vector2.Distance(x.position, PlayerScript.Player.Instance.PlayerPos))
                    .ToArray()[0];
                boss.gameObject.transform.position = furthestPoint.position;
                boss.gameObject.SetActive(true);
                StartCoroutine(CameraCinematic());
            });
        }

        private void Update() {
            if (boss.currentHp / boss.hp <= 0.8f && !_startedDialogue) {
                _startedDialogue = true;
                this.FireEvent(EventType.OnDialoguesChange, mainDialogues);
            }

            if (!boss && !_hasWon) {
                _hasWon = true;
                Win();
            }
        }

        private IEnumerator CameraCinematic() {
            enemyTrackingCam.LookAt = boss.transform;
            enemyTrackingCam.Follow = boss.transform;
            yield return new WaitForSeconds(2f);
            playerCam.enabled = false;
            yield return new WaitUntil(() =>
                Vector2.Distance(Camera.main.transform.position, boss.transform.position) < 0.1f);
            yield return new WaitForSeconds(3f);
            playerCam.enabled = true;
            enemyTrackingCam.LookAt = null;
            enemyTrackingCam.Follow = null;
        }

        private void Win() {
            this.FireEvent(EventType.ClearDialogues);
            var delay = 0f;
            foreach (var dialogues in winDialogues) {
                this.FireEvent(EventType.OnDialoguesChange, dialogues);
                delay += dialogues.dialogues.main.Sum(t => t.readingTime);
            }

            DOVirtual.DelayedCall(2f, () => {
                this.FireEvent(EventType.OnGameStateChange, GameState.Win);
            });
                        
            DOVirtual.DelayedCall(delay, () => {
                this.FireEvent(EventType.OpenActualWinUI);
            }); 
        }
    }
}
