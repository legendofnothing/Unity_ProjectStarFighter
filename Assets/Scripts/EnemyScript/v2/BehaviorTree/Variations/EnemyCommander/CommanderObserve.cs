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

namespace EnemyScript.v2.BehaviorTree.Variations.EnemyCommander {
    public class CommanderObserve : DefaultAttackBehavior {

        [TitleGroup("Refs")] 
        public float minTimeBeingChased = 1f;
        public float minThreatDistance = 3f;
        public EnemyRadar enemyRadar;
        
        protected float lastHP;
        private Projectile _currentThreat;
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
                            stateMachine.SwitchState(EnemyStates.Observing);
                        }))
                    }),
                        
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Observing)),
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
                                        
                                        SwitchToReset();
                                    }
                                })),
                            }),
                            
                            new Sequence(new List<Node> {
                                new Decorator(new Condition(() => lastHP > stateMachine.enemy.currentHp)),
                                new Decorator(new Actions(SwitchToReset))
                            })
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
    }
}