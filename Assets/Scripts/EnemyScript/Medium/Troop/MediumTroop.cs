using System;
using Core.Events;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;

namespace EnemyScript.Medium.Troop {
    public class MediumTroop : TroopGoon {
        protected override void RespondToCall(TroopCommand command) {
            switch (command.command) {
                case Commands.LookForTroop:
                    if (commander) return;
                    commander = command.commander;
                    if (commander is TroopCommander commanderTroop) {
                        commanderTroop.AddTroop(this);
                    }
                    break;
                case Commands.Attack:
                    SwitchState(State.Attack);
                    break;
            }
        }
    }
}