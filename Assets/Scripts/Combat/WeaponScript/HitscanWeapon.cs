using Sirenix.OdinInspector;

namespace Combat.WeaponScript {
    public class HitscanWeapon : Weapon {
        [TitleGroup("Hitscan Config")] 
        public float range; 
        
        protected override void OnWeaponFire() {
            if (!IsFiringPointValid()) return;
            foreach (var point in firingPoints) {
                
            }
        }
    }
}