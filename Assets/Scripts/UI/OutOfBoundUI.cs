using Core.Events;
using DG.Tweening;
using Level;
using SO;
using TMPro;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace UI {
    public class OutOfBoundUI : MonoBehaviour {
        public TextMeshProUGUI timerText;
        public Canvas outOfBoundCanvas;

        private Tween _timerTween;
        private float _time => LevelManager.Instance.levelSetting.outOfBoundDuration;
    
        
        private void Awake() {
            this.AddListener(EventType.OnPlayerOutOfBound, param => HandleUI((bool)param));
        }

        private void Start() {
            outOfBoundCanvas.enabled = false;
        }

        private void HandleUI(bool isOutOfBound) {
            if (LevelManager.Instance.CurrentState != GameState.Playing) return;
            if (isOutOfBound) {
                outOfBoundCanvas.enabled = true;
                _timerTween = DOVirtual.Float(_time, 0, _time, value => {
                    timerText.text = value.ToString("0.00") + "s";
                }).SetEase(Ease.Linear).OnComplete(() => {
                    outOfBoundCanvas.enabled = false;
                    this.FireEvent(EventType.OnGameStateChange, GameState.GameOver);
                });
            }
            else {
                outOfBoundCanvas.enabled = false;
                _timerTween?.Kill();
            }
        }
    }
}