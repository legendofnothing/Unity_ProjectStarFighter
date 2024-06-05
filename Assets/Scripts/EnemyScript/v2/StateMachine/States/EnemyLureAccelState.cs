using DG.Tweening;
using PlayerScript;
using StateMachine;

namespace EnemyScript.v2.StateMachine.States {
    public class EnemyLureAccelState : EnemyState {
        private Tween _accelTween;
        
        public EnemyLureAccelState(EnemyStates key, StateMachine<EnemyStates> stateMachine) : base(key, stateMachine) { }

        public override void OnEnter() {
            esm.enemy.currentSpeed = esm.enemy.minimumSpeed;
            esm.enemy.angularSpeed = esm.enemy.angularSpeed;
            
            var currSpeed = esm.enemy.currentSpeed;
            _accelTween = DOVirtual.Float(currSpeed, currSpeed + currSpeed * esm.lurePercentAccelAdded, esm.lureAccelTime, value => {
                esm.enemy.currentSpeed = value;
            });
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