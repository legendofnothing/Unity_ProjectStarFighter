using UnityEngine;

namespace Effect {
    public class EffectBase : MonoBehaviour {
        public void Init(Vector3 size) {
            transform.localScale = size;
        }

        public void OnAnimationFinish() {
            Destroy(gameObject);
        }
    }
}
