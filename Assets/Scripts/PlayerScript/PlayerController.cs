using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScript {
    public class PlayerController : MonoBehaviour {
        [TitleGroup("Config")] 
        public float speed = 2f;
        public float speedDampValue = .2f;
        public float angularSpeed = 2f;
        
        [TitleGroup("ReadOnly")] 
        [ReadOnly] public Vector2 direction;
        
        private Rigidbody2D _rb;
        private Vector2 _refVel = Vector2.zero;
        private Quaternion _refRot;
        private Quaternion _rotationToMouse;

        private void Start() {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            //input
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");
            direction = new Vector2(x, y).normalized;
            
            //rotation
            var rotDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var angleDiff = Mathf.Atan2(rotDir.y, rotDir.x) * Mathf.Rad2Deg - 90f;
            _rotationToMouse = Quaternion.AngleAxis(angleDiff, Vector3.forward);
        }

        private void FixedUpdate() {
            _rb.velocity = 
                Vector2.SmoothDamp(_rb.velocity, direction * speed, ref _refVel, speedDampValue);
            _rb.SetRotation(Quaternion.RotateTowards(transform.rotation, _rotationToMouse, angularSpeed));
        }
    }
}
