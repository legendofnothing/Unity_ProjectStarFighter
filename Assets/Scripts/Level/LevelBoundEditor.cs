#if UNITY_EDITOR

using System;
using Core.Logging;
using UnityEditor;
using UnityEngine;

namespace Level {
    [CustomEditor(typeof(LevelBound))]
    public class LevelBoundEditor : Editor {
        public override void OnInspectorGUI() {
            var script = (LevelBound)target;
            DrawDefaultInspector();
            if (GUILayout.Button("Generate Bounds")) {
                try {
                    script.GenerateBounds(() => { NCLogger.Log("Generated bounds"); });
                }
                catch (Exception e) {
                    NCLogger.Log(e, LogLevel.ERROR);
                }
            }
        }
    }
}

#endif