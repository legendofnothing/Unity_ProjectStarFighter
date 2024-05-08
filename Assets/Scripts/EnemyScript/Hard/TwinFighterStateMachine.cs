using EnemyScript.Hard.States;
using PlayerScript;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Hard {
    public class TwinFighterStateMachine : StateMachine<TwinFighterStateMachine.EnemyState> {
        [ReadOnly] public Enemy enemy;
        [ReadOnly] public EnemyBehaviors enemyBehaviors;

        [TitleGroup("Strafe Settings")] 
        public float minimumDistance;
        public float predictedFrames = 1;
        
        [TitleGroup("Resetting Settings")] 
        public float minimumSafeDistance;
        public float maximumSafeDistance;
        
        [TitleGroup("Retreating Config")] 
        public float safeRetreatingDistance = 20f;

        [TitleGroup("Idle Config")] 
        public float distanceInView = 10f;

        [ReadOnly] public bool overrideResettingState;
        

        public enum EnemyState {
            Idle,
            Attack,
            Resetting,
            Retreating,
        }

        protected override EnemyState SetupState() {
            states[EnemyState.Idle] = new TwinFighterIdle(EnemyState.Idle, this);
            states[EnemyState.Attack] = new TwinFighterAttack(EnemyState.Attack, this);
            states[EnemyState.Resetting] = new TwinFighterReset(EnemyState.Resetting, this);
            states[EnemyState.Retreating] = new TwinFighterRetreat(EnemyState.Retreating, this);
            
            return EnemyState.Attack;
        }

        protected override void SetupRef() {
            enemy = GetComponent<Enemy>();
            enemyBehaviors = GetComponent<EnemyBehaviors>();
        }
        
        public Vector2 PredictPlayerPosition(float framePredicted = 1) {
            return framePredicted <= 0 
                ? Player.Instance.PlayerPos 
                : Player.Instance.PlayerPos + Player.Instance.PlayerDir * (Time.fixedDeltaTime * framePredicted);
        }
    }
}