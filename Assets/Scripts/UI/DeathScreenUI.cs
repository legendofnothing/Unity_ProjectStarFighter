using System.Collections.Generic;
using Audio;
using DG.Tweening;
using SO;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DeathScreenUI : MonoBehaviour {
        public CanvasGroup mainGroup;
        public CanvasGroup reiGroup;
        public Image ditherImage;
        public DialogueUI dialogueUI;

        public List<Dialogues> DialoguesList = new();

        private Sequence _sequence;

        private void Start() {
            //OpenDeathScreen();
        }

        public void OpenDeathScreen() {
            mainGroup.alpha = 0;
            ditherImage.DOFade(0, 0).SetUpdate(true);
            reiGroup.alpha = 0;
            AudioManager.Instance.PauseAllSound();
            
            _sequence = DOTween.Sequence();
            _sequence
                .Append(DOVirtual.DelayedCall(2f, () => { }))
                .Append(ditherImage.DOFade(0.2f, 0.4f).SetEase(Ease.OutQuint))
                .Append(DOVirtual.DelayedCall(0.15f, () => { }))
                .Append(mainGroup.DOFade(1, 0.15f))
                .Append(DOVirtual.DelayedCall(1f, () => {
                    foreach (var d in DialoguesList) {
                        dialogueUI.PlayDialogue(d, true);
                    }

                    DOVirtual.DelayedCall(0.8f, () => {
                        reiGroup.DOFade(1, 5).SetEase(Ease.Linear).SetUpdate(true);
                    }).SetUpdate(true);
                }))
                .SetEase(Ease.Linear)
                .SetUpdate(true);
        }
    }
}