using Core;
using PlayerScript;
using Sirenix.OdinInspector;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Boss {
    public abstract class BossStateMachine : StateMachine<BossStateMachine.EnemyState> {
        [ReadOnly] public EnemyBehaviors enemyBehaviors;
        [ReadOnly] public Enemy enemy;
        [ReadOnly] public EnemyWeapon enemyWeapon;

        [TitleGroup("Strafe Settings")] 
        public float engagingDistance = 30f;
        public float minimumDistance;
        public float predictedFrames = 1;
        
        [TitleGroup("Resetting Settings")] 
        public float minimumSafeDistance;
        public float maximumSafeDistance;
        [Space] 
        //Accel to escape player chasing
        public float timeUntilAccelerate = 6f;

        private bool _started;

        public enum EnemyState {
            Idle,
            Strafe,
            Circle,
            Reset,
        }

        protected override void SetupRef() {
            enemyBehaviors = GetComponent<EnemyBehaviors>();
            enemy = GetComponent<Enemy>();
            enemyWeapon = GetComponent<EnemyWeapon>();
            _started = true;
        }

        public Vector2 PredictPlayerPosition(float framePredicted = 1) {
            return framePredicted <= 0 
                ? Player.Instance.PlayerPos 
                : Player.Instance.PlayerPos + Player.Instance.PlayerDir * (Time.fixedDeltaTime * framePredicted);
        }

        private void OnDrawGizmos() {
            if (_started) {
                DebugExtension.DrawString($"distance: {enemy.GetDistanceToPlayer}", transform.position + Vector3.one, Color.white, Vector2.zero);
            }
        }
    }
}