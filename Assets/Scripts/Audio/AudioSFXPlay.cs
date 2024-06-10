using UnityEngine;

namespace Audio {
    public class AudioSFXPlay : MonoBehaviour{ 
        public void PlayAudio(AudioClip clip) {
            //s
            AudioManager.Instance.PlaySFXOneShot(clip);
        }
    }
}
