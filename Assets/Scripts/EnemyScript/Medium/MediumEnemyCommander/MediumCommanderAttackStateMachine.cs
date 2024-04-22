using EnemyScript.Easy.EnemyShoot.States;
using EnemyScript.Medium.MediumEnemyCommander.States;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScript.Medium.MediumEnemyCommander {
    public class MediumCommanderAttackStateMachine : StateMachine<MediumCommanderAttackStateMachine.EnemyState> {
        [ReadOnly] public EnemyBehaviors enemyBehaviors;
        [ReadOnly] public Enemy enemy;

        [TitleGroup("Strafe Settings")] 
        public float minimumDistance;
        public float predictedFrames = 1;
        
        [TitleGroup("Resetting Settings")] 
        public float minimumSafeDistance;
        public float maximumSafeDistance;

        public enum EnemyState {
            Idle,
            Strafing,
            Resetting,
            
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new MediumCommanderIdle(EnemyState.Idle, this);
            states[EnemyState.Strafing] = new MediumCommanderStrafe(EnemyState.Strafing, this);
            states[EnemyState.Resetting] = new MediumCommanderReset(EnemyState.Resetting, this);
            return EnemyState.Strafing;
        }

        protected override void SetupRef() {
            enemy = GetComponentInParent<Enemy>();
            enemyBehaviors = GetComponentInParent<EnemyBehaviors>();
        }
    }
}
