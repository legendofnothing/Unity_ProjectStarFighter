using System.Collections.Generic;
using Core.Events;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Minimap {
    public class MinimapIcon : MonoBehaviour {
        public List<float> iconSizes = new();

        private void Awake() {
            transform.localScale = new Vector3(1, 1) * iconSizes[0];
            this.AddListener(EventType.ChangeMinimapIconSize, param => ChangeSize((int) param));
        }

        private void Start() {
            transform.localScale = new Vector3(1, 1) * iconSizes[0];
        }

        private void ChangeSize(int param) {
            transform.localScale = new Vector3(1, 1) * iconSizes[param];
        }

        private void OnDestroy() {
            this.RemoveListener(EventType.ChangeMinimapIconSize);
        }
    }
}