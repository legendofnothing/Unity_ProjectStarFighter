using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components {
    public class ButtonHoverUIImage : MonoBehaviour {
        public Image image;
        public Color onHoverColor;
        public Color disabledColor;
        [Space] 
        public Ease easeType= Ease.Linear;
        public float duration = 0.2f;

        private bool _isDisabled;
        public bool isDisabled {

            get => _isDisabled;
            set {
                if (_isDisabled == value) return;
                if (value) {
                    _tween?.Kill();
                    _tween = image.DOColor(disabledColor, duration).SetEase(easeType).SetUpdate(true);
                }
                else {
                    _tween?.Kill();
                    _tween = image.DOColor(_defaultColor, duration).SetEase(easeType).SetUpdate(true);
                }

                _isDisabled = value;
            }
        }

        private Color _defaultColor;
        private Tween _tween;
        private bool _canDisable;

        private void Start() {
            _defaultColor = image.color;
            image.color = _defaultColor;
            _canDisable = true;
        }

        public void OnHover() {
            if (isDisabled) return;
            _tween?.Kill();
            _tween = image.DOColor(onHoverColor, duration).SetEase(easeType).SetUpdate(true);
        }

        public void OnUnHover() {
            if (isDisabled) return;
            _tween?.Kill();
            _tween = image.DOColor(_defaultColor, duration).SetEase(easeType).SetUpdate(true);
        }
    }
}