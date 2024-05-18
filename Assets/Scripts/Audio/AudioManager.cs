using System.Collections.Generic;
using Core.Patterns;
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

        public void PlaySFX(AudioClip clip) {
            _audioSources[AudioType.SfxInside].PlayOneShot(clip);
        }

        public void PlaySFX(AudioClip clip, Transform transform) {
            var inst = new GameObject("AudioSFXInstance", typeof(AudioSource), typeof(AudioDistance));
            var outAudio = inst.GetComponent<AudioSource>();
            var distance = inst.GetComponent<AudioDistance>();
            outAudio.outputAudioMixerGroup = _sfxOutsideGroup;
            distance.Init(clip, transform, 50, 1);
        }

        public void PlayMusic(AudioClip clip) {
            var source = _audioSources[AudioType.Music];
            if (source.isPlaying) {
                source.Stop();
            }

            source.volume = 1;
            source.clip = clip;
            source.Play();
        }
    }
}
