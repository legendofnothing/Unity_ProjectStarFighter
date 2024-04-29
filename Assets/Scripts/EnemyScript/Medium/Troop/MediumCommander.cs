using Core;
using Core.Events;
using DG.Tweening;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using UnityEngine;

namespace EnemyScript.Medium.Troop {
    public class MediumCommander : TroopCommander {
        private bool _isRunning;
        
        protected override void OnStart() {
            SendCommand(new TroopCommand {
                command = Commands.LookForTroop,
                commander = this
            });

            if (troops.Count <= 0) {
                SwitchState(State.Attack);
            }
            
            else {
                SwitchState(State.Command);
                SendCommand(new TroopCommand {
                    commander = this,
                    command = Commands.Attack
                });
            }
        }

        public override void OnDeath() {
            
        }

        private void Update() {
            _isRunning = true;
        }

        private void OnDrawGizmos() {
            if (!_isRunning) return;
            DebugExtension
                .DrawString($"Troop Count: {troopCount}", 
                    transform.position - new Vector3(0.5f, 0),
                    Color.white, 
                    Vector2.zero);
        }

        protected override void OnTroopAdded() {
        }

        protected override void OnTroopRemoved() {
            if (troopCount <= 0) {
                SwitchState(State.Attack);
            }
        }
    }
}