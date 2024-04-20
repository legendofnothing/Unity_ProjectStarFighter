using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree {
    public class Blackboard {
        private Dictionary<string, object> _data = new();

        public Blackboard(List<string> keys) {
            foreach (var key in keys) {
                _data[key] = new object();
            }
        }

        public object GetData(string key) {
            return _data.TryGetValue(key, out var value) ? value : null;
        }

        public bool SetData(string key, object value) {
            var status = false;
            if (_data.ContainsKey(key)) {
                _data[key] = value;
                status = true;
            }
            return status;
        }
    }
}