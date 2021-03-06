﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ColorSorter.ExtensionMethods;
using CommandLine;

namespace ColorSorter {
    public class Program {
        public static void Main(string[] args) {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(RunOptions);
            //.WithNotParsed<Options>(HandleParseError);
        }

        public class Options {
            [Option("color-swatches", Required = true, HelpText = "Path to file listing color swatchs on each line as '<name>,#<hex>'")]
            public string ColorSwatchPath { get; set; }

            [Option("min-hue", Required = false, Default = 0.0f, HelpText = "Min hue value")]
            public float MinHue { get; set; }
            [Option("max-hue", Required = false, Default = 360.0f, HelpText = "Max hue value")]
            public float MaxHue { get; set; }

            [Option("min-saturation", Required = false, Default = 0.0f, HelpText = "Min saturation value")]
            public float MinSaturation { get; set; }
            [Option("max-saturation", Required = false, Default = 1.0f, HelpText = "Max saturation value")]
            public float MaxSaturation { get; set; }

            [Option("min-lightness", Required = false, Default = 0.0f, HelpText = "Min lightness value")]
            public float MinLightness { get; set; }
            [Option("max-lightness", Required = false, Default = 1.0f, HelpText = "Max lightness value")]
            public float MaxLightness { get; set; }
        }

        private static void RunOptions(Options opts) {
            List<ColorSwatch> colorSwatches = ReadColorSwatches(opts.ColorSwatchPath);
            List<ColorFilter> colorFilters = GetColorFilters();
            foreach (ColorFilter colorFilter in colorFilters) {
                ColorSwatch[] filteredColorSwatches = colorFilter.FilterColors(colorSwatches).Where(x => ColorHslSelector(x, opts)).OrderBy(x => x.Color.GetBrightness()).ToArray();
                string fileName = colorFilter.Name + ".png";
                if (File.Exists(fileName)) {
                    File.Delete(fileName);
                }
                if (filteredColorSwatches.Length != 0) {
                    ColorWriter colorWriter = new ColorWriter(fileName, ImageFormat.Png);
                    colorWriter.WriteColors(filteredColorSwatches);
                    PrintFilteredColors(colorFilter, filteredColorSwatches);
                }
            }
        }

        private static List<ColorSwatch> ReadColorSwatches(string path) {
            List<ColorSwatch> colorSwatches = new List<ColorSwatch>();
            using (StreamReader reader = new StreamReader(path)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] splitLine = line.Split(',');
                    colorSwatches.Add(new ColorSwatch(splitLine[0], ColorTranslator.FromHtml(splitLine[1])));
                }
            }
            return colorSwatches;
        }

        private static List<ColorFilter> GetColorFilters() {
            List<ColorFilter> colorFilters = new List<ColorFilter>();
            colorFilters.Add(new ColorFilter("Red", 355, 10, ColorFilter.ColorHueBreakSelector));
            colorFilters.Add(new ColorFilter("Red-Orange", 10, 20, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Orange-Brown", 20, 40, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Orange-Yellow", 40, 50, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Yellow", 50, 60, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Yellow-Green", 60, 80, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Green", 80, 140, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Green-Cyan", 140, 170, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Cyan", 170, 200, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Cyan-Blue", 200, 220, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Blue", 220, 240, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Blue-Magenta", 240, 280, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Magenta", 280, 320, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Magenta-Pink", 320, 330, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Pink", 330, 345, ColorFilter.ColorHueContiguousSelector));
            colorFilters.Add(new ColorFilter("Pink-Red", 345, 355, ColorFilter.ColorHueContiguousSelector));
            return colorFilters;
        }

        private static bool ColorHslSelector(ColorSwatch colorSwatch, Options opts) {
            Color color = colorSwatch.Color;
            float h = color.GetHue();
            float s = color.GetSaturation();
            float l = color.GetLightness();
            return
                h >= opts.MinHue        && h <= opts.MaxHue &&
                s >= opts.MinSaturation && s <= opts.MaxSaturation &&
                l >= opts.MinLightness  && l <= opts.MaxLightness;
        }

        private static void PrintFilteredColors(ColorFilter colorFilter, ColorSwatch[] colorSwatches) {
            Console.WriteLine(colorFilter.Name);
            foreach (ColorSwatch colorSwatch in colorSwatches) {
                Color color = colorSwatch.Color;
                Console.WriteLine(color.GetHue().ToString("000.000") + ", " + color.GetSaturation().ToString("0.000") + ", " + color.GetLightness().ToString("0.000") + ": " + colorSwatch.Name);
            }
            Console.WriteLine();
        }
    }
}
