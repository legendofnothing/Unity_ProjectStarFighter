
using System.Globalization;
using UnityEngine;

namespace Core {
    public static class ColorExtension {
        public static Color FromHex(string hex) {
            if (hex.Length < 6) {
                throw new System.FormatException("Needs a string with a length of at least 6");
            }

            var r = hex[..2];
            var g = hex.Substring(2, 2);
            var b = hex.Substring(4, 2);
            var alpha = hex.Length >= 8 ? hex.Substring(6, 2) : "FF";

            return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f), (int.Parse(g, NumberStyles.HexNumber) / 255f), (int.Parse(b, NumberStyles.HexNumber) / 255f), (int.Parse(alpha, NumberStyles.HexNumber) / 255f));
        }
    }
}