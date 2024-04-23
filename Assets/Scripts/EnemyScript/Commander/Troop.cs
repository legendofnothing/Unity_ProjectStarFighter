using UnityEngine;

namespace EnemyScript.Commander {
    public class Troop : MonoBehaviour {
        public MonoBehaviour attackState;

        private void Start() {
            attackState.enabled = false;
        }
    }
}