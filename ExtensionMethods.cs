using System.Drawing;

namespace ExtensionMethods {
    public static class ColorExtensions {
        public static float GetLightness(this Color color) {
            return color.GetBrightness();
        }
    }
}
