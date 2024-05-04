using UnityEngine;

namespace SO {
    [CreateAssetMenu(fileName = "Setting", menuName = "Level/Settings", order = 1)]
    public class LevelSetting : ScriptableObject {
        public float outOfBoundDuration;
    }
}