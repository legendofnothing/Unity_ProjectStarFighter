using System;
using Core.Events;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using UnityEngine;

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
                    if (commander != command.commander) return;
                    SwitchState(State.Attack);
                    break;
                case Commands.CommanderDead:
                    if (commander != command.commander) return;
                    commander = null;
                    SwitchState(State.Attack);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnStart() {
            
        }

        public override void OnDamage() {
            if (currentState == State.Command) {
                SwitchState(State.Attack);
            }
            
            Debug.Log($"{gameObject.name} damaged");
            if (commander is MediumCommander commanderTroop) {
                commanderTroop.RequestToBattle();
            }
        }
    }
}