using System.Collections.Generic;
using Core.Logging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.WeaponScript {
    public class ProjectileWeapon : Weapon {
        [TitleGroup("Projectile Config")] 
        public GameObject projectile;
        public ProjectileSetting setting;
        public MonoBehaviour owner;

        protected override void OnWeaponFire() {
            if (!IsFiringPointValid()) return;
            foreach (var point in firingPoints) {
                var projectileInst = Instantiate(projectile, point.position, point.transform.rotation);
                projectileInst.GetComponent<Projectile>().Init(setting, owner);
            }
        }
    }
}