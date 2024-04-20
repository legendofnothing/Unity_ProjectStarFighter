using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehaviorTree {
    public abstract class Tree : MonoBehaviour {
        protected List<string> keys = new();
        private Node _rootNode;
        protected Blackboard blackboard;

        protected void Start() {
            _rootNode = SetupTree();
        }

        protected void Update() {
            _rootNode?.Update(); 
        }

        protected abstract Node SetupTree();
    }
}