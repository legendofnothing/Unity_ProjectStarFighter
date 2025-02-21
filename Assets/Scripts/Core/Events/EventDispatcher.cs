using System;
using System.Collections.Generic;
using Core.Patterns;
using UnityEngine;

namespace Core.Events {
    public class EventDispatcher : Singleton<EventDispatcher> {
        public Dictionary<EventType, Action<object>> Events = new();
        private bool _isQuitting;
        
        /// <summary>
        /// Add a listener to an event type.
        /// </summary>
        /// <param name="eventType">Type of event.</param>
        /// <param name="callback">Action to trigger on event.</param>
        public void AddListener(EventType eventType, Action<object> callback) {
            if (Events.ContainsKey(eventType)) {
                Events[eventType] += callback;
            }
            else {
                Events.Add(eventType, callback);
            }
        }

        /// <summary>
        /// Remove a listener from subscribing to an event.
        /// </summary>
        /// <param name="eventType">Type of event</param>
        /// <param name="callback"></param>
        public void RemoveListener(EventType eventType) {
            if (Events.ContainsKey(eventType)) {
                Events[eventType] -= Events[eventType];
            }
        }

        /// <summary>
        /// Trigger an event and its callbacks with parameters.
        /// </summary>
        /// <param name="eventType">Event to trigger.</param>
        /// <param name="param">Paramaters</param>
        public void FireEvent(EventType eventType, object param = null) {
            if (Events.ContainsKey(eventType)) {
                var actions = Events[eventType];
                if (actions == null) {
                    Events.Remove(eventType);
                    return;
                }
                actions.Invoke(param);
            }
        }

        public void ClearListeners() {
            Events.Clear();
        }

        private void OnDestroy() {
            ClearListeners();
        }

        private void OnApplicationQuit() {
            _isQuitting = true;
        }
    }

    // WorldItem.cs : FireEvent(EventType.OnItemPickup, new object[] {ConsumableA, 2})

    // InventorySystem.cs :
    // AddListener(EventType.OnItemPickup, PickupCallback);
    // void PickupCallback(ItemType type, int itemCount)

    /// <summary>
    /// Extensions to help with calling event-related methods.
    /// </summary>
    public static class EventDispatcherExtension {
        public static void AddListener(this MonoBehaviour listener, EventType eventType, Action<object> callback) {
            EventDispatcher.Instance.AddListener(eventType, callback);
        }

        public static void RemoveListener(this MonoBehaviour listener, EventType eventType) {
            EventDispatcher.Instance.RemoveListener(eventType);
        }

        public static void FireEvent(this MonoBehaviour listener, EventType eventType, object param = null) {
            EventDispatcher.Instance.FireEvent(eventType, param);
        }
    }
}