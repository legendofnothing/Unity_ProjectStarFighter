using UnityEngine;

namespace EnemyScript {
    //headache
    public static class PredictPosition {
        /// <param name="a">target pos</param>
        /// <param name="b">current pos</param>
        /// <param name="vA">target vel</param>
        /// <param name="sB">current speed</param>
        /// <param name="result">out predicted position</param>
        /// <returns></returns>
        public static bool HasInterceptDirection(Vector2 a, Vector2 b, Vector2 vA, float sB, out Vector2 result) {
            var aToB = b - a;
            var dC = aToB.magnitude;
            var angle = Vector2.Angle(aToB, vA) * Mathf.Deg2Rad;
            var sA = vA.magnitude;
            var r = sA / sB;
            if (SolveQuadratic(
                    1 - r * r, 
                    2 * r * dC * Mathf.Cos(angle), 
                    - dC * dC, 
                    out var root1, 
                    out var root2) == 0) {
                result = Vector2.zero;
                return false;
            }
            
            var dA = Mathf.Max(root1, root2);
            var t = dA / sB;
            var c = a + vA * t;
            result = c;
            return true;
        }
        
        private static int SolveQuadratic(float a, float b, float c, out float root1, out float root2) {
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