using UnityEngine;

namespace SO {
    [CreateAssetMenu(fileName = "Cursors", menuName = "Cursors/Cursor", order = 5)]
    public class Cursors : ScriptableObject {
        public Texture2D cursorCombat;
        public Texture2D cursorNormal;
    }
}