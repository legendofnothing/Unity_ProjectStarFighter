using UnityEngine;

namespace UI {
    public class UICounterParentRotation : MonoBehaviour {
        private RectTransform _rect;

        private void Start() {
            _rect = GetComponent<RectTransform>();
        }

        private void Update() {
            _rect.rotation = Quaternion.Euler (0.0f, 0.0f, transform.parent.rotation.z * -1.0f);
        }
    }
}
