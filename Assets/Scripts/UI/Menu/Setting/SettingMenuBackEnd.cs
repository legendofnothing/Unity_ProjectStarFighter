using System.Collections.Generic;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Setting {
    public class SettingMenuBackEnd : MonoBehaviour {
        private class Settings {
            public string CurrentMode = "FullScreen";
            public string CurrentResolution = "1920x1080";
            public float CurrentMusicVolume = 1;
            public float CurrentSfxVolume = 1;
        }
        
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown windowTypeDropdown;
        [Space] 
        public Slider volumeMusicSlider;
        public Slider volumeSfxSlider;
        [Space] 
        public TMP_InputField musicInputField;
        public TMP_InputField sfxInputField;

        private FullScreenMode _currentMode;

        private Settings _settings;
        private const string SettingKey = "Setting";

        private readonly List<string> _resolutions = new() {
            "1920x1080",
            "1600x900",
            "1366x768",
            "1280x720",
            "1280x960",
            "1024x768",
            "800x600",
        };
        
        private readonly List<string> _windowModes = new() {
            "FullScreen",
            "Windowed",
            "Borderless"
        };

        private void Start() {
#if UNITY_EDITOR
            SaveSystem.ClearAll();
#endif
            
            resolutionDropdown.options.Clear();
            windowTypeDropdown.options.Clear();
            
            resolutionDropdown.AddOptions(_resolutions);
            windowTypeDropdown.AddOptions(_windowModes);
            
            _settings = SaveSystem.GetData<Settings>(SettingKey);
            if (_settings == null) return;
            
            resolutionDropdown.value = _resolutions.IndexOf(_settings.CurrentResolution);
            windowTypeDropdown.value = _windowModes.IndexOf(_settings.CurrentMode);

            volumeSfxSlider.value = _settings.CurrentMusicVolume;
            volumeMusicSlider.value = _settings.CurrentSfxVolume;

            musicInputField.text = (_settings.CurrentMusicVolume * 100).ToString("0");
            sfxInputField.text = (_settings.CurrentSfxVolume * 100).ToString("0");

            _currentMode = _settings.CurrentMode switch {
                "FullScreen" => FullScreenMode.ExclusiveFullScreen,
                "Windowed" => FullScreenMode.Windowed,
                "Borderless" => FullScreenMode.FullScreenWindow,
                _ => FullScreenMode.ExclusiveFullScreen
            };
            
            OnVolumeChange();
            
            var sizes = _settings.CurrentResolution.Split("x");
            Screen.SetResolution(int.Parse(sizes[0]), int.Parse(sizes[1]), _currentMode);
            Debug.Log($"Resolution Changed to {sizes[0]}x{sizes[1]}");
        }

        public void OnResolutionChanged() {
            var sizes = _resolutions[resolutionDropdown.value].Split("x");
            _settings.CurrentResolution = _resolutions[resolutionDropdown.value];
            Screen.SetResolution(int.Parse(sizes[0]), int.Parse(sizes[1]), _currentMode);
            Debug.Log($"Resolution Changed to {sizes[0]}x{sizes[1]}");
            
            SaveSystem.SaveData(_settings, SettingKey);
        }
        
        public void OnWindowModeChanged() {
            var current = _windowModes[windowTypeDropdown.value];
            _settings.CurrentMode = current;
            switch (current) {
                case "FullScreen":
                    _currentMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case "Windowed":
                    _currentMode = FullScreenMode.Windowed;
                    break;
                case "Borderless":
                    _currentMode = FullScreenMode.FullScreenWindow;
                    break;
            }

            Screen.fullScreenMode = _currentMode;
            Debug.Log($"Mode Changed to {current}");
            SaveSystem.SaveData(_settings, SettingKey);
        }

        public void OnMusicVolumeChange() {
            _settings.CurrentMusicVolume = volumeMusicSlider.value;
            musicInputField.text = (_settings.CurrentMusicVolume * 100).ToString("0");
            OnVolumeChange();
        }
        
        public void OnSFXVolumeChange() {
            _settings.CurrentSfxVolume = volumeSfxSlider.value;
            sfxInputField.text = (_settings.CurrentSfxVolume * 100).ToString("0");
            OnVolumeChange();
        }

        public void OnMusicVolumeInput() {
            if (!int.TryParse(musicInputField.text, out var value)) {
                musicInputField.text = (_settings.CurrentMusicVolume * 100).ToString("0");
                return;
            }
            
            if (value is < 0 or > 100) {
                musicInputField.text = (_settings.CurrentMusicVolume * 100).ToString("0");
                return;
            }

            _settings.CurrentMusicVolume = value / 100f;
            volumeMusicSlider.value = _settings.CurrentMusicVolume;
            musicInputField.text = (_settings.CurrentMusicVolume * 100).ToString("0");
            OnVolumeChange();
        }
        
        public void OnSFXVolumeInput() {
            if (!int.TryParse(sfxInputField.text, out var value)) {
                sfxInputField.text = (_settings.CurrentSfxVolume * 100).ToString("0");
                return;
            };
            
            if (value is < 0 or > 100) {
                sfxInputField.text = (_settings.CurrentSfxVolume * 100).ToString("0");
                return;
            }

            _settings.CurrentSfxVolume = value / 100f;
            volumeSfxSlider.value = _settings.CurrentSfxVolume;
            sfxInputField.text = (_settings.CurrentSfxVolume * 100).ToString("0");
            OnVolumeChange();
        }

        private void OnVolumeChange() {
            AudioManager.Instance.SetMusicVolume(_settings.CurrentMusicVolume);
            AudioManager.Instance.SetSFXVolume(_settings.CurrentSfxVolume);
            SaveSystem.SaveData(_settings, SettingKey);
        }
    }
}
    