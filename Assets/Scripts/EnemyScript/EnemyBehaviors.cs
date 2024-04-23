using Combat.WeaponScript;
using PlayerScript;
using UnityEngine;

namespace EnemyScript {
    public class EnemyBehaviors : MonoBehaviour {
        [HideInInspector] public float speedDampValue;
        
        private Rigidbody2D _rigidbody;
        private Weapon _weapon;
        private Vector2 _refVel = Vector2.zero;
        
        private void Start() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _weapon = GetComponent<Weapon>();
        }
        
        public void LookAt(Vector3 point, float maxRotationDelta) {
            var dir = (point - transform.position).normalized;
            dir.z = 0;
            var rot = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
            _rigidbody.SetRotation(Quaternion.RotateTowards(transform.rotation, rot, maxRotationDelta));
        }
        
        public void FireWeapon() {
            _weapon.FireWeapon();
        }

        public void FlyForward(float speed) {
            _rigidbody.velocity 
                = Vector2.SmoothDamp(_rigidbody.velocity, transform.up * speed, ref _refVel, speedDampValue);
        }

        public void Fly(float speed, Vector2 direction) {
            _rigidbody.velocity 
                = Vector2.SmoothDamp(_rigidbody.velocity, direction * speed, ref _refVel, speedDampValue);
        }
    }
}
