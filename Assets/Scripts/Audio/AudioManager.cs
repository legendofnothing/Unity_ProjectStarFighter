using System.Collections.Generic;
using System.Linq;
using Core.Patterns;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio {
    public static class AudioGlobalValues {
        public const float LogarithmDropOffScale = 6f;
    }
    
    public class AudioManager : Singleton<AudioManager> {
        public enum AudioType {
            None,
            Music,
            SfxOutside,
            SfxInside,
        }

        private Dictionary<AudioType, AudioSource> _audioSources = new();
        private AudioMixer _mainMixer;
        private AudioMixerGroup _musicGroup;
        private AudioMixerGroup _sfxInsideGroup;
        private AudioMixerGroup _sfxOutsideGroup;
        private List<AudioSource> _sfxSources = new();

        private bool _isFadingMusic;

        private void Awake() {
            _mainMixer = Resources.Load<AudioMixer>("Audio/MainAudioController");
        
            _musicGroup = _mainMixer.FindMatchingGroups("Music")[0];

            var sfxGroups = _mainMixer.FindMatchingGroups("SFX");
            _sfxInsideGroup = sfxGroups[1];
            _sfxOutsideGroup = sfxGroups[2];
        
            for (var i = 0; i < 3; i++) {
                var inst = new GameObject(i == 0 ? "AudioSourceMusic" : "AudioSourceSFX", typeof(AudioSource));
                inst.transform.SetParent(transform);
                var outAudio = inst.GetComponent<AudioSource>();

                outAudio.outputAudioMixerGroup = i switch {
                    0 => _musicGroup,
                    1 => _sfxInsideGroup,
                    2 => _sfxOutsideGroup,
                    _ => null
                };
            
                var group = i switch {
                    0 => AudioType.Music,
                    1 => AudioType.SfxInside,
                    2 => AudioType.SfxOutside,
                    _ => AudioType.None
                };

                _audioSources[group] = outAudio;
            }
        }

        public void PlaySFXOneShot(AudioClip clip) {
            _audioSources[AudioType.SfxInside].PlayOneShot(clip);
        }

        public void PlaySFX(AudioClip clip) {
            var inst = new GameObject("AudioSFXInstance", typeof(AudioSource));
            var outAudio = inst.GetComponent<AudioSource>();
            outAudio.outputAudioMixerGroup = _sfxInsideGroup;
            outAudio.clip = clip;
            outAudio.Play();
            _sfxSources.Add(outAudio);
            
            DOVirtual.DelayedCall(clip.length, () => {
                _sfxSources.Remove(outAudio);
                Destroy(inst);
            });
        }

        public void PlaySFX(AudioClip clip, Transform transform, bool looped = false, bool attached = false, float minDist = 1) {
            var inst = new GameObject("AudioSFXInstance", typeof(AudioSource), typeof(AudioDistance));
            var outAudio = inst.GetComponent<AudioSource>();
            var distance = inst.GetComponent<AudioDistance>();
            outAudio.outputAudioMixerGroup = _sfxOutsideGroup;
            _sfxSources.Add(outAudio);
            distance.Init(clip, transform, () => {
                _sfxSources.Remove(outAudio);
            } , looped, attached, minDist);
        }

        public void PlayMusic(AudioClip clip, float delay = 0) {
            var source = _audioSources[AudioType.Music];
            if (source.isPlaying) {
                source.Stop();
            }

            source.volume = 1;
            source.clip = clip;
            source.loop = true;
            if (delay > 0) source.PlayDelayed(delay);
            else source.Play();
        }

        private Tween _currentParamTween;
        public void SetMusicParam(string key, float value, bool doFade = true, float fadeDuration = 0) {
            if (!_mainMixer.GetFloat(key, out var v)) return;
            
            if (key == "MusicHighPass") {
                if (doFade) {
                    _currentParamTween?.Kill();
                    var currentValue = (v - 10) / 22000f;
                    _currentParamTween = DOVirtual.Float(currentValue, value, fadeDuration, v2 => {
                        _mainMixer.SetFloat(key, Mathf.Lerp(10f, 22000f, v2));
                    });
                    return;
                }
                _mainMixer.SetFloat(key, Mathf.Lerp(10f, 22000f, value));
                return;
            }
                
            _mainMixer.SetFloat(key, value);
        }

        public void StopMusic(bool doFade = true, float fadeDuration = 0) {
            if (_audioSources[AudioType.Music].isPlaying && !_isFadingMusic) {
                _isFadingMusic = true;
                var source = _audioSources[AudioType.Music];
                if (!doFade) {
                    source.volume = 0;
                    _isFadingMusic = false;
                }
                else {
                    source.DOFade(0, fadeDuration)
                        .OnComplete(() => _isFadingMusic = false)
                        .SetEase(Ease.Linear)
                        .SetUpdate(true);
                }
            }
        }

        public bool IsMusicPlaying => _audioSources[AudioType.Music].isPlaying;
        
        public void PauseAllSound() {
            var temp = new List<AudioSource>(_sfxSources);
            foreach (var t in temp.Where(t => t)) {
                t.Pause();
            }
        }

        public void ResumeAllSound() {
            var temp = new List<AudioSource>(_sfxSources);
            foreach (var t in temp.Where(t => t)) {
                t.UnPause();
            }
        }

        public void AddToSfxSource(AudioSource source) {
            _sfxSources.Add(source);
        }

        public void RemoveFromSource(AudioSource source) {
            if (_sfxSources.Contains(source)) _sfxSources.Remove(source);
        }

        public void SetMusicVolume(float volume) {
            var temp = volume;
            if (temp <= 0) {
                temp = 0.0001f;
            }
            _mainMixer.SetFloat("MusicVolume", Mathf.Log10(temp) * 20);
        }
        
        public void SetSFXVolume(float volume) {
            var temp = volume;
            if (temp <= 0) {
                temp = 0.0001f;
            }
            _mainMixer.SetFloat("SFXVolume", Mathf.Log10(temp) * 20);
        }
    }
}
