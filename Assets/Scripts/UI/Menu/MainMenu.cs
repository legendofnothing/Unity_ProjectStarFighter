using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
    public class MainMenu : MonoBehaviour {
        public StartingIntro intro;
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
    }
}