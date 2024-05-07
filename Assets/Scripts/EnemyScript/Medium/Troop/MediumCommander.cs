using System;
using System.Collections.Generic;
using BehaviorTree;
using Core;
using Core.Events;
using DG.Tweening;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using EnemyScript.Medium.MediumEnemyCommander;
using UnityEngine;
using Sequence = BehaviorTree.Sequence;

namespace EnemyScript.Medium.Troop {
    public class MediumCommander : TroopCommander {
        private bool _isRunning;
        private bool _locked;
        private global::BehaviorTree.BehaviorTree _bt;
        private Enemy _enemy;

        private bool _provoked;
        private bool _underProvoking;
        
        protected override void OnAwake() {
            _enemy = GetComponent<Enemy>();
        }
        
        protected override void OnStart() {
            SendCommand(new TroopCommand {
                command = Commands.LookForTroop,
                commander = this
            });
            
            DisableAllState();
            
            _bt = new global::BehaviorTree.BehaviorTree(new List<Node> {
                new Selector(new List<Node> {
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => troopCount <= 0)),
                        new Decorator(new Actions(() => {
                            SwitchState(State.Attack);
                        }))
                    }),
                    
                    new Selector(new List<Node> {
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => {
                                if (_provoked) {
                                    return _provoked;
                                }

                                if (_enemy.GetDistanceToPlayer <= 8f) {
                                    _provoked = true;
                                }

                                return _provoked;
                            })),
                            
                            new Decorator(new Actions(() => {
                                if (_provoked && !_underProvoking) {
                                    _underProvoking = true;
                                    
                                    DOVirtual.DelayedCall(10f, () => {
                                        _provoked = false;
                                        _underProvoking = false;
                                    });   
                                    
                                    if (attackState is MediumCommanderAttackStateMachine _esm) {
                                        _esm.SwitchState(MediumCommanderAttackStateMachine.EnemyState.Resetting);
                                    }
                                    
                                    SwitchState(State.Attack);
                                    SendCommand(new TroopCommand {
                                        command = Commands.Attack,
                                        commander = this
                                    });
                                }
                            }))
                        }),
                        
                        new Decorator(new Actions(() => {
                            SwitchState(State.Command);
                            SendCommand(new TroopCommand {
                                command = Commands.Attack,
                                commander = this
                            });
                        }))
                    })
                })
            });
        }
        
        public override void OnDeath() {
            SendCommand(new TroopCommand {
                commander = this,
                command = Commands.CommanderDead
            });
        }

        public override void OnDamage() {
            _provoked = true;
        }

        private void Update() {
            _isRunning = true;
            _bt.Update();
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
            
        }
    }
}