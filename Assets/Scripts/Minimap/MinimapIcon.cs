using System.Collections.Generic;
using System.Threading;
using Core.Events;
using DG.Tweening;
using UnityEngine;
using EventType = Core.Events.EventType;


namespace Minimap {
    public class MinimapIcon : MonoBehaviour {
        public List<float> iconSizes = new();
        public SpriteRenderer dotInner;
        public SpriteRenderer dotOuter;
        public float radarInterval;
        public bool ignoreUpdate;

        private Sequence _radarSequence;

        private void Awake() {
            transform.localScale = new Vector3(1, 1) * iconSizes[0];
            this.AddListener(EventType.ChangeMinimapIconSize, param => ChangeSize((int) param));
        }

        private void Start() {
            transform.localScale = new Vector3(1, 1) * iconSizes[0];
            
            if (ignoreUpdate) return;
            _radarSequence = DOTween.Sequence();
            _radarSequence
                .Append(dotOuter.DOFade(0, 0).OnComplete(() => {
                    dotOuter.transform.localScale = Vector3.zero;
                    dotInner.transform.localScale = Vector3.zero;
                }))
                .Append(dotOuter.transform.DOScale(1.15f, radarInterval))
                .Insert(0, dotInner.transform.DOScale(1f, radarInterval * 0.5f))
                .Insert(0, dotOuter.DOFade(1, 0.3f * radarInterval))
                .Insert(0.7f, dotOuter.DOFade(0, 0.3f * radarInterval))
                .Insert(0.7f, dotInner.transform.DOScale(0f, radarInterval * 0.3f))
                .SetLoops(-1, LoopType.Restart);
        }

        private void ChangeSize(int param) {
            transform.localScale = new Vector3(1, 1) * iconSizes[param];
        }

        private void OnDestroy() {
            this.RemoveListener(EventType.ChangeMinimapIconSize);
            _radarSequence?.Kill();
        }
    }
}