using EnemyScript.Easy.EnemyShoot.States;
using EnemyScript.Medium.MediumEnemyCommander.States;
using PlayerScript;
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
        [Space] 
        //Accel to escape player chasing
        public float timeUntilAccelerate = 6f;
        public float accelerateTime = 10f;
        public float deAccelerateTime = 3.5f;

        public enum EnemyState {
            Idle,
            Strafing,
            Resetting,
            Circling,
        }
            
        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new MediumCommanderIdle(EnemyState.Idle, this);
            states[EnemyState.Strafing] = new MediumCommanderStrafe(EnemyState.Strafing, this);
            states[EnemyState.Resetting] = new MediumCommanderReset(EnemyState.Resetting, this);
            states[EnemyState.Circling] = new MediumCommanderCircling(EnemyState.Circling, this);
            return EnemyState.Strafing;
        }

        protected override void SetupRef() {
            enemy = GetComponentInParent<Enemy>();
            enemyBehaviors = GetComponentInParent<EnemyBehaviors>();
        }
        
        public Vector2 PredictPlayerPosition(float framePredicted = 1) {
            return framePredicted <= 0 
                ? Player.Instance.PlayerPos 
                : Player.Instance.PlayerPos + Player.Instance.PlayerDir * (Time.fixedDeltaTime * framePredicted);
        }
    }
}
