using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO {
    [Serializable]
    public struct Tips {
        public string text;
        public float readingTime;
    }
    
    [CreateAssetMenu(fileName = "Random", menuName = "Random/Tips", order = 2)]
    public class BunchOfTip : ScriptableObject {
        public List<Tips> tips = new();
    }
}