using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
    public class MainMenu : MonoBehaviour {
        public Canvas mainCanvas;
        public CanvasGroup mainGroup;
        [Space] 
        public CanvasGroup mainOptionsGroup;
        public GraphicRaycaster mainOptionsRaycaster;

        private void Start() {
            mainCanvas.enabled = false;
            mainGroup.alpha = 1;
            
            mainOptionsRaycaster.enabled = false;
        }
        
        public void StartMenu() {
            mainCanvas.enabled = true;
            
            mainOptionsGroup.alpha = 0;
            mainOptionsRaycaster.enabled = false;
            
            var s = DOTween.Sequence();
            s
                .Append(DOVirtual.DelayedCall(0.2f, () => { }))
                .Append(mainOptionsGroup.DOFade(1, 0.5f).OnComplete(() => {
                    mainOptionsRaycaster.enabled = true;
                }));
        }
    }
}