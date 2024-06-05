using System.Collections.Generic;
using BehaviorTree;
using EnemyScript.v2.StateMachine;
using PlayerScript;
using UnityEngine;

namespace EnemyScript.v2.BehaviorTree.Variations.EnemyShooter {
    public class AttackBehaviorMedium : DefaultAttackBehavior {
        protected float lastHP;
        
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
                        new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Resetting)),
                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= CurrentResetDistance)),
                        new Decorator(new Actions(() => stateMachine.SwitchState(EnemyStates.Idle)))
                    }),
                })
            });
        }
    }
}