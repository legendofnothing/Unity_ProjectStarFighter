using System.Collections.Generic;
using BehaviorTree;
using Core.Events;
using DG.Tweening;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using StateMachine;
using UnityEngine;
using EventType = Core.Events.EventType;
using Sequence = BehaviorTree.Sequence;

namespace EnemyScript.Hard {
    public class TwinFighter : TroopGoon {
        private global::BehaviorTree.BehaviorTree _bt;
        private bool _pendingAttack;
        private TwinFighterStateMachine _esm;
        private Enemy _self;

        private Tween _attackTween;
        
        protected override void OnAwake() { }
        public override void OnDamage() { }
        
        protected override void OnStart() {
            _esm = (TwinFighterStateMachine)attackState;
            _self = GetComponent<Enemy>();
            _esm.overrideResettingState = true;
            SwitchState(State.Attack);
            this.AddListener(EventType.SendCommand,param => RespondToCall((TroopCommand) param));
            
            _bt = new global::BehaviorTree.BehaviorTree(new List<Node> {
                
                new Selector(new List<Node> {
                        
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => commander != null)),
                        
                        new Decorator(new Condition(() => _pendingAttack && _esm.CurrentState == TwinFighterStateMachine.EnemyState.Idle)),
                        
                        new Decorator(new Actions(() => {
                            _pendingAttack = false;
                            _esm.SwitchState(TwinFighterStateMachine.EnemyState.Attack);
                        }))    
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => _esm.CurrentState == TwinFighterStateMachine.EnemyState.Resetting)),
                        
                        new Selector(new List<Node> {
                           new Sequence(new List<Node> {
                               new Decorator(new Condition(() => _self.currentHp / _self.hp <= 0.5f && commander)),
                               new Decorator(new Actions(() => {
                                   _esm.SwitchState(TwinFighterStateMachine.EnemyState.Retreating);
                                   _esm.overrideResettingState = true;
                               }))
                           }),
                           
                           new Decorator(new Actions(() => _esm.overrideResettingState = false))
                        }),
                    })
                }),
            });
        }

        private void Update() {
            _bt.Update();
        }

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
                    _pendingAttack = true;
                    break;
                
                default:
                    break;
            }
        }
    }
}
