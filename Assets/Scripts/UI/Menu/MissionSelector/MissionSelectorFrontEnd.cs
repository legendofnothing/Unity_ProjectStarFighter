using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace UI.Menu.MissionSelector {
    public class MissionSelectorFrontEnd : MonoBehaviour {
        public enum People {
            Overseer,
            One,
            Two,
        }

        [Serializable]
        public struct PeopleCard {
            public string name;
            public Sprite image;
        }
        
        public MenuItem mainItem;
        public MainMenu mainMenu;
        public MissionSelectorLoading missionLoading;
        public PostProcessVolume volume;

        [TitleGroup("Image Refs")] 
        public Image backdrop;
        public Image ditherScreen;

        [Title("Rect Transforms")]
        public RectTransform mainRect;

        [TitleGroup("Handler Setting")] 
        public RectTransform handlerTransform;
        public Image handlerImage;
        public TypeWriter handlerText;
        public List<Sprite> handlerSprites = new();

        private Dictionary<People, PeopleCard> _peopleCards = new();

        [Title("Mission Selector Buttons")] 
        public List<ButtonHoverUIImage> buttonSelectors = new();
        public List<TypeWriter> typewriters = new();

        [Title("Mission Selector Main")]
        public CanvasGroup selectorGroup;
        public AnimationCurve selectorCurve;
        [Space]
        public TextMeshProUGUI missionBriefText;
        public TextMeshProUGUI missionTitleText;
        public TextMeshProUGUI missionDiffText;
        [Space]
        public List<TextAsset> missionBriefs = new();
        public List<string> missionNames = new();

        [Title("Duration")] 
        public float openingScreenDuration = 1f;

        [Title("Start Mission Button")] 
        public ButtonHoverUIImage startButton;
        public CanvasGroup loadingGroup;

        private Vector2 _originalMainSize;
        private Sequence _sequence;
        private Sequence _switchMissionSequence;
        
        public int currentLevel { get; private set; }

        private void Start() {
            if (volume.profile.TryGetSettings(out LensDistortion t)) {
                t.intensity.value = 0;
            }
            
            _peopleCards.Add(People.Overseer, new PeopleCard {
                name = "Overseer",
                image = handlerSprites[0]
            });
            _peopleCards.Add(People.One, new PeopleCard {
                name = "T-One A.",
                image = handlerSprites[1]
            });
            _peopleCards.Add(People.Two, new PeopleCard {
                name = "T-Two A.",
                image = handlerSprites[2]
            });

            handlerText.m_textUI.text = _peopleCards[People.Two].name;
            handlerImage.sprite = _peopleCards[People.Two].image;

            mainItem.DisableAllItems();
            backdrop.DOFade(0, 0);
            ditherScreen.DOFade(0, 0);

            _originalMainSize = mainRect.sizeDelta;
            
            missionBriefText.text = missionBriefs[0].text;
            missionDiffText.text = "SAFE";
            missionTitleText.text = missionNames[0];

            currentLevel = 0;
            
            foreach (var button in buttonSelectors) {
                if (buttonSelectors.IndexOf(button) == 0) {
                    button.isDisabled = true;
                    continue;
                }

                button.isDisabled = false;
            }
            
            loadingGroup.alpha = 0;
        }

        public void Open() {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            mainItem.canvas.enabled = true;
            
            if (volume.profile.TryGetSettings(out LensDistortion t)) {
                DOVirtual.Float(0, 40, 1, value => {
                    t.intensity.value = value;
                });
            }
            
            //Start
            mainRect.sizeDelta = Vector2.zero;
            backdrop.DOFade(0, 0);
            selectorGroup.alpha = 0;
            
            //Play
            _sequence
                .Append(backdrop.DOFade(1, openingScreenDuration).SetEase(Ease.InBounce))
                .Append(DOVirtual.DelayedCall(0, () => {
                    DOVirtual.Float(0, _originalMainSize.x, openingScreenDuration, value => {
                        mainRect.sizeDelta = new Vector2(value, mainRect.sizeDelta.y);
                    }).SetEase(Ease.InQuint);

                    DOVirtual.Float(0, _originalMainSize.y, openingScreenDuration, value => {
                        mainRect.sizeDelta = new Vector2(mainRect.sizeDelta.x, value);
                    });
                }))
                .Append(DOVirtual.DelayedCall(1.5f, () => { }))
                .Append(ditherScreen.DOFade(0.2f, 0.8f))
                .Append(selectorGroup.DOFade(1, 1.5f).SetEase(selectorCurve))
                .OnComplete(() => {
                    mainItem.raycaster.enabled = true;
                });
        }

        public void Close() {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            mainItem.raycaster.enabled = false;
            volume.profile.TryGetSettings(out LensDistortion t);
            
            //Play
            _sequence
                .Append(ditherScreen.DOFade(0f, 0.8f))
                .Append(selectorGroup.DOFade(0, 1.5f).SetEase(selectorCurve))
                .Append(DOVirtual.DelayedCall(1.5f, () => { }))
                .Append(DOVirtual.Float(t.intensity.value, 0, 1, value => {
                    t.intensity.value = value;
                }))
                .Append(DOVirtual.DelayedCall(0, () => {
                    DOVirtual.Float(_originalMainSize.x, 0, openingScreenDuration, value => {
                        mainRect.sizeDelta = new Vector2(value, mainRect.sizeDelta.y);
                    }).SetEase(Ease.InQuint);

                    DOVirtual.Float(_originalMainSize.y, 0, openingScreenDuration, value => {
                        mainRect.sizeDelta = new Vector2(mainRect.sizeDelta.x, value);
                    });
                }))
                .Append(backdrop.DOFade(0, openingScreenDuration).SetEase(Ease.InBounce))
                .OnComplete(() => {
                    mainItem.canvas.enabled = false;
                    mainMenu.OpenMenu();
                });
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                foreach (var typewriter in typewriters) {
                    typewriter.Skip();
                }
            }
        }

        public void SelectMission(int mission) {
            _switchMissionSequence?.Kill();
            
            foreach (var button in buttonSelectors) {
                if (buttonSelectors.IndexOf(button) == mission) {
                    button.isDisabled = true;
                    continue;
                }

                button.isDisabled = false;
            }

            foreach (var typewriter in typewriters) {
                typewriter.Skip();
                
                var diff = mission switch {
                    0 => "SAFE",
                    1 => "MEDIUM",
                    2 => "HARD",
                    3 => "SAFE",
                    _ => "SAFE"
                };
                
                var s = typewriters.IndexOf(typewriter) switch {
                    0 => missionNames[mission],
                    1 => diff,
                    2 => missionBriefs[mission].ToString(),
                    _ => ""
                };
                
                typewriter.Play(s, typewriters.IndexOf(typewriter) == 2 ? 30 : 10, null);
            }

            var people = mission switch {
                0 => _peopleCards[People.Two],
                1 => _peopleCards[People.One],
                2 => _peopleCards[People.One],
                3 => _peopleCards[People.Overseer],
                _ => _peopleCards[People.One]
            };
             
            handlerText.Skip();
            handlerText.Play(people.name, 10, null);

            _switchMissionSequence = DOTween.Sequence();
            _switchMissionSequence
                .Append(handlerTransform.DOLocalMoveY(-100, 1.5f).OnStart(() => {
                    handlerImage.DOFade(0, 1.5f).OnComplete(() => {
                        handlerImage.sprite = people.image;
                    });
                }))
                .Append(handlerTransform.DOLocalMoveY(0, 1.5f).OnStart(() => {
                    handlerImage.DOFade(1, 1.5f);
                }));

            currentLevel = mission;
        }

        public void StartGame() {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            loadingGroup.alpha = 0;
            mainItem.raycaster.enabled = false;
            var dur = 0.2f;
            _sequence
                .Append(startButton.image.DOColor(startButton.defaultColor, dur))
                .Append(startButton.image.DOColor(startButton.onHoverColor, dur))
                .Append(startButton.image.DOColor(startButton.defaultColor, dur))
                .Append(startButton.image.DOColor(startButton.onHoverColor, dur))
                .Append(startButton.image.DOColor(startButton.defaultColor, dur))
                .Append(startButton.image.DOColor(startButton.onHoverColor, dur))
                .Append(selectorGroup.DOFade(0, 0.8f).SetEase(selectorCurve))
                .Append(loadingGroup.DOFade(1, 1.5f).SetEase(selectorCurve))
                .OnComplete(() => {
                    missionLoading.StartLoading(currentLevel);
                });
        }
    }
}
