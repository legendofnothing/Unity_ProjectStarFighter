using System;
using System.Collections.Generic;
using BehaviorTree;
using Combat;
using DG.Tweening;
using EnemyScript.v2.Commander;
using EnemyScript.v2.StateMachine;
using PlayerScript;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using Sequence = BehaviorTree.Sequence;

namespace EnemyScript.v2.BehaviorTree.Variations.EnemyCommander {
    public class CommanderAttack : DefaultAttackBehavior {
        [TitleGroup("IFrame Delay")] 
        public AnimationClip shieldEffectClip;
        public float shieldCooldownDuration = 5f;

        [TitleGroup("Threat Config")]
        public float minThreatDistance = 3f;
        public float minTimeBeingChased = 1f;
        
        [TitleGroup("Luring Config")]
        public float maximumDistanceToCancelLure = 8f;
        public float minDistanceToCancelLure = 4f;

        [TitleGroup("Commander Attack Specific Config")]
        public float chanceToReset = 0.4f;
        public float unpredictableValue = 0.2f;

        [TitleGroup("Refs")] 
        public CommanderV2 commander;
        public LayerMask selfMask;
        public EnemyRadar enemyRadar;
        public GameObject shieldEffect;
        
        protected float lastHP;
        private Projectile _currentThreat;
        private bool _shieldOnCooldown;
        private float _currentTimeBeingChased;

        protected override void SetupTree() {
            CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);
            lastHP = stateMachine.enemy.hp;
            
