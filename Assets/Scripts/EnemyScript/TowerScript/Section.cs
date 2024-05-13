using System.Collections.Generic;
using DG.Tweening;
using Effect;
using UnityEngine;

namespace EnemyScript.TowerScript {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Section : MonoBehaviour {
        public List<SpriteRenderer> renderers = new();
        public BoxCollider2D collider;
        public Rigidbody2D rigidbody;

        [Space] 
        public GameObject explosionEffect;
        public float size;
        

        private void Start() {
            collider.enabled = false;
            rigidbody.simulated = false;
        }

        public void OnDeath() {
            var explosionInst = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            explosionInst.GetComponent<EffectBase>().Init(Vector3.one * size);
            gameObject.transform.parent = null;
            collider.enabled = true;
            rigidbody.simulated = true;
            rigidbody.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);
            rigidbody.AddForce(transform.up * 4f, ForceMode2D.Impulse);
            foreach (var renderer in renderers) {
                renderer.DOColor(new Color(0.4f, 0.4f, 0.4f), 0.5f);
                renderer.sortingOrder = -1;
            }
        }
    }
}
