using EnemyScript.Easy.EnemyShoot.States;
using EnemyScript.Medium.MediumEnemyCommander.States;
using PlayerScript;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScript.Medium.MediumEnemyCommander {
    public class MediumCommanderCommandStateMachine : StateMachine<MediumCommanderCommandStateMachine.EnemyState> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;
        
        public enum EnemyState {
            Idle,
            Strafing,
            Resetting,
            Circling,
        }
        
        protected override EnemyState SetupState() {
            return 0;
        }

        protected override void SetupRef() {
            enemy = GetComponentInParent<Enemy>();
            enemyBehaviors = GetComponentInParent<EnemyBehaviors>();
        }
    }
}
