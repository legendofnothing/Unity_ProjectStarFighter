using Audio;
using Core;
using PlayerScript;
using UnityEngine;

namespace Effect {
    public class EngineEffect : MonoBehaviour {
        public Rigidbody2D rb;
        public float maxSpeed;
        public float ratioToMaxThrust;
        [Space] 
        public bool isPlayer;
        public float maxVolume;
        public AudioSource audioSource;
        public AudioClip engineNoise;
        
        private SpriteRenderer _renderer;

        private void Start() {
            _renderer = GetComponent<SpriteRenderer>();
            audioSource.clip = engineNoise;
            audioSource.loop = true;
            audioSource.volume = 0;
            audioSource.Play();
            AudioManager.Instance.AddToSfxSource(audioSource);
        }

        private void Update() {
            if (rb) {
                var t = maxSpeed * ratioToMaxThrust;
                var r = Mathf.Clamp(rb.velocity.magnitude, 0, t) / t;
                _renderer.color = new Color(1, 1, 1, r);
                audioSource.volume = Mathf.Clamp(r, 0, maxVolume);
            }

            if (!isPlayer) {
                var minDist = 1;
                float dist = Vector3.Distance(transform.position, Player.Instance.PlayerPos);
                if (dist <= minDist) {
                    audioSource.volume = 1;
                }
                else {
                    var value = 1 * (1 / (1 + AudioGlobalValues.LogarithmDropOffScale * (dist - 1)));
                    audioSource.volume = value;
                }
            }
        }

        private void OnDestroy() {
            if (AudioManager.Instance) AudioManager.Instance.RemoveFromSource(audioSource);
        }
    }
}