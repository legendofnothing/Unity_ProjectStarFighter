using System.Collections.Generic;
using System.Linq;
using Combat.WeaponScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat {
    public class CombatManager : MonoBehaviour {
        [TitleGroup("Config")] 
        public List<Weapon> weapons = new();
        private Weapon _currentWeapon;
        private int _lastEquippedIndex = 0;

        private void Start() {
            foreach (var weapon in weapons) {
                weapon.transform.gameObject.SetActive(false);
            }
            _currentWeapon = weapons[0];
            _currentWeapon.gameObject.SetActive(true);
        }

        private void Update() {
            if (Input.GetKey(KeyCode.Mouse0)) _currentWeapon.FireWeapon();
            else if (Input.GetKeyDown(KeyCode.Q)) SwitchWeapon(_lastEquippedIndex);
            else if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeapon(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchWeapon(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6)) SwitchWeapon(5);
            else if (Input.GetKeyDown(KeyCode.Alpha7)) SwitchWeapon(6);
            else if (Input.GetKeyDown(KeyCode.Alpha8)) SwitchWeapon(7);
            else if (Input.GetKeyDown(KeyCode.Alpha9)) SwitchWeapon(8);
            else if (Input.GetKeyDown(KeyCode.Alpha0)) SwitchWeapon(9);
        }

        private void SwitchWeapon(int index) {
            if (weapons.ElementAtOrDefault(index) == null || weapons.IndexOf(_currentWeapon) == index) return;
            _lastEquippedIndex = weapons.IndexOf(_currentWeapon);
            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = weapons[index];
            _currentWeapon.gameObject.SetActive(true);
            _currentWeapon.OnWeaponSwitched();
        }
    }
}