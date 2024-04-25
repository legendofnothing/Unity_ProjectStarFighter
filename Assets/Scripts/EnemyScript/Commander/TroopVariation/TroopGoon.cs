using System;
using DG.Tweening;

namespace EnemyScript.Commander.TroopVariation {
    public class TroopGoon : Troop {
        protected override void OnStart() {
            DOVirtual.DelayedCall(1.2f, () => {
                if (!commander) {
                    attackState.enabled = true;
                    underCommandState.enabled = false;
                } 
            });
        }

        protected override void RespondToCall(TroopCommand command) {
            
        }
    }
}