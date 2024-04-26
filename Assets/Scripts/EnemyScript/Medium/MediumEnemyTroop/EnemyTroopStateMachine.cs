using EnemyScript.Easy.EnemyShoot.States;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyTroop {
    public class EnemyTroopStateMachine : StateMachine<EnemyTroopStateMachine.EnemyState> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;
        
        public enum EnemyState {
        }
            
        protected override EnemyState SetupState() {
            return 0;
        }

        protected override void SetupRef() {
        }
    }
}
