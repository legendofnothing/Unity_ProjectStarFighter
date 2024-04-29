using System;
using Core;
using Core.Events;
using DG.Tweening;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using EnemyScript.Medium.MediumEnemyCommander;
using EnemyScript.Medium.MediumEnemyTroop;
using UnityEngine;

namespace EnemyScript.Medium.Troop {
    public class MediumCommander : TroopCommander {
        private bool _isRunning;
        private bool _locked;
        
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

        protected override void OnAwake() {
        }

        public override void OnDeath() {
            SendCommand(new TroopCommand {
                commander = this,
                command = Commands.CommanderDead
            });
        }

        public override void OnDamage() {
            if (currentState == State.Command) {
                MakeDecision(State.Attack);
            }
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
            _locked = false;
        }

        protected override void OnTroopRemoved() {
            if (troopCount <= 0) {
                SwitchState(State.Attack);
                _locked = true;
            }
        }

        public void MakeDecision(State requestState, MediumCommanderAttackStateMachine.EnemyState commanderAttackState = MediumCommanderAttackStateMachine.EnemyState.Resetting) {
            if (_locked) return;
            switch (requestState) {
                case State.Attack:
                    if (currentState == State.Attack) return;
                    
                    if (attackState is MediumCommanderAttackStateMachine esm) {
                        esm.SwitchState(commanderAttackState);
                    }
                    
                    foreach (var troop in troops) {
                        troop.SwitchState(State.Command);
                        var casted = (EnemyTroopStateMachine) troop.commandState;
                        casted.SwitchState(EnemyTroopStateMachine.EnemyState.Observing);
                    }
                    SwitchState(requestState);
                    break;
                case State.Command:
                    if (currentState == State.Command) return;
                    
                    if (commandState is MediumCommanderCommandStateMachine esn) {
                        esn.SwitchState(MediumCommanderCommandStateMachine.EnemyState.Observing);
                    }
                    
                    foreach (var troop in troops) {
                        troop.SwitchState(State.Attack);
                    }
                    SwitchState(requestState);
                    break;
            }
        }

        public void SendAllTroops() {
            if (troops.Count <= 0) return;
            foreach (var troop in troops) {
                troop.SwitchState(State.Attack);
            }
        }

        public void RequestToBattle() {
            if (self.currentHp / self.hp >= 0.4f) {
                MakeDecision(State.Attack, MediumCommanderAttackStateMachine.EnemyState.Strafing);
            };
        }
    }
}