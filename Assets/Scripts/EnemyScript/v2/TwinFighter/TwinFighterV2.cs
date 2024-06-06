using System;
using System.Collections.Generic;
using BehaviorTree;
using Combat;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using EnemyScript.TowerScript;
using EnemyScript.v2.BehaviorTree;
using EnemyScript.v2.BehaviorTree.Variations.EnemyTwin;
using EnemyScript.v2.Commander;
using EnemyScript.v2.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyScript.v2.TwinFighter {
    public class TwinFighterV2 : TroopGoon {
        [TitleGroup("Twin Fighter Refs")]
        public DefaultBehavior AttackNoCommanderBehavior;
        public DefaultBehavior AttackYesCommanderBehavior;

        [TitleGroup("Twin Fighter Config")] 
        public float maximumTimeSpentLuring = 2;
        public float maximumTimeDamageTaken = 2;
        public float maximumTimeDamageTakenToSnapOut = 1;
        [Space] 
        public float suicidalTendenciesPercentIncrease = 0.4f;
        public float mentallyUnstableRate = 0.1f;
        [Space] 
        public float overrideCircleDistance = 12f;

        private float _currentTimeSpentLuring;
        private float _lastCircleDistance;
        private Commands _currentCommand;

        private float CurrentTimeSpentLuring {
            get {
                if (AttackYesCommanderBehavior is TwinAttackTowerCommander tower) {
                    return tower.TimeSpentLuring;
                }

                return 0f;
            }
            set { }
        }

        private float _currentTimeTakenDamage;
        private global::BehaviorTree.BehaviorTree _mainTree;
        private bool _currentlyOverride;

        protected override void OnStart() {
            _lastCircleDistance = AttackYesCommanderBehavior.stateMachine.minCirclingDistance;
            AttackNoCommanderBehavior.canRun = false;
            AttackYesCommanderBehavior.canRun = false;
            if (commander) {
                AttackYesCommanderBehavior.canRun = true;
            }
            else {
                AttackNoCommanderBehavior.canRun = true;
            }


            if (!commander) {
                _mainTree = new global::BehaviorTree.BehaviorTree(new List<Node> {
                    new Selector(new List<Node> {
                        
                        new Sequence(new List<Node>()),
                        
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => _currentlyOverride)),
                            
                            new Selector(new List<Node> {
                                new Sequence(new List<Node> { 
                                    new Decorator(new Condition(() => !AttackNoCommanderBehavior.canRun)),
                                    new Decorator(new Actions(() => {
                                        if (AttackNoCommanderBehavior is TwinAttackNoCommander attack) {
                                            attack.currentSuicidalTendencies += attack.suicidalTendencies *
                                                                                suicidalTendenciesPercentIncrease;
                                        } 
                                        
                                        AttackNoCommanderBehavior.MainTree.Reset();
                                        AttackYesCommanderBehavior.MainTree.Reset();
                                    
                                        AttackNoCommanderBehavior.canRun = true;
                                        AttackYesCommanderBehavior.canRun = false;
                                        
                                        AttackNoCommanderBehavior.stateMachine.SwitchState(EnemyStates.Idle);
                                    })),
                                }),
                                
                                new Sequence(new List<Node> { 
                                    new Decorator(new Condition(() => _currentTimeTakenDamage >= maximumTimeDamageTakenToSnapOut)),
                                    new Decorator(new Actions(() => {
                                        _currentlyOverride = false;
                                        
                                        if (AttackNoCommanderBehavior is TwinAttackNoCommander attack) {
                                            attack.currentSuicidalTendencies = attack.suicidalTendencies;
                                        } 
                                        
                                        _currentTimeTakenDamage = 0;
                                        
                                        AttackNoCommanderBehavior.MainTree.Reset();
                                        AttackYesCommanderBehavior.MainTree.Reset();
                                        
                                        AttackNoCommanderBehavior.canRun = false;
                                        AttackYesCommanderBehavior.canRun = true;
                                        
                                        AttackYesCommanderBehavior.stateMachine.SwitchState(EnemyStates.Idle);
                                    }))
                                })
                            })
                        }),
                        
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => _currentTimeTakenDamage >= maximumTimeDamageTaken || CurrentTimeSpentLuring >= maximumTimeSpentLuring)),
                            new Decorator(new Actions(() => {
                                _currentTimeTakenDamage = 0;
                                CurrentTimeSpentLuring = 0;

                                if (Random.Range(0f, 1f) < mentallyUnstableRate) {
                                    _currentlyOverride = true;
                                }
                            }))
                        }),
                    })
                });
            }
            else {
                if (commander.gameObject.TryGetComponent<Tower>(out _)) {
                    _mainTree = new global::BehaviorTree.BehaviorTree(new List<Node> {
                    new Selector(new List<Node> {
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => _currentlyOverride)),
                            
                            new Selector(new List<Node> {
                                new Sequence(new List<Node> {
                                    new Decorator(new Actions(() => {
                                        if (AttackNoCommanderBehavior is not TwinAttackNoCommander attack) return;
                                        if (!(attack.currentSuicidalTendencies > attack.suicidalTendencies)) return;
                                        
                                        attack.currentSuicidalTendencies += attack.suicidalTendencies *
                                                                            suicidalTendenciesPercentIncrease;
                                        
                                        AttackNoCommanderBehavior.MainTree.Reset();
                                        AttackNoCommanderBehavior.stateMachine.SwitchState(EnemyStates.Idle);
                                    })),
                                }),
                                
                                new Sequence(new List<Node> { 
                                    new Decorator(new Condition(() => _currentTimeTakenDamage >= maximumTimeDamageTakenToSnapOut)),
                                    new Decorator(new Actions(() => {
                                        _currentlyOverride = false;
                                        _currentTimeTakenDamage = 0;
                                        
                                        if (AttackNoCommanderBehavior is TwinAttackNoCommander attack) {
                                            attack.currentSuicidalTendencies = attack.suicidalTendencies;
                                        } 
                                        
                                        AttackYesCommanderBehavior.MainTree.Reset();
                                        AttackYesCommanderBehavior.stateMachine.SwitchState(EnemyStates.Idle);
                                    }))
                                })
                            }),
                        }),
                        
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => _currentTimeTakenDamage >= maximumTimeDamageTaken)),
                            new Decorator(new Actions(() => {
                                _currentTimeTakenDamage = 0;

                                if (Random.Range(0f, 1f) < mentallyUnstableRate) {
                                    _currentlyOverride = true;
                                }
                            }))
                        }),
                    })
                });
                }
                else if (commander.gameObject.TryGetComponent<CommanderV2>(out _)) {
                    _mainTree = new global::BehaviorTree.BehaviorTree(new List<Node> {
                        new Selector(new List<Node> {
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => !_currentlyOverride)),
                                
                                new Selector(new List<Node> {
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => _currentTimeTakenDamage >= maximumTimeDamageTaken)),
                                        new Decorator(new Actions(() => {
                                            _currentTimeTakenDamage = 0;

                                            if (Random.Range(0f, 1f) < mentallyUnstableRate) {
                                                _currentlyOverride = true;
                                            }
                                        }))
                                    }),
                                    
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => _currentCommand == Commands.Command)),
                                        new Decorator(new Condition(() => !AttackYesCommanderBehavior.canRun)),
                                        new Decorator(new Actions(() => {
                                            
                                            AttackNoCommanderBehavior.canRun = false;
                                    
                                            AttackYesCommanderBehavior.MainTree.Reset();
                                            AttackYesCommanderBehavior.stateMachine.SwitchState(EnemyStates.Idle);
                                            AttackYesCommanderBehavior.canRun = true;
                                        }))
                                    }),
                                    
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => _currentCommand == Commands.Attack)),
                                        new Decorator(new Condition(() => !AttackNoCommanderBehavior.canRun)),
                                        new Decorator(new Actions(() => {
                                            AttackYesCommanderBehavior.canRun = false;
                                    
                                            AttackNoCommanderBehavior.MainTree.Reset();
                                            AttackNoCommanderBehavior.stateMachine.SwitchState(EnemyStates.Idle);
                                            AttackNoCommanderBehavior.canRun = true;
                                        }))
                                    }),
                                }),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => _currentTimeTakenDamage >= maximumTimeDamageTakenToSnapOut)),
                                new Decorator(new Actions(() => {
                                    _currentlyOverride = false;
                                    _currentTimeTakenDamage = 0;
                                    
                                    if (AttackNoCommanderBehavior is TwinAttackNoCommander attack) {
                                        attack.currentSuicidalTendencies = attack.suicidalTendencies;
                                    }
                                }))
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => !AttackNoCommanderBehavior.canRun)),
                                new Decorator(new Actions(() => {
                                    
                                    AttackYesCommanderBehavior.canRun = false;
                                    AttackNoCommanderBehavior.canRun = true;
                                    
                                    AttackNoCommanderBehavior.MainTree.Reset();
                                    AttackNoCommanderBehavior.stateMachine.SwitchState(EnemyStates.Strafing);
                                    
                                    AttackYesCommanderBehavior.stateMachine.minCirclingDistance = _lastCircleDistance;
                                    
                                    if (AttackNoCommanderBehavior is TwinAttackNoCommander attack) {
                                        attack.currentSuicidalTendencies += attack.suicidalTendencies *
                                                                            suicidalTendenciesPercentIncrease;
                                    }
                                }))
                            })
                        })
                    });
                }
            }
        }

        public override void OnDamage() {
            _currentTimeTakenDamage++;
        }

        protected override void RespondToCall(TroopCommand command) {
            _currentCommand = command.command;

            if (command.command != Commands.CommanderDead) return;
            
            _currentlyOverride = false;
            AttackNoCommanderBehavior.MainTree.Reset();
            AttackYesCommanderBehavior.MainTree.Reset();
                                
            AttackNoCommanderBehavior.canRun = true;
            AttackYesCommanderBehavior.canRun = false;
        }

        private void Update() {
            if (_mainTree.children.Count <= 0) return;
            _mainTree.Update();
        }
    }
}