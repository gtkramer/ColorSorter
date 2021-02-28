using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorSorter {
    public class ColorFilter {
        public string Name { get; }
        private int MinInclusiveHue;
        private int MaxExclusiveHue;
        private Func<ColorSwatch, int, int, bool> Selector;

        public ColorFilter(string name, int minInclusiveHue, int maxExclusiveHue, Func<ColorSwatch, int, int, bool> selector) {
            Name = name;
            MinInclusiveHue = minInclusiveHue;
            MaxExclusiveHue = maxExclusiveHue;
            Selector = selector;
        }

        public IEnumerable<ColorSwatch> FilterColors(IEnumerable<ColorSwatch> colorSwatches) {
            return colorSwatches.Where(x => Selector(x, MinInclusiveHue, MaxExclusiveHue));
        }

        public static bool ColorHueContiguousSelector(ColorSwatch colorSwatch, int minInclusiveHue, int maxExclusiveHue) {
            float h = colorSwatch.Color.GetHue();
            return h >= minInclusiveHue && h < maxExclusiveHue;
        }

        public static bool ColorHueBreakSelector(ColorSwatch colorSwatch, int minInclusiveHue, int maxExclusiveHue) {
            float h = colorSwatch.Color.GetHue();
            return h >= minInclusiveHue || h < maxExclusiveHue;
        }

        public override string ToString() {
            return Name;
        }
    }
}
