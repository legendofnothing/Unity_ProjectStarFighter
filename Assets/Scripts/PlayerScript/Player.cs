using Core.Patterns;
using Unity.Collections;
using UnityEngine;

namespace PlayerScript {
    public class Player : Singleton<Player> {
        public float hp;
        [ReadOnly] public float currentHp;

        private bool _hasDied;

        public void TakeDamage(float amount) {
            if (_hasDied) return;
            currentHp -= amount;
            if (currentHp <= 0) {
                currentHp = 0;
                Death();
            }
        }

        public void Death() {
            Debug.Log("death");
            _hasDied = true;
        }
    }
}
