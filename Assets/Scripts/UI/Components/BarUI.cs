using Core.Events;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using EventType = Core.Events.EventType;

namespace UI.Components {
    public class BarUI : MonoBehaviour {
        public Slider progressBar;
        public Image fillImage;

        [Space] 
        public EventType eventType;
        public Color fillColor;

        [TitleGroup("Tween Settings")] 
        public bool useTween = true;

        [ShowIf(nameof(useTween))] public Ease easeType;
        [ShowIf(nameof(useTween))] public float duration;

        private Tween _tween;
        private bool _started;

        private void Awake() {
            if (fillImage) fillImage.color = fillColor;
            progressBar.value = 0;
            OnEvent();
            _started = true;
        }

        protected virtual void OnEvent() {
            this.AddListener(eventType, response => {
                switch (response) {
                    case float value:
                        if (useTween) {
                            _tween?.Kill();
                            _tween = DOVirtual.Float(progressBar.value, value, duration, v => {
                                progressBar.value = v;
                            }).SetEase(easeType);
                        }
                        else {
                            progressBar.value = value;
                        }
                        break;
                    default:
                        Debug.LogError($"Invalid callback at {gameObject.name}, callback response is not a value!");
                        break;
                }
            });
        }

        private void OnDrawGizmos() {
            if (!fillImage || _started) return;
            fillImage.color = fillColor;
        }
    }
}