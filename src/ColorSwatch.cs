using System.Drawing;

namespace ColorSorter {
    public class ColorSwatch {
        public string Name { get; }
        public Color Color { get; }

        public ColorSwatch(string name, Color color) {
            Name = name;
            Color = color;
        }
    }
}
