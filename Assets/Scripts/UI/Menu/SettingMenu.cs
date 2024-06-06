using System.Collections.Generic;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
    public class SettingMenu : MonoBehaviour {
        [TitleGroup("Menu Item Ref")]
        public MenuItem settingMenu;
        public List<Image> buttons = new();
        [Space] 
        public List<Canvas> optionCanvases = new();

        [TitleGroup("Other Ref")]
        public RectTransform backgroundImageTransform;

        private const string SelectedColor = "7D7D7D"; 
        private const string UnselectedColor = "9C9C9C";

        private void Start() {
            settingMenu.DisableAllItems();
            backgroundImageTransform.sizeDelta = Vector2.zero;
            foreach (var button in buttons) {
                button.color = ColorExtension.FromHex(buttons.IndexOf(button) == 0 ? SelectedColor : UnselectedColor);
            }

            foreach (var canvas in optionCanvases) {
                canvas.enabled = false;
            }
        }

        public void OpenSetting() {
            settingMenu.canvas.enabled = true;
            settingMenu.canvasGroup.alpha = 1;
            var rectTransform = (RectTransform) settingMenu.canvas.transform;
            var s = DOTween.Sequence();
            s
                .Append(DOVirtual.Vector3(Vector2.zero, rectTransform.sizeDelta, 1f, value => {
                    backgroundImageTransform.sizeDelta = value;
                }))
                .OnComplete(() => {
                    settingMenu.raycaster.enabled = true;
                    foreach (var canvas in optionCanvases) {
                        DOVirtual.DelayedCall(0.5f, () => {
                            canvas.enabled = true;
                        });
                    }
                });
        }

        public void SelectSetting(int index) {
            foreach (var button in buttons) {
                button.color = ColorExtension.FromHex(buttons.IndexOf(button) == index ? SelectedColor : UnselectedColor);
            }
        }
        
        //7D7D7D
    }
}