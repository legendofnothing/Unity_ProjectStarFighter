using DG.Tweening;
using PlayerScript;
using StateMachine;

namespace EnemyScript.v2.StateMachine.States {
    public class EnemyLureState : EnemyState {
        private Tween _accelTween;
        
        public EnemyLureState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) { }

        public override void OnEnter() {
            esm.enemy.currentSpeed = esm.enemy.minimumSpeed;
            esm.enemy.angularSpeed = esm.enemy.angularSpeed;
        }

        public override void OnExit() {
        }

        public override void OnUpdate() {
            esm.enemyBehaviors.LookAt(Player.Instance.PlayerPos, esm.enemy.angularSpeed); 
            esm.enemyBehaviors.Fly(esm.enemy.currentSpeed, -esm.transform.up);

            var dot = esm.enemy.GetDotToPlayer;
            if (dot >= UniversalSetting.DotToShoot && esm.enemy.GetDistanceToTarget(Player.Instance.PlayerPos) <= esm.minEngageDistance) {
                esm.enemyBehaviors.FireWeapon();
            }
        }
    }
}