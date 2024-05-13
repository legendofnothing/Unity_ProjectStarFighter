using Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using EventType = Core.Events.EventType;

namespace UI.Components {
    
    public class TextUI : MonoBehaviour {
        public TextMeshProUGUI textMesh;
        public EventType eventType;

        private void Start() {
           this.AddListener(eventType, response => {
               switch (response) {
                   case string text:
                       textMesh.text = text;
                       break;
                   case float text:
                       textMesh.text = text.ToString("0");
                       break;
                   case int text:
                       textMesh.text = text.ToString("0");
                       break;
                   default:
                       Debug.LogError($"Invalid callback at {gameObject.name}, callback response is not a string!");
                       break;
               }
           });
        }
    }
}
