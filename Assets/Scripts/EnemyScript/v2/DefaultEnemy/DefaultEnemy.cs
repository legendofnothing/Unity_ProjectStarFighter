using System.Collections.Generic;
using BehaviorTree;
using EnemyScript.Commander;
using EnemyScript.Commander.Variation;
using EnemyScript.v2.BehaviorTree.Variations;
using EnemyScript.v2.StateMachine;
using Sirenix.OdinInspector;

namespace EnemyScript.v2.DefaultEnemy {
    public class DefaultEnemy : TroopGoon {
        private Commands _currentCommand = Commands.None;
        private global::BehaviorTree.BehaviorTree _mainTree;

        [TitleGroup("Refs")] 
        public DefaultAttackBehavior attackBehavior;
        public DefaultAttackBehavior observeBehavior;
        
        protected override void OnStart() {
            if (!commander) {
                attackBehavior.canRun = true;
                if (observeBehavior) observeBehavior.canRun = false;
            }
            else {
                attackBehavior.canRun = false;
                observeBehavior.canRun = false;

                _mainTree = new global::BehaviorTree.BehaviorTree(new List<Node> {
                    new Selector(new List<Node> {
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => _currentCommand == Commands.Command)),
                            new Decorator(new Condition(() => !observeBehavior.canRun)),
                            new Decorator(new Actions(() => {
                                attackBehavior.canRun = false;
                                
                                observeBehavior.MainTree.Reset();
                                observeBehavior.stateMachine.SwitchState(EnemyStates.Idle);
                                observeBehavior.canRun = true;
                            }))
                        }),
                        
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => _currentCommand == Commands.Attack)),
                            new Decorator(new Condition(() => !attackBehavior.canRun)),
                            new Decorator(new Actions(() => {
                                observeBehavior.canRun = false;
                                
                                attackBehavior.MainTree.Reset();
                                attackBehavior.stateMachine.SwitchState(EnemyStates.Idle);
                                attackBehavior.canRun = true;
                            }))
                        })
                    })
                });
            }
        }

        private void Update() {
            if (commander) {
                _mainTree.Update();
            }
        }

        public override void OnDamage() { }

        protected override void RespondToCall(TroopCommand command) {
            _currentCommand = command.command;

            if (command.command != Commands.CommanderDead) return;
            
            observeBehavior.canRun = false;
            attackBehavior.MainTree.Reset();
            attackBehavior.stateMachine.SwitchState(EnemyStates.Strafing);
            attackBehavior.canRun = true;
        }
    }
}