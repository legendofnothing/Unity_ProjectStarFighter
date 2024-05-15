using System.Collections;
using Sirenix.OdinInspector;
using SO;
using TMPro;
using UnityEngine;

namespace UI {
    public class InGameMenu : MonoBehaviour {
        public Canvas mainCanvas;
        public Canvas pauseCanvas;

        [TitleGroup("Miscs")] 
        public TextMeshProUGUI tipsText;
        public BunchOfTip tips;

        private bool _isPausing;
        private bool _paused;
        private void Start() {
            pauseCanvas.enabled = false;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (_paused) UnPause();
                else PauseGame();
            }
        }

        public void PauseGame() {
            if (_isPausing) return;
            _isPausing = true;
            Time.timeScale = 0;
            _paused = !_paused;
            pauseCanvas.enabled = true;
            mainCanvas.enabled = false;
            _isPausing = false;

            var element = tips.tips[Random.Range(0, tips.tips.Count)];
            tipsText.text = "Tips: " + element.text;
        }

        public void UnPause() {
            if (_isPausing) return;
            _isPausing = true;
            Time.timeScale = 1;
            _paused = !_paused;
            pauseCanvas.enabled = false;
            mainCanvas.enabled = true;
            _isPausing = false;
        }
    }
}
