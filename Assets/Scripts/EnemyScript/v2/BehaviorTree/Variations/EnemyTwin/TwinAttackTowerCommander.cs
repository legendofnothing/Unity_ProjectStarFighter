using System.Collections.Generic;
using BehaviorTree;
using Combat;
using EnemyScript.Commander.Variation;
using EnemyScript.v2.BehaviorTree.Variations.EnemyShooter;
using EnemyScript.v2.StateMachine;
using PlayerScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.v2.BehaviorTree.Variations.EnemyTwin {
    public class TwinAttackTowerCommander : AttackBehaviorMedium {
        [TitleGroup("Twin Attack No Commander Config")]
        public float towerDangerDistance = 12f;
        public float maximumTimePlayerNearTower = 1f;

        [Space] 
        public float maximumDistanceToCancelLure = 8f;
        public float minDistanceToCancelLure = 4f;

        [TitleGroup("Refs")] 
        public TroopGoon troopGoon;
        public EnemyRadar enemyRadar;
        
        private Projectile _currentThreat;
        private float _distancePlayerToTower;
        private float _timePlayerInDangerZone;
        
        protected override void SetupTree() {
            CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);
            lastHP = stateMachine.enemy.hp;
            
            MainTree = new global::BehaviorTree.BehaviorTree(new List<Node> {
                new Selector(new List<Node> {
                        
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Idle)),
                        new Decorator(new Actions(() => {
                            lastHP = stateMachine.enemy.currentHp;
                            if (troopGoon) {
                                if (_distancePlayerToTower < towerDangerDistance) {
                                    stateMachine.SwitchState(EnemyStates.Luring);
                                }
                                else {
                                    stateMachine.SwitchState(Random.Range(0, 2) < 1
                                        ? EnemyStates.Strafing
                                        : EnemyStates.Circling);
                                }
                            }
                            
                            else {
                                stateMachine.SwitchState(Random.Range(0, 2) < 1
                                    ? EnemyStates.Strafing
                                    : EnemyStates.Circling);
                            }
                        }))
                    }),
                        
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Strafing or EnemyStates.Circling or EnemyStates.Luring)),
                        new Selector(new List<Node> {
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => _currentThreat)),
                                new Decorator(new Actions(() => {
                                    if (_currentThreat == null) return;

                                    if (PredictPosition.HasInterceptDirection(
                                            _currentThreat.transform.position,
                                            transform.position,
                                            _currentThreat.Velocity,
                                            rb.velocity.magnitude,
                                            out var pos)) {
                                        
                                        if (stateMachine.enemy.GetDistanceToTarget(pos) >= 1.8f) return;
                                        SwitchToReset();
                                    }
                                })),
                            }),
                            
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => _distancePlayerToTower > towerDangerDistance)),
                                
                                new Sequence(new List<Node> {
                                    new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Strafing)),
                        
                                    new Selector(new List<Node> {
                                        new Sequence(new List<Node> {
                                            new Decorator(new Condition(() => stateMachine.enemy.GetDotToPoint(Player.Instance.PlayerPos) >= dotInFront)),
                                            new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= dangerDistance)),
                                            new Decorator(new Actions(SwitchToReset))
                                        }),
                            
                                        new Sequence(new List<Node> {
                                            new Decorator(new Condition(() => lastHP > stateMachine.enemy.currentHp)),
                                            new Decorator(new Actions(SwitchToReset))
                                        })
                                    }),
                                }),
                            
                                new Sequence(new List<Node> {
                                    new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Circling)),
                                    new Decorator(new Condition(() => lastHP > stateMachine.enemy.currentHp)),
                                    new Decorator(new Actions(SwitchToReset))
                                }),
                                
                                new Sequence(new List<Node> {
                                    new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Luring)),
                                    new Selector(new List<Node> {
                                        new Sequence(new List<Node> {
                                            new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToPlayer >= maximumDistanceToCancelLure || lastHP > stateMachine.enemy.currentHp)),
                                            new Decorator(new Actions(() => {
                                                stateMachine.SwitchState(Random.Range(0, 2) < 1
                                                    ? EnemyStates.Strafing
                                                    : EnemyStates.Circling);
                                            }))
                                        }),
                                        
                                        new Sequence(new List<Node> {
                                            new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToPlayer <= minDistanceToCancelLure)),
                                            new Decorator(new Actions(() => {
                                                stateMachine.SwitchState(EnemyStates.LuringAccel);
                                            }))
                                        }),
                                    })
                                }),
                                
                                new Sequence(new List<Node> {
                                    new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.LuringAccel)),
                                    new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToPlayer >= maximumDistanceToCancelLure)),
                                    new Decorator(new Actions(SwitchToReset))
                                }),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Actions(() => {
                                    _timePlayerInDangerZone += Time.fixedDeltaTime;
                                })),
                                new Decorator(new Condition(() => _timePlayerInDangerZone >= maximumTimePlayerNearTower)),
                                
                                new Decorator(new Actions(() => {
                                    _timePlayerInDangerZone = 0;
                                    switch (stateMachine.CurrentState) {
                                        case EnemyStates.Circling or EnemyStates.Strafing:
                                            stateMachine.SwitchState(EnemyStates.Luring);
                                            break;
                                        case EnemyStates.Luring:
                                            stateMachine.SwitchState(Random.Range(0, 2) < 1
                                                ? EnemyStates.Strafing
                                                : EnemyStates.Circling);
                                            break;
                                    }
                                }))
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Strafing)),
                        
                                new Selector(new List<Node> {
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => stateMachine.enemy.GetDotToPoint(Player.Instance.PlayerPos) >= dotInFront)),
                                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= dangerDistance)),
                                        new Decorator(new Actions(SwitchToReset))
                                    }),
                            
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => lastHP > stateMachine.enemy.currentHp)),
                                        new Decorator(new Actions(SwitchToReset))
                                    })
                                }),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Circling)),
                                new Decorator(new Condition(() => lastHP > stateMachine.enemy.currentHp)),
                                new Decorator(new Actions(SwitchToReset))
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Luring)),
                                new Selector(new List<Node> {
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToPlayer >= maximumDistanceToCancelLure || lastHP > stateMachine.enemy.currentHp)),
                                        new Decorator(new Actions(() => {
                                            stateMachine.SwitchState(Random.Range(0, 2) < 1
                                                ? EnemyStates.Strafing
                                                : EnemyStates.Circling);
                                        }))
                                    }),
                                        
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToPlayer <= minDistanceToCancelLure)),
                                        new Decorator(new Actions(() => {
                                            stateMachine.SwitchState(EnemyStates.LuringAccel);
                                        }))
                                    }),
                                })
                            }),
                                
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.LuringAccel)),
                                new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToPlayer >= maximumDistanceToCancelLure)),
                                new Decorator(new Actions(SwitchToReset))
                            }),
                        })
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Resetting or EnemyStates.ResettingAccel)),
                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= CurrentResetDistance)),
                        new Decorator(new Actions(() => stateMachine.SwitchState(EnemyStates.Idle)))
                    }),
                })
            });
        }

        protected override void SwitchToReset() {
            CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);
            stateMachine.SwitchState(EnemyStates.Resetting);
        }
        
        protected override void Update() {
            if (enemyRadar.CurrentHit == null) {
                _currentThreat = null;
            }
            else if (enemyRadar.CurrentHit.TryGetComponent<Projectile>(out var projectile)) {
                if (projectile.owner == Player.Instance) {
                    _currentThreat = projectile;
                }
            }

            if (troopGoon) {
                _distancePlayerToTower = Vector2.Distance(troopGoon.commander.transform.position,
                    Player.Instance.PlayerPos);
            } 
            
            base.Update();
        }
    }
}