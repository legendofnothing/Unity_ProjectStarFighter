﻿using System;
using System.Collections.Generic;
using Audio;
using DG.Tweening;
using Sirenix.OdinInspector;
using UI.Menu.MissionSelector;
using UI.Menu.Setting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace UI.Menu {
    [Serializable]
    public class MenuItem {
        public CanvasGroup canvasGroup;
        public Canvas canvas;
        public GraphicRaycaster raycaster;

        private Sequence _sequence;

        public void DisableAllItems() {
            if (canvasGroup) canvasGroup.alpha = 0;
            canvas.enabled = false;
            if (raycaster) raycaster.enabled = false;
        }

        public void OpenMenu(float duration, Action callback = null, Ease easeType = Ease.Linear) {
            _sequence?.Kill();
            canvas.enabled = true;
            if (raycaster) raycaster.enabled = false;
            if (canvasGroup) {
                canvasGroup.alpha = 0;
                _sequence = DOTween.Sequence();
                _sequence
                    .Append(canvasGroup.DOFade(1, duration).SetEase(easeType))
                    .OnComplete(() => {
                        raycaster.enabled = true;
                        callback?.Invoke();
                    });
            }
            else {
                if (raycaster) raycaster.enabled = true;
                callback?.Invoke();
            }
        }

        public void CloseMenu(float duration, Action callback = null, Ease easeType = Ease.Linear) {
            _sequence?.Kill();
            if (raycaster) raycaster.enabled = false;
            if (canvasGroup) {
                canvasGroup.alpha = 1;
                _sequence = DOTween.Sequence();
                _sequence
                    .Append(canvasGroup.DOFade(0, duration).SetEase(easeType))
                    .OnComplete(() => {
                        canvas.enabled = false;
                        callback?.Invoke();
                    });
            }
            else {
                canvas.enabled = false;
                callback?.Invoke();
            }
        }
    }
    
    public class MainMenu : MonoBehaviour {
        [TitleGroup("Blink Effect")] 
        public List<Image> blinks = new();

        [TitleGroup("Menu Refs")] 
        public SettingMenuFrontEnd settingMenuFrontEnd;
        public MissionSelectorFrontEnd missionSelectorMenu;
        [Space]
        public MenuItem homeMenu;

        [Space] 
        public AudioClip startMenuAudio;
        public AudioClip selectedAudio;
        public AudioClip mainMenuMusic;

        private void Start() {
            homeMenu.DisableAllItems();
            foreach (var blink in blinks) {
                blink.fillAmount = 0f;
            }
        }

        public void OpenMenu() {
            homeMenu.OpenMenu(1f);
            AudioManager.Instance.PlaySFXOneShot(startMenuAudio);
            DOVirtual.DelayedCall(0.5f, () => {
                if (!AudioManager.Instance.IsMusicPlaying) AudioManager.Instance.PlayMusic(mainMenuMusic);
            });
        }

        public void OpenLevelSelector() {
            AudioManager.Instance.PlaySFXOneShot(selectedAudio);
            homeMenu.CloseMenu(0.8f, () => {
                missionSelectorMenu.Open();
            });
        }

        public void OpenSetting() {
            AudioManager.Instance.PlaySFXOneShot(selectedAudio);
            homeMenu.CloseMenu(0.5f, () => {
                settingMenuFrontEnd.OpenSetting();
            });
        }

        public void Quit() {
            homeMenu.CloseMenu(0.8f, () => {
                foreach (var blink in blinks) {
                    DOVirtual.Float(blink.fillAmount, 0.5f, 1.5f, value => {
                        blink.fillAmount = value;
                    }).SetEase(Ease.InElastic).OnComplete(Application.Quit);
                }
            });
        }
    }
}