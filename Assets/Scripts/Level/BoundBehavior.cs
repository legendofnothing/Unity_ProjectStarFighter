using Core.Events;
using Scripts.Core;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Level {
    public class BoundBehavior : MonoBehaviour {
        public enum Type {
            OutOfBound,
            LimitBound,
        }

        public Type type;
        public LayerMask playerLayer;
        private EdgeCollider2D _collider;

        private void Start() {
            _collider = GetComponent<EdgeCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            OnEnter(other);
        }

        private void OnTriggerExit2D(Collider2D other) {
            OnExit(other);
        }

        private void OnEnter(Collider2D other) {
            if (CheckLayerMask.IsInLayerMask(other.gameObject, playerLayer)) {
                this.FireEvent(EventType.OnPlayerOutOfBound, true);
            }
        }

        private void OnExit(Collider2D other) {
            if (CheckLayerMask.IsInLayerMask(other.gameObject, playerLayer)) {
                var pos = other.gameObject.transform.position;
                if (pos.x <= _collider.bounds.extents.x && pos.x >= -_collider.bounds.extents.x &&
                    pos.y <= _collider.bounds.extents.y && pos.y >= -_collider.bounds.extents.y) {
                    this.FireEvent(EventType.OnPlayerOutOfBound, false);
                } 
            }
        }
    }
}
