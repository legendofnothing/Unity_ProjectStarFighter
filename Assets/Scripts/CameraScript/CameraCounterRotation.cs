using UnityEngine;

namespace CameraScript {
    public class CameraCounterRotation : MonoBehaviour {
        private void LateUpdate() {
            if (transform.parent != null) transform.rotation = Quaternion.Euler (0.0f, 0.0f, transform.parent.rotation.z * -1.0f);
        }
    }
}
