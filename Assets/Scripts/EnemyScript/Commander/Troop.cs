using EnemyScript.Commander.Variation;
using UnityEngine;

namespace EnemyScript.Commander {
    public struct TroopCommand {
        public TroopCommander.Commands command;
        public Enemy commander;
    }
    
    public abstract class Troop : MonoBehaviour {
        public enum Commands {
            LookForTroop,
            Attack,
        }

        public MonoBehaviour attackState;
        public MonoBehaviour commandState;
        protected Enemy commander;

        private void Start() {
            attackState.enabled = false;
            commandState.enabled = false;
            OnStart();
        }

        protected abstract void OnStart();
    }
}