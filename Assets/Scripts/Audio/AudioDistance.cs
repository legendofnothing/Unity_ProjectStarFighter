using System;
using System.Collections;
using DG.Tweening;
using PlayerScript;
using UnityEngine;

namespace Audio {
    public class AudioDistance : MonoBehaviour {
        private AudioSource _audioSource;
        private bool _ready;
        private Transform _target;

        private float _minDist;

        public void Init(AudioClip clip, Transform targetTransform, Action destroyCallback, bool looped = false, bool attachedToTransform = false, float minDist = 1) {
            _audioSource = GetComponent<AudioSource>();
            _target = targetTransform;
            _minDist = minDist;
            if (attachedToTransform) {
                transform.SetParent(_target);
                transform.localPosition = Vector3.zero;
            }
            else {
                transform.position = _target.position;
            }
            _audioSource.clip = clip;
            _audioSource.Play();
            _audioSource.loop = looped;
            try {
                if (!looped) {
                    DOVirtual.DelayedCall(clip.length, () => {
                        destroyCallback?.Invoke();
                        Destroy(gameObject);
                    });
                }
            }
            catch (Exception e) {
                // ignored
                Debug.Log(e);
            }
            _ready = true;
        }

        private void Update() {
            if (!_ready) return;
            float dist = Vector3.Distance(transform.position, Player.Instance.PlayerPos);
            if (dist <= _minDist) {
                _audioSource.volume = 1;
            }
            else {
                _audioSource.volume = _minDist * (1 / (1 + AudioGlobalValues.LogarithmDropOffScale * (dist - 1)));
            }
        }
    }
}
