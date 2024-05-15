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

        private bool _isPlayingDialogues;
        private List<Dialogues> _queue = new();

        private void Awake() {
            canvasGroup.alpha = 0;
            this.AddListener(EventType.OnDialoguesChange, param => {
                if (_isPlayingDialogues) {
                    _queue.Add((Dialogues)param);
                }
                else {
                    StartCoroutine(PlayDialogues((Dialogues)param));
                }
            });
        }

        private IEnumerator PlayDialogues(Dialogues dialogues) {
            _isPlayingDialogues = true;
            canvasGroup.alpha = 1;
            
            handleName.text = dialogues.dialogues.handleName;
            handleName.color = dialogues.dialogues.handleColor;
            
            foreach (var text in dialogues.dialogues.main) {
                messageText.text = text.text;
                yield return new WaitForSeconds(text.readingTime);
            }

            if (_queue.Count > 0) {
                StartCoroutine(PlayDialogues(_queue[0]));
                _queue.RemoveAt(0);
                yield break;
            }

            canvasGroup.alpha = 0;
            _isPlayingDialogues = false;
        }
    }
}