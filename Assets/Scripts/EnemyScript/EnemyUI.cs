using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyScript {
    public class EnemyUI : MonoBehaviour {
        public Slider healthBar;
        public CanvasGroup canvasGroup;
        [TitleGroup("Tween Settings")] 
        public bool useTween = true;

        [ShowIf(nameof(useTween))] public Ease easeType;
        [ShowIf(nameof(useTween))] public float duration;
        
        private Tween _tween;

        public void SetValue(float value) {
            if (useTween) {
                _tween?.Kill();
                _tween = DOVirtual.Float(healthBar.value, value, duration, v => {
                    healthBar.value = v;
                }).SetEase(easeType);
            }
            else {
                healthBar.value = value;
            }
        }
    }
}
