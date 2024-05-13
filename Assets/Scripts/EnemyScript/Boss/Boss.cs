using System;
using System.Collections.Generic;
using BehaviorTree;
using Combat;
using Core.Events;
using DG.Tweening;
using EnemyScript.Boss.StateMachines;
using PlayerScript;
using Sirenix.OdinInspector;
using UnityEngine;
using EventType = Core.Events.EventType;
using Random = UnityEngine.Random;
using Sequence = BehaviorTree.Sequence;

namespace EnemyScript.Boss {
    public class Boss : MonoBehaviour {
        [ReadOnly] public EnemyBehaviors enemyBehaviors;
        [ReadOnly] public EnemyWeapon enemyWeapon;
        [ReadOnly] public Enemy enemy;
        [ReadOnly] public EnemyRadar enemyRadar;
        [ReadOnly] public Rigidbody2D rb;

        [TitleGroup("State Machine refs")]
        public SpriteRenderer sprite;
        public PolygonCollider2D collider;
        public LayerMask selfMask;
        public BossMainStateMachine stateMachine;
        public bool notFacade;
        [ShowIf(nameof(notFacade))] public GameObject facade;
        [ShowIf(nameof(notFacade))] public List<SpriteRenderer> spritesToTurnOff = new();

        private global::BehaviorTree.BehaviorTree _switchWeaponBt;
        private global::BehaviorTree.BehaviorTree _mainBehavior;
        private Projectile _currentThreat;
        private int facadeCount;
        private bool _canSpawnFacades = true;

        private void Awake() {
            enemyBehaviors = GetComponent<EnemyBehaviors>();
            enemyWeapon = GetComponent<EnemyWeapon>();
            enemy = GetComponent<Enemy>();
            enemyRadar = GetComponent<EnemyRadar>();
            rb = GetComponent<Rigidbody2D>();
            
            enemyWeapon.ChangeWeapon(0);
            if (notFacade) {
                this.AddListener(EventType.OnFacadeDestroyed, param => {
                    if (facadeCount > 0) facadeCount--;
                });
            }
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

            if (notFacade) {
                _mainBehavior = new global::BehaviorTree.BehaviorTree(new List<Node> {
                    new Selector(new List<Node> {
                        new Sequence(new List<Node> {
                            new Decorator(new Condition(() => _currentThreat != null)),
                            new Decorator(new Condition(() => facadeCount <= 0)),
                            new Decorator(new Condition(() => _canSpawnFacades)),
                            new Decorator(new Actions(() => {
                                if (_currentThreat == null) return;
                                var b =
                                    PredictPosition.HasInterceptDirection(
                                        _currentThreat.transform.position,
                                        transform.position,
                                        _currentThreat.Velocity, 
                                        rb.velocity.magnitude,
                                        out var res);
                              
                                if (b) {
                                    var dir = (_currentThreat.transform.position - transform.position).normalized;
                                    var dot = Vector2.Dot(transform.up,dir);
                                    if (dot >= 0.98f &&  Physics2D.Raycast(_currentThreat.transform.position, _currentThreat.transform.up, enemyRadar.detectRadius + 10f, selfMask)) {
                                        stateMachine.enabled = false;
                                        collider.enabled = false;
                                        enemy.stopUpdatingUI = true;

                                        var list = new List<Vector3> {
                                            Player.Instance.PlayerPos + new Vector2(0, 5),
                                            Player.Instance.PlayerPos + new Vector2(5, 0),
                                            Player.Instance.PlayerPos + new Vector2(-5, 0),
                                        };
                                        
                                        sprite.DOColor(Color.clear, 0.5f).OnComplete(() => {
                                            var index = Random.Range(0, list.Count);
                                            list.RemoveAt(index);
                                            transform.position = list[index];
                                        });

                                        foreach (var s in spritesToTurnOff) {
                                            s.DOColor(Color.clear, 0.5f);
                                            s.enabled = false;
                                        }

                                        List<Boss> bosses = new();

                                        for (var i = 0; i < 2; i++) {
                                            facadeCount++;
                                            var inst = Instantiate(facade, list[i], transform.rotation);
                                            var boss = inst.GetComponent<Boss>();
                                            bosses.Add(boss);
                                            boss.sprite.color = Color.clear;
                                            boss.stateMachine.enabled = false;
                                            boss.collider.enabled = false;
                                            boss.enemy.Ui.canvasGroup.alpha = 0;
                                            boss.enemy.stopUpdatingUI = true;
                                        }

                                        DOVirtual.DelayedCall(2f, () => {
                                            sprite.DOColor(Color.white, 0.2f);
                                            stateMachine.enabled = true;
                                            collider.enabled = true;
                                            enemy.stopUpdatingUI = false;
                                            
                                            foreach (var s in spritesToTurnOff) {
                                                s.enabled = true;
                                            }

                                            foreach (var boss in bosses) {
                                                boss.sprite.DOColor(Color.white, 0.2f);
                                                boss.stateMachine.enabled = true;
                                                boss.collider.enabled = true;
                                                boss.enemy.stopUpdatingUI = false;
                                            }
                                        });
                                    }
                                }
                            }))
                        }),
                    })
                });
            }
        }

        private void Update() {
            _switchWeaponBt.Update();
            if (!notFacade) return;
            _mainBehavior.Update();

            if (enemyRadar.CurrentHit == null) {
                _currentThreat = null;
            }
            else if (enemyRadar.CurrentHit.TryGetComponent<Projectile>(out var projectile)) {
                if (projectile.owner == Player.Instance) {
                    _currentThreat = projectile;
                }
            }
        }

        private void OnDestroy() {
            if (!notFacade) {
                this.FireEvent(EventType.OnFacadeDestroyed);
            }
        }
    }
}
