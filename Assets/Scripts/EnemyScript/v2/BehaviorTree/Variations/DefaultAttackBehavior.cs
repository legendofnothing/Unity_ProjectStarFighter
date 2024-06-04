using System.Collections.Generic;
using BehaviorTree;
using EnemyScript.v2.StateMachine;
using PlayerScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.v2.BehaviorTree.Variations {
    public class DefaultAttackBehavior : DefaultBehavior {
        [TitleGroup("Attack Config")] 
        [Range(0, 1)]
        public float dotInFront = 0.998f;
        public float dangerDistance = 4f;

        [TitleGroup("Resetting Config")] 
        public float minResetDistance = 4f;
        public float maxResetDistance = 8f;

        protected float CurrentResetDistance;
        
        protected override void SetupTree() {
            CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);

            MainTree = new global::BehaviorTree.BehaviorTree(new List<Node> {
                new Selector(new List<Node> {
                        
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Idle)),
                        new Decorator(new Actions(() => stateMachine.SwitchState(EnemyStates.Strafing)))
                    }),
                        
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Strafing)),
                        new Decorator(new Condition(() => stateMachine.enemy.GetDotToPoint(Player.Instance.PlayerPos) >= dotInFront)),
                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= dangerDistance)),
                        new Decorator(new Actions(SwitchToReset))
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => stateMachine.CurrentState == EnemyStates.Resetting )),
                        new Decorator(new Condition(() => stateMachine.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) >= CurrentResetDistance)),
                        new Decorator(new Actions(() => stateMachine.SwitchState(EnemyStates.Idle)))
                    }),
                })
            });
        }
        
        protected virtual void SwitchToReset() {
            CurrentResetDistance = Random.Range(minResetDistance, maxResetDistance);
            stateMachine.SwitchState(EnemyStates.Resetting);
        }
    }
}