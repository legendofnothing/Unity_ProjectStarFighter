using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Components {
    public class ButtonHoverText : MonoBehaviour {
        public TextMeshProUGUI text;
        public Color onHoverColor;
        [Space] 
        public Ease easeType;
        public float duration;

        private Color _defaultColor;
        private Tween _tween;

        private void Start() {
            _defaultColor = text.color;
        }

        public void OnHover() {
            _tween?.Kill();
            _tween = text.DOColor(onHoverColor, duration).SetEase(easeType).SetUpdate(true);
        }

        public void OnUnHover() {
            _tween?.Kill();
            _tween = text.DOColor(_defaultColor, duration).SetEase(easeType).SetUpdate(true);
        }
    }
}