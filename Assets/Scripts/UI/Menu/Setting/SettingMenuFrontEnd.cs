using System.Collections.Generic;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Setting {
    public class SettingMenuFrontEnd : MonoBehaviour {
        [TitleGroup("Menu Item Ref")] 
        public MainMenu mainMenu;
        public MenuItem settingMenu;
        public List<Image> buttons = new();
        [Space] 
        public List<Canvas> optionCanvases = new();

        [TitleGroup("Setting Ref")] 
        public List<MenuItem> settingItems = new();

        [TitleGroup("Other Ref")]
        public RectTransform backgroundImageTransform;

        [TitleGroup("ButtonRef")]
        public Image fillInner;
        public float transitionDuration;
        private Tween _transitionTween;

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
                    var s = DOTween.Sequence();
                    foreach (var canvas in optionCanvases) {
                        s.Append(DOVirtual.DelayedCall(0.5f, () => {
                            canvas.enabled = true;
                        }));
                    }
                    
                    EnableSetting(0);
                });
        }

        public void SelectSetting(int index) {
            foreach (var button in buttons) {
                button.color = ColorExtension.FromHex(buttons.IndexOf(button) == index ? SelectedColor : UnselectedColor);
            }
            
            EnableSetting(index);
        }

        private void EnableSetting(int index) {
            foreach (var item in settingItems) {
                if (settingItems.IndexOf(item) == index) {
                    item.canvas.enabled = true;
                    item.raycaster.enabled = true;
                }
                else {
                    item.canvas.enabled = false;
                    item.raycaster.enabled = false;
                }
            }
        }

        public void OnButtonHover() {
            _transitionTween?.Kill();
            _transitionTween = fillInner.DOFade(0.2f, transitionDuration);
        }
        
        public void OnButtonUnHover() {
            _transitionTween?.Kill();
            _transitionTween = fillInner.DOFade(0.5f, transitionDuration);
        }

        public void OnButtonPressed() {
            var s = DOTween.Sequence();
            
            foreach (var item in settingItems) {
                item.canvas.enabled = false;
                item.raycaster.enabled = false;
            }
            
            foreach (var canvas in optionCanvases) {
                if (optionCanvases.IndexOf(canvas) == 0) {
                    canvas.enabled = false;
                    continue;
                } 
                
                s.Append(DOVirtual.DelayedCall(0.8f, () => {
                    canvas.enabled = false;
                }));
            }
            s.Append(DOVirtual.DelayedCall(0.5f, (() => { })));
            s.Append(DOVirtual.Vector3(backgroundImageTransform.sizeDelta, Vector2.zero, 1f, value => {
                backgroundImageTransform.sizeDelta = value;
            })).OnComplete(() => {
                settingMenu.canvas.enabled = false;
                mainMenu.OpenMenu();
            });
        }
    }
}