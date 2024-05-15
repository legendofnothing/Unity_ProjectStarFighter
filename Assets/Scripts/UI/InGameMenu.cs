using System.Collections;
using Core.Events;
using DG.Tweening;
using Sirenix.OdinInspector;
using SO;
using TMPro;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace UI {
    public class InGameMenu : MonoBehaviour {
        public Canvas mainCanvas;
        public Canvas pauseCanvas;
        public Canvas deathCanvas;

        [TitleGroup("Death")] 
        public DeathScreenUI deathUI;

        [TitleGroup("Miscs")] 
        public TextMeshProUGUI tipsText;
        public BunchOfTip tips;

        private bool _isPausing;
        private bool _paused;
        private bool _hasDied;
        
        
        private void Start() {
            pauseCanvas.enabled = false;
            deathCanvas.enabled = false;
            this.AddListener(EventType.OpenDeathUI, _ => OpenDeathUI());
        }

        private void OpenDeathUI() {
            Time.timeScale = 0;
            deathCanvas.enabled = true;
            deathUI.OpenDeathScreen();
            _hasDied = true;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && !_hasDied) {
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
