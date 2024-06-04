using EnemyScript.v2.StateMachine;
using PlayerScript;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.v2.BehaviorTree {
    public abstract class DefaultBehavior : MonoBehaviour {
        public EnemyStateMachine stateMachine;
        [ReadOnly] public bool canRun = true;
        public global::BehaviorTree.BehaviorTree MainTree;
        protected Rigidbody2D rb;

        protected abstract void SetupTree();

        protected void Awake() {
            rb = GetComponent<Rigidbody2D>();
            SetupTree();
        }

        protected virtual void Update() {
            if (MainTree.children.Count <= 0) return;
            if (!canRun) return;
            MainTree.Update();
        }

        public void ResetTree() {
            MainTree.Reset();
        }
    }
}