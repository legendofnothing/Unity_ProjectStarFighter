using UnityEngine;

namespace Audio {
    public class AudioSfxPlay : MonoBehaviour{ 
        public void PlayAudio(AudioClip clip) {
            AudioManager.Instance.PlaySFXOneShot(clip);
        }
    }
}
