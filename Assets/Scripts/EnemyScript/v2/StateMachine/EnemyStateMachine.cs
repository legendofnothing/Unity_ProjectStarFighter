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

        protected override EnemyStates SetupState() {
            states[EnemyStates.Idle] = new EnemyIdleState(EnemyStates.Idle, this);
            states[EnemyStates.Strafing] = new EnemyStrafeState(EnemyStates.Strafing, this);
            states[EnemyStates.Circling] = new EnemyCircleState(EnemyStates.Circling, this);
            states[EnemyStates.Resetting] = new EnemyResetState(EnemyStates.Resetting, this);
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