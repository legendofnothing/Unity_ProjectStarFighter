using Core.Events;
using DG.Tweening;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace EnemyScript.Commander {
    public abstract class Troop : MonoBehaviour {
        public MonoBehaviour attackState;
        public MonoBehaviour underCommandState;
        protected Enemy commander;

        private void Start() {
            attackState.enabled = false;
            underCommandState.enabled = false;
            
            this.AddListener(EventType.SendCommand,param => RespondToCall((TroopCommand) param));
            OnStart();
        }

        protected abstract void OnStart();
        protected abstract void RespondToCall(TroopCommand command);
    }
}