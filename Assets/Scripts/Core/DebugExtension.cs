﻿using UnityEngine;

namespace Core {
    public static class DebugExtension {
        public static void DrawString(string text, Vector3 worldPosition, Color textColor, Vector2 anchor, float textSize = 15f) {
#if UNITY_EDITOR
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            if (!view)
                return;
            var screenPosition = view.camera.WorldToScreenPoint(worldPosition);
            if (screenPosition.y < 0 || screenPosition.y > view.camera.pixelHeight || screenPosition.x < 0 || screenPosition.x > view.camera.pixelWidth || screenPosition.z < 0) return;
            var pixelRatio = UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.right).x - UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.zero).x;
            UnityEditor.Handles.BeginGUI();
            var style = new GUIStyle(GUI.skin.label) {
                fontSize = (int)textSize,
                normal = new GUIStyleState{ textColor = textColor }
            };
            Vector2 size = style.CalcSize(new GUIContent(text)) * pixelRatio;
            var alignedPosition =
                ((Vector2)screenPosition +
                 size * ((anchor + Vector2.left + Vector2.up) / 2f)) * (Vector2.right + Vector2.down) +
                Vector2.up * view.camera.pixelHeight;
            GUI.Label(new Rect(alignedPosition / pixelRatio, size / pixelRatio), text, style);
            UnityEditor.Handles.EndGUI();
#endif
        }
    }
}