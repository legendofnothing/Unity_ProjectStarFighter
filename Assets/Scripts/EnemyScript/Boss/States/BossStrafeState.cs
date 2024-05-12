using PlayerScript;
using StateMachine;
using UnityEngine;

namespace EnemyScript.Boss.States {
    public class BossStrafeState : BossState {
        private float _lastHp;
        private int _hitTaken;
        private int _maximumHitTaken;
        
        public BossStrafeState(BossStateMachine.EnemyState key, StateMachine<BossStateMachine.EnemyState> stateMachine) : base(key, stateMachine) { }

        public override void OnEnter() {
            esm.enemy.currentAngularSpeed = esm.enemy.angularSpeed;
            esm.enemy.currentSpeed = esm.enemy.minimumSpeed;
            _lastHp = esm.enemy.currentHp;
            _hitTaken = 0;
            _maximumHitTaken = Random.Range(1, 4);
        }

        public override void OnExit() { }

        public override void OnUpdate() {
            var predictedFrames =
                Mathf.Lerp(
                    0,
                    esm.minimumDistance,
                    Mathf.Clamp(esm.enemy.GetDistanceToPlayer / esm.engagingDistance, 0, 1));

            Vector2 predictedPos;
            if (PredictPosition.HasInterceptDirection(
                    Player.Instance.PlayerPos, 
                    esm.transform.position, 
                    Player.Instance.PlayerDir, 
                    esm.enemyWeapon.currentWeapon.setting.speed, 
                    out var res)) {
                predictedPos = res + Player.Instance.PlayerDir * (Time.fixedDeltaTime * predictedFrames);
            }
            else {
                predictedPos = Player.Instance.PlayerPos;
            }
            
            //this for weapon
            var predictedDot = esm.enemy.GetDotToPoint(predictedPos);
            if (predictedDot > 0.99f && esm.enemy.GetDistanceToPlayer <= esm.engagingDistance) {
                esm.enemyWeapon.FireWeapon();
            }
            
            //this for mvoement;
            if (predictedDot > 0.97f && esm.enemy.GetDistanceToPlayer <= esm.minimumDistance) {
                esm.SwitchState(BossStateMachine.EnemyState.Reset);
            }
            else {
                esm.enemyBehaviors.LookAt(predictedPos, esm.enemy.currentAngularSpeed); 
            }
            
            var lerpedSpeed = Mathf.Lerp(esm.enemy.minimumSpeed, esm.enemy.speed - esm.enemy.minimumSpeed, Mathf.Clamp(predictedDot, 0f, 1f));
            esm.enemy.currentSpeed = lerpedSpeed;
            esm
                .enemyBehaviors.FlyForward(esm.enemy.currentSpeed); 
            
            if (_lastHp > esm.enemy.currentHp && _hitTaken >= _maximumHitTaken) {
                esm.SwitchState(BossStateMachine.EnemyState.Reset);
            }
            else {
                _hitTaken++;
                _lastHp = esm.enemy.currentHp;
            }
        }
    }
}