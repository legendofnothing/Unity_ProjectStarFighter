using Core.Events;

namespace EnemyScript.Commander.Variation {
    public abstract class TroopGoon : Troop {
        protected override void OnAwake() {
            this.AddListener(EventType.SendCommand,param => RespondToCall((TroopCommand) param));
            if (!commander) {
                SwitchState(State.Attack);
            }
        }

        public override void OnDeath() {
            if (commander && commander.TryGetComponent<TroopCommander>(out var cmd)) {
                cmd.RemoveTroop(this);
            }
        }

        protected abstract void RespondToCall(TroopCommand command);

        private void OnDestroy() {
            this.RemoveListener(EventType.SendCommand);
        }
    }
}