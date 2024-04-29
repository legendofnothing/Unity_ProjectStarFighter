using EnemyScript.Easy.EnemyShoot.States;
using EnemyScript.Medium.MediumEnemyTroop.States;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyTroop {
    public class EnemyTroopStateMachine : StateMachine<EnemyTroopStateMachine.EnemyState> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;
        
        public enum EnemyState {
            Idle,
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new EnemyTroopIdle(EnemyState.Idle, this);
            return EnemyState.Idle;
        }

        protected override void SetupRef() {
        }
    }
}
