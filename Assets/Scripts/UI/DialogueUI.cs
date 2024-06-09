using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Core.Events;
using DG.Tweening;
using SO;
using TMPro;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace UI {
    public class DialogueUI : MonoBehaviour {
        public TextMeshProUGUI handleName;
        public TextMeshProUGUI messageText;
        public CanvasGroup canvasGroup;
        public bool doManually;

        private bool _isPlayingDialogues;
        private List<Dialogues> _queue = new();

        private Action<object> _action1;
        private Action<object> _action2;

        private void Awake() {
            canvasGroup.alpha = 0;
            if (doManually) return;

            _action1 = param => {
                if (_isPlayingDialogues) {
                    _queue.Add((Dialogues)param);
                }
                else {
                    PlayDialogue((Dialogues)param);
                }
            };

            _action2 = _ => StopDialogue();
            
            this.AddListener(EventType.OnDialoguesChange, _action1);
            this.AddListener(EventType.ClearDialogues, _action2);
        }

        public void PlayDialogue(Dialogues dialogues, bool realTime = false) {
            Debug.Log("Played Dialogues - Inner");
            Debug.Log($"{dialogues}");
            if (_isPlayingDialogues) {
                _queue.Add(dialogues);
            }
            else {
                StartCoroutine(PlayDialoguesRoutine(dialogues, realTime));
            }
        }

        public void StopDialogue() {
            _isPlayingDialogues = false;
            canvasGroup.alpha = 0;
            StopAllCoroutines();
            _queue.Clear();
        }

        public void ChangeTextColor(Color color) {
            messageText.color = color;
        }

        private IEnumerator PlayDialoguesRoutine(Dialogues dialogues, bool realTime = false) {
            _isPlayingDialogues = true;
            canvasGroup.alpha = 1;
            
            handleName.text = dialogues.dialogues.handleName;
            handleName.color = dialogues.dialogues.handleColor;
            
            foreach (var text in dialogues.dialogues.main) {
                messageText.text = text.text;
                if (text.optionalAudio != null) {
                    AudioManager.Instance.PlaySFX(text.optionalAudio);
                } 
                
                if (realTime) {
                    yield return new WaitForSecondsRealtime(text.readingTime);
                }
                else {
                    yield return new WaitForSeconds(text.readingTime);
                }
            }

            if (_queue.Count > 0) {
                StartCoroutine(PlayDialoguesRoutine(_queue[0], realTime));
                _queue.RemoveAt(0);
                yield break;
            }

            canvasGroup.alpha = 0;
            _isPlayingDialogues = false;
        }
    }
}