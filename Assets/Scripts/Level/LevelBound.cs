using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Level {
    public class LevelBound : MonoBehaviour {
        public SpriteRenderer targetRenderer;
        public float offset;
        public string name = "Collider";
        public bool isTrigger = false;

        private EdgeCollider2D _col;
        
        public void GenerateBounds(Action callback = null) {
            var corners = GetSpriteCorners(targetRenderer, offset);
            if (!_col) {
                var inst = new GameObject(name, typeof(EdgeCollider2D));
                inst.transform.SetParent(gameObject.transform);
                _col = inst.GetComponent<EdgeCollider2D>();
            }
            
            _col.gameObject.name = name;
            _col.isTrigger = isTrigger;
            
            var list = new List<Vector2> {
                corners[0],
                corners[1],
                corners[1],
                corners[2],
                corners[2],
                corners[3],
                corners[3],
                corners[0]
            };
            
            _col.SetPoints(list);
            callback?.Invoke();
        }

        private static Vector3[] GetSpriteCorners(SpriteRenderer renderer, float offset = 0) {
            var offsetVector = new Vector3(1, 1) * offset;
            return new[] {
                //Top Right Corner
                renderer.transform.TransformPoint(renderer.sprite.bounds.max - offsetVector), 
                //Bottom Right Corner
                renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.max.x - offset, renderer.sprite.bounds.min.y + offset, 0)), 
                //Bottom Right Corner
                renderer.transform.TransformPoint(renderer.sprite.bounds.min + offsetVector), 
                //Bottom Left Corner
                renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x + offset, renderer.sprite.bounds.max.y - offset, 0))
            };
        }
    }
}
