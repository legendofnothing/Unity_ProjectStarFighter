using System.Collections.Generic;
using Combat.WeaponScript;
using UnityEngine;

namespace EnemyScript {
    public class EnemyWeapon : MonoBehaviour {
        public List<Weapon> weapons = new();
        private int _currentIndex = 0;

        public void ChangeWeapon(int index) {
            if (weapons[index] != null) {
                _currentIndex = index;
            }
        }

        public void FireWeapon() {
            weapons[_currentIndex].FireWeapon();
        }
    }
}