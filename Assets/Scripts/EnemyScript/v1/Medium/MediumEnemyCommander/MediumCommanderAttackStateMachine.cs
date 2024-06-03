using EnemyScript.v1.Medium.MediumEnemyCommander.States.Attacking;
using EnemyScript.v1.Medium.Troop;
using PlayerScript;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.v1.Medium.MediumEnemyCommander {
    public class MediumCommanderAttackStateMachine : StateMachine<MediumCommanderAttackStateMachine.EnemyState> { 
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;
        public MediumCommander commander;

        [TitleGroup("Strafe Settings")] 
        public float minimumDistance;
        public float predictedFrames = 1;
        
        [TitleGroup("Resetting Settings")] 
        public float minimumSafeDistance;
        public float maximumSafeDistance;
        [Space] 
        //Accel to escape player chasing
        public float timeUntilAccelerate = 6f;

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
            return EnemyState.Circling;
        }

        protected override void SetupRef() {
     
        }
        
        public Vector2 PredictPlayerPosition(float framePredicted = 1) {
            return framePredicted <= 0 
                ? Player.Instance.PlayerPos 
                : Player.Instance.PlayerPos + Player.Instance.PlayerDir * (Time.fixedDeltaTime * framePredicted);
        }
    }
}
