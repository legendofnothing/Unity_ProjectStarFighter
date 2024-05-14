using System.Collections.Generic;
using System.Linq;
using Core.Events;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Minimap {
    public class MinimapChangeSize : MonoBehaviour {
        public List<float> sizes = new();
        public Camera miniMapCamera;

        private int _currentIndex;

        private void Start() {
            ChangeCameraSize(0);
        }

        private void ChangeCameraSize(int index) {
            _currentIndex = index;
            miniMapCamera.orthographicSize = sizes[_currentIndex];
            this.FireEvent(EventType.OnMinimapSizeChange, $"[{_currentIndex + 1}]");
            this.FireEvent(EventType.ChangeMinimapIconSize, _currentIndex);
        }

        public void GoNextSize() {
            _currentIndex++;
            if (_currentIndex >= sizes.Count) _currentIndex = 0;
            ChangeCameraSize(_currentIndex);
        }
    }
}