            MainTree = new global::BehaviorTree.BehaviorTree(new List<Node> {
                new Selector(new List<Node> {
                        
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Idle)),
                        new Decorator(new Actions(() => {
                            lastHP = stateMachine.enemy.currentHp;
                            stateMachine.SwitchState(Random.Range(0, 2) < 1
                                ? EnemyStates.Strafing
                                : EnemyStates.Circling);
                        }))
                    }),
                        
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Strafing or EnemyStates.Circling or EnemyStates.Luring or EnemyStates.LuringAccel)),
                        new Selector(new List<Node> {
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => _currentThreat)),
                                new Decorator(new Actions(() => {
                                    if (_currentThreat == null) return;
                                    
                                    var b =
                                        PredictPosition.HasInterceptDirection(
                                            _currentThreat.transform.position,
                                            transform.position,
                                            _currentThreat.Velocity, 
                                            rb.velocity.magnitude,
                                            out _);

                                    if (!b) return;
                                    
                                    var dir = (_currentThreat.transform.position - transform.position).normalized;
                                    var dot = Vector2.Dot(transform.up,dir);

                                    if (dot >= 0.9f || Physics2D.Raycast(_currentThreat.transform.position,
                                            _currentThreat.transform.up, enemyRadar.detectRadius + 10f, selfMask)) {
                                        if (Vector2.Distance(_currentThreat.transform.position,
                                                stateMachine.transform.position) >= minThreatDistance) {
                                            return;
                                        }
                                    }

                                    if (!_shieldOnCooldown) {
                                        _shieldOnCooldown = true;
                                        var inst = Instantiate(shieldEffect, transform.position, quaternion.identity);
                                        inst.transform.SetParent(gameObject.transform);
                                        inst.transform.localPosition = Vector3.zero;
                                        stateMachine.enemy.canDamage = false;

                                        DOVirtual.DelayedCall(shieldEffectClip.length, () => {
                                            stateMachine.enemy.canDamage = true;
                                        });

                                        DOVirtual.DelayedCall(shieldCooldownDuration, () => {
                                            _shieldOnCooldown = false;
                                        });
                                        
                                        SwitchToReset(true);
                                    }
                                        
                                    SwitchToReset();
                                })),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Strafing)),
                        
                                new Selector(new List<Node> {
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => stateMachine.enemy.GetDotToPoint(Player.Instance.PlayerPos) >= dotInFront)),
                                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) <= dangerDistance)),
                                        new Decorator(new Actions(SwitchToReset))
                                    }),
                            
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => lastHP > stateMachine.enemy.currentHp)),
                                        new Decorator(new Actions(SwitchToReset))
                                    })
                                }),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Circling)),
                                new Decorator(new Condition(() => lastHP > stateMachine.enemy.currentHp)),
                                new Decorator(new Actions(SwitchToReset))
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Luring)),
                                new Selector(new List<Node> {
                                    new Sequence(new List<Node> {
                                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToPlayer >= maximumDistanceToCancelLure || lastHP > stateMachine.enemy.currentHp)),
                                        new Decorator(new Actions(SwitchToReset))
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
                                new Decorator(new Actions(() => SwitchToReset(true)))
                            }),
                        })
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Resetting)),
                        new Selector(new List<Node> {
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) <= CurrentResetDistance)),
                                new Decorator(new Actions(() => stateMachine.SwitchState(EnemyStates.Idle)))
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Actions(() => _currentTimeBeingChased += Time.fixedDeltaTime)),
                                new Decorator(new Condition(() => _currentTimeBeingChased >= minTimeBeingChased)),
                                new Decorator(new Actions(() => {
                                    stateMachine.SwitchState(EnemyStates.ResettingAccel);
                                    _currentTimeBeingChased = 0;
                                }))
                            })
                        }),
                        
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= CurrentResetDistance)),
                        }),
                        new Decorator(new Actions(() => stateMachine.SwitchState(EnemyStates.Idle)))
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.ResettingAccel)),
                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= CurrentResetDistance)),
                        new Decorator(new Actions(() => stateMachine.SwitchState(EnemyStates.Idle)))
                    }),
                })
            });
        }
        
        protected override void Update() {
            if (enemyRadar.CurrentHit == null) {
                _currentThreat = null;
            }
            else if (enemyRadar.CurrentHit.TryGetComponent<Projectile>(out var projectile)) {
                if (projectile.owner == Player.Instance 
                    && Vector2.Distance(stateMachine.transform.position, projectile.transform.position) <= minThreatDistance) {
                    _currentThreat = projectile;
                }
            } 
            
            base.Update();
        }

        private void SwitchToReset(bool isOverride) {
            CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);
            stateMachine.SwitchState(EnemyStates.ResettingAccel);
            lastHP = stateMachine.enemy.currentHp;
        }

        protected override void SwitchToReset() {
            var ratio = stateMachine.enemy.currentHp / stateMachine.enemy.hp;
            if (Random.Range(0f, 1f) <= Mathf.Lerp(chanceToReset, 0, ratio)) {
                CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);
                stateMachine.SwitchState(Random.Range(0f, 1f) <= unpredictableValue ? EnemyStates.ResettingAccel : EnemyStates.Resetting);
            }
            else {
                if (commander.troopCount <= 0) {
                    switch (stateMachine.CurrentState) {
                        case EnemyStates.Strafing:
                            stateMachine.SwitchState(EnemyStates.Circling);
                            break;
                        case EnemyStates.Circling:
                            CurrentResetDistance = minResetDistance;
                            stateMachine.SwitchState(Random.Range(0f, 1f) <= unpredictableValue ? EnemyStates.ResettingAccel : EnemyStates.Resetting);
                            break;
                    }
                }
                else {
                    switch (stateMachine.CurrentState) {
                        case EnemyStates.Strafing or EnemyStates.Circling:
                            stateMachine.SwitchState(EnemyStates.Luring);
                            break;
                        default:
                            CurrentResetDistance = minResetDistance;
                            stateMachine.SwitchState(Random.Range(0f, 1f) <= unpredictableValue ? EnemyStates.ResettingAccel : EnemyStates.Resetting);
                            break;
                    }
                }
            }
            
            CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);
            lastHP = stateMachine.enemy.currentHp;
        }
    }
}