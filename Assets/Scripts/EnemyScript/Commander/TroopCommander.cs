using System.Collections.Generic;
using Core.Events;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace EnemyScript.Commander {
    public struct TroopCommand {
        public TroopCommander.Commands command;
        public Enemy commander;
    }
    
    public class TroopCommander: MonoBehaviour {
        public enum Commands {
            LookForTroop,
            Attack,
        }
        
        [ReadOnly] public List<Enemy> troops = new();
        public int troopCount => troops.Count;
        private Enemy _commander;

        private void Start() {
            _commander = GetComponent<Enemy>();
            DOVirtual.DelayedCall(1.2f, () => {
                if (troopCount <= 0) {
                    this.FireEvent(EventType.SendCommand, new TroopCommand() {
                        command = Commands.LookForTroop,
                        commander = _commander
                    });
                }
            });
        }
    }
}