using System.Collections.Generic;
using Core.Events;
using DG.Tweening;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Audio {
    public class MusicHandler : MonoBehaviour {
        public List<AudioClip> soundtracks = new();
        public float delayBetweenEachTrack = 1.5f;

        [Space] 
        public AudioClip loseAudio;
        public float initialLoseDelay = 8f;
        
        [Space] 
        public AudioClip winAudio;
        public float initialWinDelay = 5f;

        private bool _isPausing;
        private int _currentMusicIndex = -1;

        private Tween _tweenDelay;
        private Tween _tweenDelayMusic;

        private void Start() {
            this.AddListener(EventType.OnPause, _ => OnPause());
            this.AddListener(EventType.OpenDeathUI, _ => OnDeath());
            this.AddListener(EventType.OpenWinUI, _ => OnWin(false));
            this.AddListener(EventType.OpenActualWinUI, _ => OnWin(true));
            _currentMusicIndex = -1;
            PlayMusic();
        }

        private void OnPause() {
            _isPausing = !_isPausing;
            AudioManager.Instance.SetMusicParam("MusicLowPass", _isPausing ? 0.989f : 0f);
        }

        private void PlayMusic() {
            AudioManager.Instance.StopMusic();
            _currentMusicIndex++;
            if (_currentMusicIndex >= soundtracks.Count) {
                _currentMusicIndex = 0;
            }
            
            _tweenDelayMusic?.Kill();
            _tweenDelay?.Kill();

            _tweenDelay = DOVirtual.DelayedCall(delayBetweenEachTrack, () => {
                AudioManager.Instance.PlayMusic(soundtracks[_currentMusicIndex]);
                _tweenDelayMusic = DOVirtual.DelayedCall(soundtracks[_currentMusicIndex].length, PlayMusic);
            });
        }

        private void OnDeath() {
            AudioManager.Instance.StopMusic();
            _tweenDelayMusic?.Kill();
            _tweenDelay?.Kill();
            AudioManager.Instance.PlayMusic(loseAudio, initialLoseDelay);
        }

        private void OnWin(bool actual) {
            if (!actual) {
                AudioManager.Instance.StopMusic(true, 2f);
                return;
            }
            
            AudioManager.Instance.StopMusic();
            _tweenDelayMusic?.Kill();
            _tweenDelay?.Kill();
            AudioManager.Instance.PlayMusic(winAudio, initialWinDelay);
        }
    }
}
