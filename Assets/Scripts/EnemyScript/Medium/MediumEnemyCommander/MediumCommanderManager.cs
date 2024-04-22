using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander {
    public class MediumCommanderManager : MonoBehaviour {
        [TitleGroup("Refs")] 
        public MediumCommanderAttackStateMachine esm;
        
        [TitleGroup("Config")] 
        public List<GameObject> troopToCommand = new();
        
        private void Start() {
            
        }
    }
}