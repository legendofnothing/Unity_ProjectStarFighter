using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO {
    [Serializable]
    public struct DialoguesSettings {
        public string text;
        public float readingTime;
    }
    
    [Serializable]
    public struct DialoguesText {
        public Color handleColor;
        public string handleName;
        public List<DialoguesSettings> main;
    }
    
    [CreateAssetMenu(fileName = "Dialogues", menuName = "Dialogues/Dialogue", order = 2)]
    public class Dialogues : ScriptableObject {
        public DialoguesText dialogues;
    }
}