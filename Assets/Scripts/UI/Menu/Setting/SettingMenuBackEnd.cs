using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Setting {
    public class SettingMenuBackEnd : MonoBehaviour {
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown windowTypeDropdown;
        [Space] 
        public Slider volumeSfxSlider;
        public Slider volumeMusicSlider;

        private FullScreenMode _currentMode;
        private string _currentResolution;

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
            resolutionDropdown.options.Clear();
            windowTypeDropdown.options.Clear();
            
            resolutionDropdown.AddOptions(_resolutions);
            windowTypeDropdown.AddOptions(_windowModes);

            resolutionDropdown.value = 0;
            
            _currentMode = FullScreenMode.ExclusiveFullScreen;
            _currentResolution = _resolutions[0];
        }

        public void OnResolutionChanged() {
            var sizes = _resolutions[resolutionDropdown.value].Split("x");
            Screen.SetResolution(int.Parse(sizes[0]), int.Parse(sizes[1]), _currentMode);
            Debug.Log($"Resolution Changed to {sizes[0]}x{sizes[1]}");
        }
        
        public void OnWindowModeChanged() {
            var current = _windowModes[windowTypeDropdown.value];
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
        }
    }
}
    