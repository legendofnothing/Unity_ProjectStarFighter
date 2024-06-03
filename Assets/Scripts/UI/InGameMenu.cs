using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Core.Events;
using DG.Tweening;
using PlayerScript;
using Sirenix.OdinInspector;
using SO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventType = Core.Events.EventType;

namespace UI {
    public class InGameMenu : MonoBehaviour {
        public Cursors Cursors;
        [Space]
        public Canvas introCanvas;
        public Canvas mainCanvas;
        public Canvas pauseCanvas;
        public Canvas deathCanvas;
        public Canvas winCanvas;

        [TitleGroup("Raycasters")] 
        public GraphicRaycaster winRaycaster;
        public GraphicRaycaster pauseRaycaster;
        public GraphicRaycaster deathRaycaster;

        [TitleGroup("UI Refs")] 
        public DeathScreenUI deathUI;
        public WinScreenUI winScreen;

        [TitleGroup("Intro")] 
        public List<Image> images = new();


        private bool _isPausing;
        private bool _paused;
        private bool _hasGameEnded;
        
        
        private void Start() {
            pauseCanvas.enabled = false;
            deathCanvas.enabled = false;
            winCanvas.enabled = false;
            introCanvas.enabled = true;

            Time.timeScale = 0;
            Player.Instance.ManipulateInput(false);
            foreach (var image in images) {
                image.fillAmount = 0.5f;
            }
            
            DoBlink182(0, 2.5f, () => {
                Time.timeScale = 1;
                introCanvas.enabled = false;
                Player.Instance.ManipulateInput();
            });
            
            this.AddListener(EventType.OpenDeathUI, _ => OpenDeathUI());
            this.AddListener(EventType.OpenWinUI, _ => OpenWinUI());
            this.AddListener(EventType.OpenActualWinUI, _ => OpenActualWinUI());
        }

        private void OpenDeathUI() {
            Time.timeScale = 0;
            deathCanvas.enabled = true;
            deathUI.OpenDeathScreen();
            _hasGameEnded = true;
        }

        private void OpenWinUI() {
            winCanvas.enabled = true;
            _hasGameEnded = true;
        }

        private void OpenActualWinUI() {
            winScreen.OpenWinScreen();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && !_hasGameEnded) {
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
            AudioManager.Instance.SetInsideVolume(0);
            AudioManager.Instance.SetOutsideVolume(0);
        }

        public void UnPause() {
            if (_isPausing) return;
            _isPausing = true;
            Time.timeScale = 1;
            _paused = !_paused;
            pauseCanvas.enabled = false;
            mainCanvas.enabled = true;
            _isPausing = false;
            AudioManager.Instance.SetInsideVolume(1);
            AudioManager.Instance.SetOutsideVolume(1);
        }

        public void Retry() {
            Time.timeScale = 0;
            Player.Instance.ManipulateInput(false);
            introCanvas.enabled = true;
            pauseRaycaster.enabled = false;
            deathRaycaster.enabled = false;
            winRaycaster.enabled = false;

            DoBlink182(0.5f, 1.5f, () => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        public void Quit() {
            Time.timeScale = 0;
            Player.Instance.ManipulateInput(false);
            introCanvas.enabled = true;
            pauseRaycaster.enabled = false;
            deathRaycaster.enabled = false;
            winRaycaster.enabled = false;

            DoBlink182(0.5f, 1.5f, () => {
                SceneManager.LoadScene("Menu");
            });
        }

        private void DoBlink182(float to, float duration, TweenCallback action) {
            foreach (var image in images) {
                DOVirtual.Float(image.fillAmount, to, duration, value => {
                    image.fillAmount = value;
                }).SetEase(Ease.InElastic).SetUpdate(true).OnComplete(action);
            }
        }
    }
}
