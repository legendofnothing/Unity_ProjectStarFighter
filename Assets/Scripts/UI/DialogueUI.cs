using System.Collections;
using System.Collections.Generic;
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

        private void Awake() {
            canvasGroup.alpha = 0;
            if (doManually) return;
            this.AddListener(EventType.OnDialoguesChange, param => {
                if (_isPlayingDialogues) {
                    _queue.Add((Dialogues)param);
                }
                else {
                    StartCoroutine(PlayDialogues((Dialogues)param));
                }
            });
        }

        public void PlayDialogue(Dialogues dialogues, bool realTime = false) {
            if (_isPlayingDialogues) {
                _queue.Add(dialogues);
            }
            else {
                StartCoroutine(PlayDialogues(dialogues, realTime));
            }
        }

        private IEnumerator PlayDialogues(Dialogues dialogues, bool realTime = false) {
            _isPlayingDialogues = true;
            canvasGroup.alpha = 1;
            
            handleName.text = dialogues.dialogues.handleName;
            handleName.color = dialogues.dialogues.handleColor;
            
            foreach (var text in dialogues.dialogues.main) {
                messageText.text = text.text;
                if (realTime) {
                    yield return new WaitForSecondsRealtime(text.readingTime);
                }
                else {
                    yield return new WaitForSeconds(text.readingTime);
                }
            }

            if (_queue.Count > 0) {
                StartCoroutine(PlayDialogues(_queue[0], realTime));
                _queue.RemoveAt(0);
                yield break;
            }

            canvasGroup.alpha = 0;
            _isPlayingDialogues = false;
        }
    }
}