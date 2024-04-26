using Core.Events;
using DG.Tweening;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;

namespace EnemyScript.Medium.Troop {
    public class MediumCommander : TroopCommander {
        protected override void OnStart() {
            SendCommand(new TroopCommand {
                command = Commands.LookForTroop,
                commander = self
            });

            DOVirtual.DelayedCall(0.8f, () => {
                SwitchState(troops.Count <= 0 ? State.Attack : State.Command);
            });
        }
    }
}