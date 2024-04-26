using Core.Events;

namespace EnemyScript.Commander.Variation {
    public abstract class TroopGoon : Troop {
        protected override void OnStart() {
            this.AddListener(EventType.SendCommand,param => RespondToCall((TroopCommand) param));
        }
        
        protected abstract void RespondToCall(TroopCommand command);
    }
}