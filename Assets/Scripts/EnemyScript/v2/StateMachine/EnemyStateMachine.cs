using EnemyScript.v2.StateMachine.States;
using PlayerScript;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.v2.StateMachine {
    public enum EnemyStates {
        Idle,
        Strafing,
        Circling,
        Resetting,
        ResettingAccel,
        Luring,
        LuringAccel,
    }

    public static class UniversalSetting {
        public static float DotToShoot = 0.998f;
    }
    
    public class EnemyStateMachine : StateMachine<EnemyStates> {
        public EnemyBehaviors enemyBehaviors;
        public Enemy enemy;

        [TitleGroup("Strafe Config")] 
        public float minEngageDistance = 20f;

        [TitleGroup("Circling Config")] 
        public float minCirclingDistance = 8f;
        public float predictedOffset = 1.5f;

        [TitleGroup("Reset Accel Config")] 
        public float accelTime = 3f;
        public float percentAccelAdded = 0.6f;
        
        [TitleGroup("Lure Accel Config")] 
        public float lureAccelTime = 3f;
        public float lurePercentAccelAdded = 0.2f;

        protected override EnemyStates SetupState() {
            states[EnemyStates.Idle] = new EnemyIdleState(EnemyStates.Idle, this);
            states[EnemyStates.Strafing] = new EnemyStrafeState(EnemyStates.Strafing, this);
            states[EnemyStates.Circling] = new EnemyCircleState(EnemyStates.Circling, this);
            
            states[EnemyStates.Luring] = new EnemyLureState(EnemyStates.Luring, this);
            states[EnemyStates.LuringAccel] = new EnemyLureAccelState(EnemyStates.LuringAccel, this);
            
            states[EnemyStates.Resetting] = new EnemyResetState(EnemyStates.Resetting, this);
            states[EnemyStates.ResettingAccel] = new EnemyResetAccelState(EnemyStates.ResettingAccel, this);
            return EnemyStates.Idle;
        }

        protected override void SetupRef() {
            
        }

        public Vector2 GetPredictedPos() {
            if (PredictPosition
                .HasInterceptDirection(
                    Player.Instance.PlayerPos,
                    transform.position,
                    Player.Instance.PlayerDir,
                    enemyBehaviors.WeaponProjectile.setting.speed,
                    out var predictedPos)) {
                if (Random.Range(0, 2) < 1) {
                    predictedPos += Vector2.one * predictedOffset;
                }
                else {
                    predictedPos -= Vector2.one * predictedOffset;
                }
            }
            else {
                predictedPos = Player.Instance.transform.position;
            }

            return predictedPos;
        }
    }
}