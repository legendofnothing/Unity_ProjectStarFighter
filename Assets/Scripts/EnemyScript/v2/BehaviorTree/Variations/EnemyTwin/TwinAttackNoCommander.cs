using System.Collections.Generic;
using BehaviorTree;
using Combat;
using EnemyScript.v2.BehaviorTree.Variations.EnemyShooter;
using EnemyScript.v2.StateMachine;
using PlayerScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.v2.BehaviorTree.Variations.EnemyTwin {
    public class TwinAttackNoCommander : AttackBehaviorMedium {
        [TitleGroup("Twin Attack No Commander Config")]
        public float chanceToAccelReset = 0.4f;
        public float suicidalTendencies = 0.09f;
        
        [TitleGroup("Refs")] 
        public EnemyRadar enemyRadar;
        
        private Projectile _currentThreat;
        
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
                                        
                                        if (stateMachine.enemy.GetDistanceToTarget(pos) >= 1.8f) return;
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
                        new Decorator(new Condition(() => stateMachine.CurrentState is EnemyStates.Resetting or EnemyStates.ResettingAccel)),
                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= CurrentResetDistance)),
                        new Decorator(new Actions(() => stateMachine.SwitchState(EnemyStates.Idle)))
                    }),
                })
            });
        }

        protected override void SwitchToReset() {
            if (Random.Range(0f, 1f) <= suicidalTendencies) {
                switch (stateMachine.CurrentState) {
                    case EnemyStates.Strafing:
                        stateMachine.SwitchState(EnemyStates.Strafing);
                        break;
                    case EnemyStates.Circling:
                        CurrentResetDistance = 2f;
                        stateMachine.SwitchState(EnemyStates.ResettingAccel);
                        break;
                }
            }
            else {
                CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);
                stateMachine.SwitchState(Random.Range(0f, 1.01f) < chanceToAccelReset ? EnemyStates.Resetting : EnemyStates.ResettingAccel);
            }
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