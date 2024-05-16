using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UI.Menu {
    public class StartConsoleWriter : MonoBehaviour {
        public List<StartTypeWriter> Writers = new();
        public MainMenu mainMenu;

        private bool _isTransitioning;
        private Sequence _s;

        private void Start() {
            _s = DOTween.Sequence();
            _s.Append(Writers[0].Play("> user.login('subject103', site17ffff110)", 10f, () => {}).SetEase(Ease.Linear));
            _s.Append(Writers[1].Play("> checking credentials...", 8f, () => {}).SetEase(Ease.OutCirc));
            _s.Append(Writers[2].Play("> logging in...", 8f, () => {}).SetEase(Ease.Linear));
            _s.AppendCallback(() => {
                mainMenu.StartMenu();
                _isTransitioning = true;
            });
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && !_isTransitioning) {
                _s?.Kill();
                mainMenu.StartMenu();
                _isTransitioning = true;
            }
        }
    }
}