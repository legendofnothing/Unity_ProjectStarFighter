using System.Collections.Generic;
using System.Linq;
using Combat.WeaponScript;
using UnityEngine;

namespace EnemyScript {
    public class EnemyWeapon : MonoBehaviour {
        public List<Weapon> weapons = new();
        private int _currentIndex = 0;

        public ProjectileWeapon currentWeapon {
            get {
                if (weapons.ElementAtOrDefault(_currentIndex)) {
                    return (ProjectileWeapon) weapons[_currentIndex];
                }

                return null;
            }
        }

        public void ChangeWeapon(int index) {
            if (weapons[index] != null && _currentIndex != index) {
                _currentIndex = index;
            }
        }

        public void FireWeapon() {
            weapons[_currentIndex].FireWeapon();
        }
    }
}