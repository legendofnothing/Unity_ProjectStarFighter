using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
    public class MainMenu : MonoBehaviour {
        public Canvas startingCanvas;
        public CanvasGroup startingGroup;
        [Space]
        public Canvas mainCanvas;
        public CanvasGroup mainGroup;
        [Space] 
        public CanvasGroup mainOptionsGroup;
        public GraphicRaycaster mainOptionsRaycaster;

        private void Start() {
            startingCanvas.enabled = true;
            startingGroup.alpha = 1;
            
            mainCanvas.enabled = false;
            mainGroup.alpha = 0;
            
            mainOptionsRaycaster.enabled = false;
        }
        
        public void StartMenu() {
            mainCanvas.enabled = true;
            mainGroup.alpha = 0;
            mainOptionsGroup.alpha = 0;
            mainOptionsRaycaster.enabled = false;

            var time = 1.8f;
            
            var s = DOTween.Sequence();
            s
                .Append(startingGroup.DOFade(0, time).OnComplete(() => startingCanvas.enabled = false))
                .Insert(0, mainGroup.DOFade(1, time))
                .Append(DOVirtual.DelayedCall(0.8f, () => { }))
                .Append(mainOptionsGroup.DOFade(1, 0.5f).OnComplete(() => {
                    mainOptionsRaycaster.enabled = true;
                }));
        }
    }
}