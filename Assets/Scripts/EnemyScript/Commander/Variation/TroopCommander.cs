using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace EnemyScript.Commander.Variation {
    public abstract class TroopCommander : Troop {
        [ReadOnly] public List<Enemy> troops = new();
        public int troopCount => troops.Count;
    }
}