using EnemyScript.v2.StateMachine;
using PlayerScript;
using UnityEngine;

namespace EnemyScript.v2.BehaviorTree {
    public abstract class DefaultBehavior : MonoBehaviour {
        public EnemyStateMachine stateMachine;
        protected global::BehaviorTree.BehaviorTree MainTree;

        protected abstract void SetupTree();

        protected void Awake() {
            SetupTree();
        }

        protected void Update() {
            if (MainTree.children.Count <= 0) return;
            MainTree.Update();
        }
    }
}