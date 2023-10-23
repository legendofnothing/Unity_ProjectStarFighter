using CameraScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerScript {
    public class PlayerCamera : MonoBehaviour {
        [TitleGroup("Mouse Follow Config")] 
        public float x;
        public float y;
        
        private Transform _cameraPoint;

        private void Awake() {
            _cameraPoint = FindObjectOfType<CameraPoint>().transform;
        }
        
        private void FixedUpdate() {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            var playerPos = transform.position;
            var target = (playerPos + mousePos) / 2;
            target = new Vector3(
                Mathf.Clamp(target.x, -x + playerPos.x, x + playerPos.x),
                Mathf.Clamp(target.y, -y + playerPos.y, y + playerPos.y));
            _cameraPoint.transform.position = target;
        }
    }
}