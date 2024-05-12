using System.Collections;
using Core;
using UnityEngine;

namespace Scenes.PredictionTest {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Gun : MonoBehaviour {
        public Rigidbody2D rb;
        public Transform target;
        public float speed = 5f;
        public GameObject bullet;
        private bool _canFire = true;

        [Space] 
        public Rigidbody2D _targetRb;

        private Vector3 pos;
        private float root1;
        private float root2;

        private bool _started;
        private void Start() {
            _started = true;
        }
        
        private void Update() {
            if (HasInterceptDirection(
                    target.position,
                    transform.position,
                    _targetRb.velocity,
                    speed,
                    out var result)) {
                pos = result;
            }
            else {
                pos = target.position;
            }
            
            var dir = (pos - transform.position).normalized;
            dir.z = 0;
            var rot = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f, Vector3.forward);
            rb.SetRotation(Quaternion.RotateTowards(transform.rotation, rot, 99f));
            
            if (_canFire) {
                StartCoroutine(FireRoutine());
            }
        }

        IEnumerator FireRoutine() {
            _canFire = false;
            yield return new WaitForSeconds(0.02f);
            var inst = Instantiate(bullet, transform.position, transform.rotation);
            inst.GetComponent<Bullet>().speed = speed;
            _canFire = true;
        }

        private void OnDrawGizmos() {
            if (_started) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(pos, Vector2.one * 1.2f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, pos);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + transform.up * 5f);
                DebugExtension.DrawString($"root1: {root1}\nroot2: {root2}", transform.position - (Vector3)Vector2.one, Color.white, Vector2.zero);
            }
        }
        
        /// <summary>
        /// shit
        /// </summary>
        /// <param name="a">target pos</param>
        /// <param name="b">current pos</param>
        /// <param name="vA">target vel</param>
        /// <param name="sB">current vel</param>
        /// <param name="result">direction to aim for</param>
        /// <returns></returns>
        public bool HasInterceptDirection(Vector2 a, Vector2 b, Vector2 vA, float sB, out Vector2 result) {
            var aToB = b - a;
            var dC = aToB.magnitude;
            var angle = Vector2.Angle(aToB, vA) * Mathf.Deg2Rad;
            var sA = vA.magnitude;
            var r = sA / sB;
            if (CustomMathExtension.SolveQuadratic(
                    1 - r * r, 
                    2 * r * dC * Mathf.Cos(angle), 
                    - dC * dC, 
                    out var root1, 
                    out var root2) == 0) {
                result = Vector2.zero;
                return false;
            }

            this.root1 = root1;
            this.root2 = root2;

            var dA = Mathf.Max(root1, root2);
            var t = dA / sB;
            var c = a + vA * t;
            result = c;
            return true;
        }
    }

    public static class CustomMathExtension {
        public static int SolveQuadratic(float a, float b, float c, out float root1, out float root2) {
            var discriminant = b * b - 4 * a * c;
            if (discriminant < 0) {
                root1 = Mathf.Infinity;
                root2 = -root1;
                return 0;
            }

            root1 = -b + Mathf.Sqrt(discriminant) / 2 * a;
            root2 = -b - Mathf.Sqrt(discriminant) / 2 * a;
            return discriminant > 0 ? 2 : 1;
        }
    }
}
