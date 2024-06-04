using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using UnityEngine;

namespace EnemyScript.v2.TwinFighter {
    public class TwinFighter : TroopGoon {
        protected override void OnStart() { }

        public override void OnDamage() { }

        protected override void RespondToCall(TroopCommand command) { }
    }
}