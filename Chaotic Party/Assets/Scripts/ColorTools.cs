using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorTools
{
    private static Dictionary<Color, string> _colorToName = new ()
    {
        { Color.white, "Blanc" },
        { new Color(0.454902f, 0.6784314f, 0.1960784f), "Vert"},
        { new Color(0.6352941f, 0.1803922f, 0.6588235f), "Violet"},
        { new Color(0.812f, 0.059f, 0.043f), "Rouge"},
        { new Color(1f, 0.514f, 0.969f), "Rose"},
        { new Color(0.898f, 0.792f, 0.349f), "Jaune"}
    };

    public static bool ColorToName(Color color, out string colorName)
    {
        foreach ((Color key, string value) in _colorToName)
        {
            if (!ColorEqualsApproximate(key, color)) continue;
            colorName = value;
            return true;
        }
    
        if (_colorToName.ContainsKey(color))
        {
            colorName = _colorToName[color];
            return true;
        }

        colorName = "";
        return false;
    }

    private static bool ColorEqualsApproximateWithAlpha(Color color1, Color color2)
    {
        return ColorEqualsApproximate(color1, color2) &&
               Math.Abs(color1.a - color2.a) < 0.1f;
    }

    private static bool ColorEqualsApproximate(Color color1, Color color2)
    {
        return Math.Abs(color1.r - color2.r) < 0.1f &&
               Math.Abs(color1.g - color2.g) < 0.1f &&
               Math.Abs(color1.b - color2.b) < 0.1f;
    }
}
