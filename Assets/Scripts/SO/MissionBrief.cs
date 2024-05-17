using System.Collections.Generic;
using UnityEngine;

namespace SO {
    [CreateAssetMenu(fileName = "MissionBrief", menuName = "MissionBrief/Brief", order = 2)]
    public class MissionBrief : ScriptableObject {
        public string name;
        public TextAsset text;
    }
}