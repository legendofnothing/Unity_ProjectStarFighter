using System.Collections.Generic;
using BehaviorTree;
using Combat;
using DG.Tweening;
using EnemyScript.v2.StateMachine;
using PlayerScript;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using Sequence = BehaviorTree.Sequence;

namespace EnemyScript.v2.BehaviorTree.Variations.EnemyShooter {
    public class AttackBehaviorAdvanced : DefaultAttackBehavior {
        [TitleGroup("IFrame Delay")] 
        public AnimationClip shieldEffectClip;
        public float shieldCooldownDuration = 5f;
        
        [TitleGroup("Refs")] 
        public EnemyRadar enemyRadar;
        public GameObject shieldEffect;
        
        protected float lastHP;
        private Projectile _currentThreat;
        private bool _shieldOnCooldown;

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
                        new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Strafing or EnemyStates.Circling)),
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
                                        
                                        if (stateMachine.enemy.GetDistanceToTarget(pos) >= 2) return;

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
                                        }
                                        
                                        SwitchToReset();
                                    }
                                })),
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
                                new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Circling)),
                                new Decorator(new Condition(() => lastHP > stateMachine.enemy.currentHp)),
                                new Decorator(new Actions(SwitchToReset))
                            }),
                        })
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Resetting)),
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
                if (projectile.owner == Player.Instance) {
                    _currentThreat = projectile;
                }
            } 
            
            base.Update();
        }
    }
}