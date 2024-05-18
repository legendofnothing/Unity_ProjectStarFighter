using System.Collections;
using PlayerScript;
using UnityEngine;

namespace Audio {
    public class AudioDistance : MonoBehaviour {
        private AudioSource _audioSource;
        private bool _ready;
        private Transform _target;

        private float _minDist;

        public void Init(AudioClip clip, Transform transform, float maxDistance, float minDist) {
            _audioSource = GetComponent<AudioSource>();
            _target = transform;
            _minDist = minDist;
            transform.position = _target.position;
            _audioSource.clip = clip;
            _audioSource.Play();
            Destroy(gameObject, clip.length);
            _ready = true;
        }

        private void Update() {
            if (!_ready) return;
            float dist = Vector3.Distance(transform.position, Player.Instance.PlayerPos);
            if (dist <= _minDist) {
                _audioSource.volume = 1;
            }
            else {
                Debug.Log(dist);
                _audioSource.volume = _minDist * (1 / (1 + AudioGlobalValues.LogarithmDropOffScale * (dist - 1)));
            }
        }
    }
}
