using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu.MissionSelector {
    public class MissionSelectorLoading : MonoBehaviour {
        public Slider progressBar;
        public List<string> scenes = new();

        public TextMeshProUGUI overallStatus;
        public List<TextMeshProUGUI> statues = new();

        [Space] 
        public Image blackboard;
        public PostProcessVolume volume;
        public AnimationCurve loadingCurve;

        private int _level;
        //0.1
        //0.4
        //0.8

        private void Start() {
            progressBar.value = 0;
            blackboard.DOFade(0, 0);
        }

        public void StartLoading(int level) {
            _level = level;
            overallStatus.text = "Status: Loading";
            DOVirtual.Float(0, 1, 10f, value => {
                progressBar.value = value;

                if (Math.Abs(value - 0.1f) <= 0.01f) {
                    statues[0].text = "Core Components: Loaded";
                }
                
                else if (Math.Abs(value - 0.4f) <= 0.01f) {
                    statues[1].text = "Safety Components: Loaded";
                }
                
                else if (Math.Abs(value - 1f) <= 0.01f) {
                    overallStatus.text = "Status: Loaded";
                    statues[2].text = "Other Components: Loaded";
                }
            }).SetEase(loadingCurve).OnComplete(() => {
                DOVirtual.DelayedCall(1.5f, () => {
                    blackboard.DOFade(1, 0);
                    DOVirtual.DelayedCall(1.5f, () => {
                        SceneManager.LoadScene(scenes[level]);
                        DOTween.Clear(true);
                    });
                });
            });
        }
    }
}