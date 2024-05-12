using System;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.Boss {
    public class Boss : MonoBehaviour {
        [ReadOnly] public EnemyBehaviors enemyBehaviors;
        [ReadOnly] public EnemyWeapon enemyWeapon;
        [ReadOnly] public Enemy enemy;

        private global::BehaviorTree.BehaviorTree _switchWeaponBt;
        
        private void Awake() {
            enemyBehaviors = GetComponent<EnemyBehaviors>();
            enemyWeapon = GetComponent<EnemyWeapon>();
            enemy = GetComponent<Enemy>();
            
            enemyWeapon.ChangeWeapon(0);
        }

        private void Start() {
            _switchWeaponBt = new global::BehaviorTree.BehaviorTree(new List<Node> {
                new Selector(new List<Node> {
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => enemy.GetDistanceToPlayer <= 5f)),
                        new Decorator(new Actions(() => enemyWeapon.ChangeWeapon(1)))
                    }),
                    
                    new Sequence(new List<Node> {
                        new Decorator(new Condition(() => enemy.GetDistanceToPlayer >= 5f)),
                        new Decorator(new Actions(() => enemyWeapon.ChangeWeapon(0)))
                    })
                })
            });
        }

        private void Update() {
            _switchWeaponBt.Update();
        }
    }
}
