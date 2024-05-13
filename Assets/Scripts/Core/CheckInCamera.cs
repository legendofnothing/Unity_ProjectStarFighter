using UnityEngine;

namespace Core {
    public static class CheckInCamera {
        public static bool IsInFrustum(this Camera camera, Vector3 worldPoint) {
            var screenPos = camera.WorldToScreenPoint(worldPoint);
            return screenPos.z >= camera.nearClipPlane
                   && screenPos.z <= camera.farClipPlane
                   && screenPos.x >= 0
                   && screenPos.x <= Screen.width
                   && screenPos.y >= 0
                   && screenPos.y <= Screen.height;
        }
    }
}