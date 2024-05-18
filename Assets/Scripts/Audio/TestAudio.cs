using System.Collections.Generic;
using UnityEngine;

namespace Audio {
    public class TestAudio : MonoBehaviour {
        public List<AudioClip> adios = new();

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                AudioManager.Instance.PlayMusic(adios[0]);
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                AudioManager.Instance.PlaySFX(adios[1]);
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                AudioManager.Instance.PlaySFX(adios[0], transform);
            }
        }
    }
}