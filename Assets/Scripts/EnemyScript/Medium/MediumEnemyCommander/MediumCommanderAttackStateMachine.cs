using EnemyScript.Easy.EnemyShoot.States;
using EnemyScript.Medium.MediumEnemyCommander.States;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Medium.MediumEnemyCommander {
    public class MediumCommanderAttackStateMachine : StateMachine<MediumCommanderAttackStateMachine.EnemyState> {
        [ReadOnly] public EnemyBehaviors enemyBehaviors;
        [ReadOnly] public Enemy enemy;

        [TitleGroup("Strafe Settings")] 
        public float minimumSafeDistance;

        public enum EnemyState {
            Idle,
            Strafing
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new MediumCommanderIdle(EnemyState.Idle, this);
            states[EnemyState.Strafing] = new MediumCommanderStrafe(EnemyState.Strafing, this);
            return EnemyState.Strafing;
        }

        protected override void SetupRef() {
            enemy = GetComponentInParent<Enemy>();
            enemyBehaviors = GetComponentInParent<EnemyBehaviors>();
        }
    }
}
