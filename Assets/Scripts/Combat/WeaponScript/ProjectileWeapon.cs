 using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.WeaponScript {
    public class ProjectileWeapon : Weapon {
        [TitleGroup("Projectile Config")] 
        public GameObject projectile;
        public ProjectileSetting setting;
        public List<Transform> firingPoints;
        
        protected override void OnWeaponFire() {
            if (firingPoints.Count <= 0) return;
            foreach (var point in firingPoints) {
                var projectileInst = Instantiate(projectile, point.position, point.transform.rotation);
                projectileInst.GetComponent<Projectile>().Init(setting);
            }
        }
    }
}