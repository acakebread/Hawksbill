// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 26/02/2021 12:44:13 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    public static class ColorX
    {
        public static string html(this Color color) => ColorUtility.ToHtmlStringRGBA (color);
        public static Color inverse(this Color color) => new Color (1 - color.r, 1 - color.g, 1 - color.b);
        public static Color32 inverse(this Color32 color) => new Color32 ((byte) (255 - color.r), (byte) (255 - color.g), (byte) (255 - color.b), 255);

        static int floatToInt(float g) => Mathf.RoundToInt (g * 255);
        public static int toInt24(this Color color) => toInt24 ((Color32) color);
        public static int toInt24(this Color32 color) => (color.r << 16) | (color.g << 8) | color.b;
    }
}