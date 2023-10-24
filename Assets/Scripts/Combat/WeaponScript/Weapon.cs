using System.Collections;
using System.Collections.Generic;
using Core.Logging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.WeaponScript {
    public abstract class Weapon : MonoBehaviour {
        [TitleGroup("Config")] 
        public float fireDelay = 1f;
        public List<Transform> firingPoints;

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

        protected virtual IEnumerator FireDelay() {
            isFiring = true;
            yield return new WaitForSeconds(fireDelay);
            isFiring = false;
        }
        
        protected abstract void OnWeaponFire();

        protected bool IsFiringPointValid() {
            if (firingPoints.Count > 0) return true;
            NCLogger.Log($"No firing point setup in {gameObject.name}");
            return false;
        }
    }
}