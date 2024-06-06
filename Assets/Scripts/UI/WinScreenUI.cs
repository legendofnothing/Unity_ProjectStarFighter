using System.Collections.Generic;
using DG.Tweening;
using SO;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class WinScreenUI : MonoBehaviour {
        public CanvasGroup backGroundGroup;
        public CanvasGroup mainGroup;
        public GraphicRaycaster raycaster;
        public CanvasGroup randomAssImage;
        public Image ditherImage;
        public DialogueUI dialogueUI;

        public List<Dialogues> DialoguesList = new();
        private Sequence _sequence;

        private void Start() {
            mainGroup.alpha = 0;
            ditherImage.DOFade(0, 0).SetUpdate(true);
            randomAssImage.alpha = 0;
            backGroundGroup.alpha = 0;
            raycaster.enabled = false;
        }

        public void OpenWinScreen() {
            mainGroup.alpha = 0;
            ditherImage.DOFade(0, 0).SetUpdate(true);
            randomAssImage.alpha = 0;
            backGroundGroup.alpha = 0;
            
            _sequence = DOTween.Sequence();
            _sequence
                .Append(backGroundGroup.DOFade(1, 1.5f))
                .Append(ditherImage.DOFade(0.2f, 0.4f).SetEase(Ease.OutQuint))
                .Append(DOVirtual.DelayedCall(0.15f, () => { }))
                .Append(mainGroup.DOFade(1, 0.15f))
                .Append(DOVirtual.DelayedCall(1f, () => {
                    foreach (var d in DialoguesList) {
                        dialogueUI.PlayDialogue(d, true);
                    }

                    DOVirtual.DelayedCall(0.8f, () => {
                        randomAssImage.DOFade(1, 5).SetEase(Ease.Linear).SetUpdate(true);
                    }).SetUpdate(true);

                    raycaster.enabled = true;
                }))
                .SetEase(Ease.Linear)
                .SetUpdate(true);
        }
    }
}
