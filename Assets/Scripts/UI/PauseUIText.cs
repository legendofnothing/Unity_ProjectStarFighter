using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI {
    public class PauseUIText : MonoBehaviour {
        private Sequence _delayTween;
        private int _index;
        private string _text = "> unable to resolve connection, 503\n> retrying";
        private TextMeshProUGUI _textMesh;
        
        private void Start() {
            _textMesh = GetComponent<TextMeshProUGUI>();
            _delayTween = DOTween.Sequence();
            _delayTween
                .Append(DOVirtual.DelayedCall(0.8f, AddText))
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear)
                .SetUpdate(true);
        }

        private void AddText() {
            _index++;
            if (_index >= 4) _index = 0;
            var textToAdd = "";
            for (var i = 0; i < _index; i++) {
                textToAdd += ".";
            }

            _textMesh.text = _text + textToAdd;
        }
    }
}
