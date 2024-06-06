using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Combat;
using Core.Events;
using DG.Tweening;
using EnemyScript;
using PlayerScript;
using Sirenix.OdinInspector;
using SO;
using UI;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Level.Levels {
    public class Tutorial : MonoBehaviour {
        public enum TutorialState {
            None,
            Moving,
            Shooting,
            Combat,
            Finished,
        }
        
        [TitleGroup("Dialogues")] 
        public Dialogues startingDialogues;
        public Dialogues afterMovingDialogues;
        public Dialogues afterShootingDialogues;
        public Dialogues endOfTutorialDialogues;
        
        public List<Dialogues> tutorialPrompts = new();

        [TitleGroup("Refs")]
        public DialogueUI tutorialDialogueUI;
        public PlayerController playerController;
        public CombatManager combatManager;
        
        [TitleGroup("Cinematic")] 
        public CinemachineVirtualCamera enemyTrackingCam;
        public CinemachineVirtualCamera playerCam;
        
        [TitleGroup("Spawning")] 
        public List<Transform> commanderSpawnPoints = new();
        public List<GameObject> commandersToSpawn = new();

        private List<Enemy> _enemies = new();

        private float _prevSpeed;
        private TutorialState _currentState = TutorialState.None;

        private int _enemyCount;

        private void Awake() {
            this.AddListener(EventType.OnEnemySpawned, _ => {
                _enemyCount++;
            });
            this.AddListener(EventType.OnEnemyKilled, _ => {
                _enemyCount--;
            });
        }

        private void Start() {
            this.FireEvent(EventType.OnDialoguesChange, startingDialogues);
            _prevSpeed = playerController.speed;
            playerController.speed = 0;
            combatManager.canFire = false;
            Player.Instance.overridesDie = true;

            DOVirtual.DelayedCall(startingDialogues.dialogues.main.Sum(t => t.readingTime), () => {
                tutorialDialogueUI.PlayDialogue(tutorialPrompts[0]);
                _currentState = TutorialState.Moving;
                playerController.speed = _prevSpeed;
            });
        }

        private void Update() {
            switch (_currentState) {
                case TutorialState.None:
                    break;
                case TutorialState.Moving:
                    if (Player.Instance.PlayerDir.magnitude >= playerController.speed * 0.8f) {
                        _currentState = TutorialState.None;
                        tutorialDialogueUI.StopDialogue();
                        
                        AfterAction(afterMovingDialogues, () => {
                            tutorialDialogueUI.PlayDialogue(tutorialPrompts[1]);
                            combatManager.canFire = true;
                            _currentState = TutorialState.Shooting;
                        });
                    }
                    break;
                case TutorialState.Shooting:
                    if (Input.GetKeyDown(KeyCode.Mouse0)) {
                        _currentState = TutorialState.None;
                        tutorialDialogueUI.StopDialogue();
                        AfterAction(afterShootingDialogues, () => {
                            tutorialDialogueUI.PlayDialogue(tutorialPrompts[2]); 
                            
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

                            _currentState = TutorialState.Combat;
                            StartCoroutine(CameraCinematic());
                            StartCoroutine(OnTaskForceDown());
                        });
                    }
                    break;
                case TutorialState.Combat:
                    break;
                case TutorialState.Finished:
                    _currentState = TutorialState.None;
                    tutorialDialogueUI.StopDialogue();
                    this.FireEvent(EventType.OnDialoguesChange, endOfTutorialDialogues);
                        
                    var delay = endOfTutorialDialogues.dialogues.main.Sum(t => t.readingTime);
                    DOVirtual.DelayedCall(1.5f, () => {
                        this.FireEvent(EventType.OnGameStateChange, GameState.Win);
                    });
                        
                    DOVirtual.DelayedCall(delay, () => {
                        this.FireEvent(EventType.OpenActualWinUI);
                    });
                    break;
            }
        }

        private void AfterAction(Dialogues dialogues, TweenCallback callback) {
            this.FireEvent(EventType.OnDialoguesChange, dialogues);
            DOVirtual.DelayedCall(dialogues.dialogues.main.Sum(t => t.readingTime), callback);
        }
        
        private IEnumerator OnTaskForceDown() {
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() =>_enemyCount <= 0);
            _currentState = TutorialState.Finished;
        }
        
        private IEnumerator CameraCinematic() {
            enemyTrackingCam.LookAt = _enemies[0].transform;
            enemyTrackingCam.Follow = _enemies[0].transform;
            yield return new WaitForSeconds(1.2f);
            playerCam.enabled = false;
            yield return new WaitUntil(() =>
                Vector2.Distance(Camera.main.transform.position, _enemies[0].transform.position) < 0.1f);
            yield return new WaitForSeconds(2f);
            playerCam.enabled = true;
            enemyTrackingCam.LookAt = null;
            enemyTrackingCam.Follow = null;
        }
    }
}