using System.Collections.Generic;
using BehaviorTree;
using Combat;
using Core.Events;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using EnemyScript.v2.BehaviorTree.Variations.EnemyCommander;
using EnemyScript.v2.StateMachine;
using PlayerScript;
using Sirenix.OdinInspector;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace EnemyScript.v2.Commander {
    public class CommanderV2 : TroopCommander {
        private global::BehaviorTree.BehaviorTree _mainTree;

        [TitleGroup("Attack Behaviors")] 
        public CommanderObserve observeBehavior;
        public CommanderAttack attackBehavior;

        [TitleGroup("Commander Observe Config")] 
        public float minTroopCountToCancelCommand = 1f;
        public float dangerPlayerDistance = 4f;
        public float maximumTimeDamageTakenWhileObserving = 2f;

        [TitleGroup("Commander Attack Config")]
        public float maximumTimeSpentAttack = 1.5f;
        public float maximumTimeDamageTakenWhileAttack = 5f;

        [TitleGroup("Commander Other Values")] 
        public float chanceToMakeDecisionOnPowerValue = 0.7f;
        public float minSafeRatio = 0.1f;
        public float maxSafeRatio = 0.6f;
        public float commanderSuicidalTendencies = 0.1f;

        [TitleGroup("Ref")]
        public EnemyRadar enemyRadar;
        public Rigidbody2D rb;

        private float _currentTimeTakenDamage;
        private float _currentTimeSpentAttacking;
        private bool _isAttacking;
        private Projectile _currentThreat;
        
        private float _currentPowerValue;
        private float _maximumPowerValue;
        private float _powerRatio => _currentPowerValue / _maximumPowerValue;
        
        protected override void OnStart() {
            SendCommand(new TroopCommand {
                command = Commands.Command,
                commander = this
            });
        }

        protected override void OnAwake() {
            observeBehavior.canRun = false;
            attackBehavior.canRun = false;
            
            if (troopsToSpawn.Count <= 0) return;
            var ogPosition = (Vector2)transform.position + Vector2.one;
            foreach (var troop in troopsToSpawn) {
                var inst = Instantiate(troop, ogPosition, transform.rotation);
                var tr = inst.GetComponent<Troop>();
                
                AddTroop(tr);
                tr.commander = this;

                _currentPowerValue += troop.powerValue;
            }

            _maximumPowerValue = _currentPowerValue;

            _mainTree = new global::BehaviorTree.BehaviorTree(new List<Node> {
                new Selector(new List<Node> {
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => troopCount <= minTroopCountToCancelCommand)),
                        new Decorator(new Condition(() => !attackBehavior.canRun)),
                        new Decorator(new Actions(SetAttack))
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => _isAttacking)),
                        new Decorator(new Actions(() => _currentTimeSpentAttacking += Time.fixedDeltaTime)),
                        new Decorator(new Condition(() => _currentTimeTakenDamage >= maximumTimeDamageTakenWhileAttack || _currentTimeSpentAttacking >= maximumTimeSpentAttack)),
                        new Decorator(new Actions(() => {
                            _isAttacking = false;
                            _currentTimeTakenDamage = 0;
                            _currentTimeSpentAttacking = 0;

                            SetObserve();
                        }))
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => !_isAttacking)),
                        
                        new Selector(new List<Node> {
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => _currentThreat)),
                                new Decorator(new Actions(() => {
                                    if (_currentThreat == null) return;

                                    if (!PredictPosition.HasInterceptDirection(
                                            _currentThreat.transform.position,
                                            transform.position,
                                            _currentThreat.Velocity,
                                            rb.velocity.magnitude,
                                            out _)) return;
                                    SetAttack();
                                })),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => _currentTimeTakenDamage >= maximumTimeDamageTakenWhileObserving)),
                                new Decorator(new Actions(SetAttack)),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => attackBehavior.stateMachine.enemy.GetDistanceToPlayer <= dangerPlayerDistance)),
                                new Decorator(new Actions(SetAttack)),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => !observeBehavior.canRun)),
                                new Decorator(new Actions(SetObserve))
                            })
                        }),
                    }),
                })
            });
        }


        private void Update() {
            _mainTree.Update();
            
            if (enemyRadar.CurrentHit == null) {
                _currentThreat = null;
            }
            else if (enemyRadar.CurrentHit.TryGetComponent<Projectile>(out var projectile)) {
                if (projectile.owner == Player.Instance 
                    && Vector2.Distance(transform.position, projectile.transform.position) <= 3f) {
                    _currentThreat = projectile;
                }
            }
        }

        public override void OnDeath() {
            SendCommand(new TroopCommand {
                command = Commands.CommanderDead,
                commander = this,
            });
        }

        public override void OnDamage() {
            _currentTimeTakenDamage++;
        }

        private void SetAttack() {
            _currentTimeTakenDamage = 0;
                            
            observeBehavior.canRun = false;
                            
            attackBehavior.MainTree.Reset();

            attackBehavior.stateMachine.SwitchState(Random.Range(0f, 1f) <= commanderSuicidalTendencies
                ? EnemyStates.Strafing
                : EnemyStates.ResettingAccel);

            attackBehavior.canRun = true;

            if (Random.Range(0f, 1f) <= chanceToMakeDecisionOnPowerValue) {
                var ratio = Random.Range(minSafeRatio, maxSafeRatio);
                if (_powerRatio <= ratio) {
                    SendCommand(new TroopCommand {
                        command = Commands.Command,
                        commander = this
                    });
                }
                else {
                    SendCommand(new TroopCommand {
                        command = Commands.Attack,
                        commander = this
                    });
                }
            }
            else {
                SendCommand(new TroopCommand {
                    command = Commands.Command,
                    commander = this
                });
            }

            _isAttacking = true;
        }

        private void SetObserve() {
            attackBehavior.canRun = false;
                            
            observeBehavior.MainTree.Reset();
            attackBehavior.stateMachine.SwitchState(Random.Range(0f, 1f) <= commanderSuicidalTendencies
                ? EnemyStates.Observing
                : EnemyStates.ResettingAccel);
            
            observeBehavior.canRun = true;
                            
            if (Random.Range(0f, 1f) <= chanceToMakeDecisionOnPowerValue) {
                var ratio = Random.Range(minSafeRatio, maxSafeRatio);
                if (_powerRatio <= ratio) {
                    SendCommand(new TroopCommand {
                        command = Commands.Attack,
                        commander = this
                    });
                }
                else {
                    SendCommand(new TroopCommand {
                        command = Commands.Command,
                        commander = this
                    });
                }
            }
            else {
                SendCommand(new TroopCommand {
                    command = Commands.Attack,
                    commander = this
                });
            }
        }

        protected override void OnTroopRemoved(Troop troop) {
            _currentPowerValue -= troop.powerValue;
            if (Random.Range(0f, 1f) <= commanderSuicidalTendencies) {
                SetAttack();
            }
        }

        protected override void OnTroopAdded() { }
        protected override void OnTroopRemoved() { }
    }
}