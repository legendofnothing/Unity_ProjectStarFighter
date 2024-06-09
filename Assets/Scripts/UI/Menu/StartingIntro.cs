using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
    public class StartingIntro : MonoBehaviour {
        [TitleGroup("Intro Refs")]
        public Dialogues dialogue;
        public DialogueUI uiDialogue;
        public TextMeshProUGUI skipText;
        public DOTweenAnimation tween;
        public List<Image> images;

        [TitleGroup("Other Refs")] 
        public MainMenu mainMenu;
        
        private Tween _delayTween;
        private bool _isStarting;
        
        private void Start() {
            GetComponent<Canvas>().enabled = true;
            
            uiDialogue.PlayDialogue(dialogue, true);
            Debug.Log("Played Dialogues");
            
            var delay = dialogue.dialogues.main.Sum(t => t.readingTime);
            _delayTween = DOVirtual.DelayedCall(delay, () => {
                _isStarting = true;
                skipText.DOFade(0, 0.2f);
                foreach (var image in images) {
                    DOVirtual.Float(image.fillAmount, 0, 1.5f, value => {
                        image.fillAmount = value;
                    }).SetEase(Ease.InElastic).OnComplete(() => {
                        this.GetComponent<Canvas>().enabled = false;
                        mainMenu.OpenMenu();
                    });
                }
            });
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && !_isStarting) {
                _delayTween?.Kill();
                uiDialogue.StopDialogue();
                _isStarting = true;

                tween.DOKill();
                skipText.DOFade(0, 0.2f);
                
                foreach (var image in images) {
                    DOVirtual.Float(image.fillAmount, 0, 1.5f, value => {
                        image.fillAmount = value;
                    }).SetEase(Ease.InElastic).OnComplete(() => {
                        GetComponent<Canvas>().enabled = false;
                        mainMenu.OpenMenu();
                    });
                }
            }
        }
    }
}