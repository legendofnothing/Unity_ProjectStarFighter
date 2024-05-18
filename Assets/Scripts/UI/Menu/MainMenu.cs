using System.Linq;
using Audio;
using Core.Events;
using DG.Tweening;
using SO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventType = Core.Events.EventType;

namespace UI.Menu {
    public class MainMenu : MonoBehaviour {
        public Dialogues outroDialogues;
        public StartingIntro intro;
        public AudioClip menuMusic;
        public AudioClip transitionAudio;
        public AudioClip outOfIntroAudio;
        [Space]
        public Canvas mainCanvas;
        public CanvasGroup mainGroup;
        [Space] 
        public Canvas mainOptionsCanvas;
        public CanvasGroup mainOptionsGroup;
        public GraphicRaycaster mainOptionsRaycaster;
        [Space] 
        public CanvasGroup levelSelectorGroup;
        public Canvas levelSelectorCanvas;
        public GraphicRaycaster levelSelectorRaycaster;

        private void Start() {
            Time.timeScale = 1;
            mainCanvas.enabled = false;
            mainGroup.alpha = 1;
            
            mainOptionsCanvas.enabled = false;
            mainOptionsRaycaster.enabled = false;

            levelSelectorCanvas.enabled = false;
            levelSelectorGroup.alpha = 0;
            levelSelectorRaycaster.enabled = false;
        }
        
        public void StartMenu() {
            mainCanvas.enabled = true;
            
            mainOptionsGroup.alpha = 0;
            mainOptionsRaycaster.enabled = false;
            mainOptionsCanvas.enabled = true;
            AudioManager.Instance.PlaySFX(outOfIntroAudio);
            AudioManager.Instance.PlayMusic(menuMusic, 2f);
            
            var s = DOTween.Sequence();
            s
                .Append(DOVirtual.DelayedCall(0.2f, () => { }))
                .Append(mainOptionsGroup.DOFade(1, 0.5f).OnComplete(() => {
                    mainOptionsRaycaster.enabled = true;
                }));
        }

        public void GoToLevelSelector() {
            mainOptionsRaycaster.enabled = false;
            
            levelSelectorCanvas.enabled = true;
            levelSelectorGroup.alpha = 0;
            levelSelectorRaycaster.enabled = false;
            
            AudioManager.Instance.PlaySFX(transitionAudio);
            
            var s = DOTween.Sequence();
            s
                .Append(mainOptionsGroup.DOFade(0, 0.5f).OnComplete(() => {
                    mainOptionsCanvas.enabled = false;
                    
                    levelSelectorRaycaster.enabled = true;
                }))
                .Insert(0, levelSelectorGroup.DOFade(1, 0.5f));
        }

        public void ReturnToMenuFromLevelSelector() {
            levelSelectorRaycaster.enabled = false;

            mainOptionsCanvas.enabled = true;
            mainOptionsGroup.alpha = 0;
            mainOptionsRaycaster.enabled = false;
            
            AudioManager.Instance.PlaySFX(transitionAudio);
            
            var s = DOTween.Sequence();
            s
                .Append(levelSelectorGroup.DOFade(0, 0.5f).OnComplete(() => {
                    mainOptionsRaycaster.enabled = true;

                    levelSelectorCanvas.enabled = false;
                }))
                .Insert(0, mainOptionsGroup.DOFade(1, 0.5f));
        }

        public void Quit() {
            intro.GetComponent<Canvas>().enabled = true;
            mainOptionsRaycaster.enabled = false;

            mainOptionsGroup.DOFade(0, 0.8f).SetEase(Ease.InOutExpo);
            
            foreach (var image in intro.images) {
                DOVirtual.Float(image.fillAmount, 0.5f, 1.5f, value => {
                    image.fillAmount = value;
                }).SetEase(Ease.InElastic).OnComplete(Application.Quit);
            }
        }

        public void Play(int level) {
            levelSelectorRaycaster.enabled = false;
            mainOptionsRaycaster.enabled = false;
            
            intro.GetComponent<Canvas>().enabled = true;
            AudioManager.Instance.StopMusic(true, 2f);
            
            var s = DOTween.Sequence();
            s
                .Append(levelSelectorGroup.DOFade(0, 0.5f).OnComplete(() => {
                    levelSelectorCanvas.enabled = false;
                }))
                .OnComplete(() => {
                    intro.uiDialogue.ChangeTextColor(new Color(0.4f, 0.4f, 0.4f));
                    intro.uiDialogue.PlayDialogue(outroDialogues);
                    var delay = outroDialogues.dialogues.main.Sum(t => t.readingTime);

                    DOVirtual.DelayedCall(delay * 0.8f, () => {
                        foreach (var image in intro.images) {
                            DOVirtual.Float(image.fillAmount, 0.5f, 1.5f, value => {
                                image.fillAmount = value;
                            }).SetEase(Ease.InElastic).OnComplete(() => {
                                
                                var sceneName = level switch {
                                    1 => "LevelOne",
                                    2 => "LevelTwo",
                                    3 => "LevelThree",
                                    _ => "LevelOne"
                                };

                                SceneManager.LoadScene(sceneName);
                            });
                        }
                    });
                });
        }
    }
}