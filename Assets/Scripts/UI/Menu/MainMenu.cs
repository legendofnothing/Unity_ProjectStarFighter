using DG.Tweening;
using UnityEngine;

namespace UI.Menu {
    public class MainMenu : MonoBehaviour {
        public Canvas startingCanvas;
        public CanvasGroup startingGroup;
        [Space]
        public Canvas mainCanvas;
        public CanvasGroup mainGroup;

        private void Start() {
            startingCanvas.enabled = true;
            startingGroup.alpha = 1;
            
            mainCanvas.enabled = false;
            mainGroup.alpha = 0;
        }
        
        public void StartMenu() {
            mainCanvas.enabled = true;
            mainGroup.alpha = 0;

            var time = 1.8f;
            
            var s = DOTween.Sequence();
            s
                .Append(startingGroup.DOFade(0, time).OnComplete(() => startingCanvas.enabled = false))
                .Insert(0, mainGroup.DOFade(1, time));
        }
    }
}