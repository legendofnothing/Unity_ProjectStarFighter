using Combat.WeaponScript;
using PlayerScript;
using UnityEngine;

namespace EnemyScript {
    public class EnemyBehaviors : MonoBehaviour {
        private Rigidbody2D _rigidbody;
        private Weapon _weapon;
        
        private void Start() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _weapon = GetComponent<Weapon>();
        }
        
        protected void LookAt(Vector3 point, float maxRotationDelta) {
            var dir = (point - transform.position).normalized;
            dir.z = 0;
            var rot = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rot, maxRotationDelta));
        }

        protected void FireWeapon() {
            _weapon.FireWeapon();
        }

        protected void FlyForward(float speed) {
            _rigidbody.velocity = transform.up * speed;
        }
    }
}
