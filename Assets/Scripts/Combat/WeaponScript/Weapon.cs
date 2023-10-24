using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.WeaponScript {
    public abstract class Weapon : MonoBehaviour {
        [TitleGroup("Config")] 
        public float fireDelay = 1f;

        [TitleGroup("Readonly")]
        [ReadOnly] public bool isFiring;

        public void FireWeapon() {
            if (isFiring) return;
            OnWeaponFire();
            StartCoroutine(FireDelay());
        }

        public virtual void OnWeaponSwitched() {
            StopAllCoroutines();
            isFiring = false;
        }

        private IEnumerator FireDelay() {
            isFiring = true;
            yield return new WaitForSeconds(fireDelay);
            isFiring = false;
        }
        
        protected abstract void OnWeaponFire();
    }
}