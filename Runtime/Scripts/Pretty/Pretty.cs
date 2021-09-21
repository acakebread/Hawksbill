// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 18/02/2021 10:05:59 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Hawksbill
{
    ///<summary>Static class that logs errors in pretty format with color</summary>
    public static class Pretty
    {
        public static class Colors
        {
            public static Color AR = new Color (1, 065f, 0);
            public static Color Network = new Color (0.69f, 0.94f, 0.94f);
            public static Color Prefs = new Color (1, 0.4f, 1);
            public static Color Anim = new Color (1, 0, 1);
            public static Color Audio = new Color (0, 1, 0.75f);
            public static Color Timeline = new Color32 (64, 224, 208, 255);
            public static Color Important = new Color (1, 0, 0, 1);
            public static Color Unimportant = new Color (1, 1, 1, 0.33f);
        }

        static void DebugLog(object msg) => UnityEngine.Debug.Log (msg);
        static void DebugLogWarning(object msg) => UnityEngine.Debug.LogWarning (msg);
        static void DebugLogError(object msg) => UnityEngine.Debug.LogError (msg);

        ///<summary>Log messages in a pretty format</summary>
        public static void Log(Color color, object msg, object post = null) =>
            DebugLog (str (color, msg, post));

        ///<summary>Log warning in a pretty format</summary>
        public static void LogWarning(Color color, object msg, object post = null) =>
            DebugLogWarning (str (color, msg, post));

        ///<summary>Log error in a pretty format</summary>
        public static void LogError(Color color, object msg, object post = null) =>
             DebugLogError ("<color=#" + color.html () + ">" + msg + "</color>" + add (post));

        ///<summary>Log exception in a pretty format</summary>
        public static void LogException(Color color, System.Exception ex) =>
            DebugLogError ("<color=#" + color.html () + ">" + ex.Message + "</color>");


        // static string TrackMessage = "!";
        ///<summary>Log messages in a pretty format</summary>
        public static void Track(object message = null)
        {
            //MethodBase.GetCurrentMethod ().Name
            StackTrace stackTrace = new StackTrace ();
            StackFrame stackFrame = stackTrace.GetFrame (1);
            MethodBase methodBase = stackFrame.GetMethod ();
            DebugLog (
                str (Color.green, "[" + methodBase.ReflectedType.Name + "::" + methodBase.Name + "]") +
                str (Color.green, "[" + Time.frameCount + "]") +
                (message == null ? "" : " " + message));
        }

        //Private Support
        static string co(string color) => "<color=#" + color + ">";
        static string cc() => "</color>";
        static string add(object s) => (s == null) ? "" : "\n" + s;
        static string str(Color color, object msg, object post = null) =>
            "<color=#" + color.html () + ">" + msg + "</color>" + add (post);
    }

    //https://www.w3schools.com/colors/colors_names.asp
    public static class Colors
    {
        public static Color AliceBlue = new Color (0.941176470588235f, 0.972549019607843f, 1f, 1);
        public static Color AntiqueWhite = new Color (0.980392156862745f, 0.92156862745098f, 0.843137254901961f, 1);
        public static Color Aqua = new Color (0f, 1f, 1f, 1);
        public static Color Aquamarine = new Color (0.498039215686275f, 1f, 0.831372549019608f, 1);
        public static Color Azure = new Color (0.941176470588235f, 1f, 1f, 1);
        public static Color Beige = new Color (0.96078431372549f, 0.96078431372549f, 0.862745098039216f, 1);
        public static Color Bisque = new Color (1f, 0.894117647058824f, 0.768627450980392f, 1);
        public static Color Black = new Color (0f, 0f, 0f, 1);
        public static Color BlanchedAlmond = new Color (1f, 0.92156862745098f, 0.803921568627451f, 1);
        public static Color Blue = new Color (0f, 0f, 1f, 1);
        public static Color BlueViolet = new Color (0.541176470588235f, 0.168627450980392f, 0.886274509803922f, 1);
        public static Color Brown = new Color (0.647058823529412f, 0.164705882352941f, 0.164705882352941f, 1);
        public static Color BurlyWood = new Color (0.870588235294118f, 0.72156862745098f, 0.529411764705882f, 1);
        public static Color CadetBlue = new Color (0.372549019607843f, 0.619607843137255f, 0.627450980392157f, 1);
        public static Color Chartreuse = new Color (0.498039215686275f, 1f, 0f, 1);
        public static Color Chocolate = new Color (0.823529411764706f, 0.411764705882353f, 0.117647058823529f, 1);
        public static Color Coral = new Color (1f, 0.498039215686275f, 0.313725490196078f, 1);
        public static Color CornflowerBlue = new Color (0.392156862745098f, 0.584313725490196f, 0.929411764705882f, 1);
        public static Color Cornsilk = new Color (1f, 0.972549019607843f, 0.862745098039216f, 1);
        public static Color Crimson = new Color (0.862745098039216f, 0.0784313725490196f, 0.235294117647059f, 1);
        public static Color Cyan = new Color (0f, 1f, 1f, 1);
        public static Color DarkBlue = new Color (0f, 0f, 0.545098039215686f, 1);
        public static Color DarkCyan = new Color (0f, 0.545098039215686f, 0.545098039215686f, 1);
        public static Color DarkGoldenRod = new Color (0.72156862745098f, 0.525490196078431f, 0.0431372549019608f, 1);
        public static Color DarkGray = new Color (0.662745098039216f, 0.662745098039216f, 0.662745098039216f, 1);
        public static Color DarkGreen = new Color (0f, 0.392156862745098f, 0f, 1);
        public static Color DarkKhaki = new Color (0.741176470588235f, 0.717647058823529f, 0.419607843137255f, 1);
        public static Color DarkMagenta = new Color (0.545098039215686f, 0f, 0.545098039215686f, 1);
        public static Color DarkOliveGreen = new Color (0.333333333333333f, 0.419607843137255f, 0.184313725490196f, 1);
        public static Color DarkOrange = new Color (1f, 0.549019607843137f, 0f, 1);
        public static Color DarkOrchid = new Color (0.6f, 0.196078431372549f, 0.8f, 1);
        public static Color DarkRed = new Color (0.545098039215686f, 0f, 0f, 1);
        public static Color DarkSalmon = new Color (0.913725490196078f, 0.588235294117647f, 0.47843137254902f, 1);
        public static Color DarkSeaGreen = new Color (0.56078431372549f, 0.737254901960784f, 0.56078431372549f, 1);
        public static Color DarkSlateBlue = new Color (0.282352941176471f, 0.23921568627451f, 0.545098039215686f, 1);
        public static Color DarkSlateGray = new Color (0.184313725490196f, 0.309803921568627f, 0.309803921568627f, 1);
        public static Color DarkTurquoise = new Color (0f, 0.807843137254902f, 0.819607843137255f, 1);
        public static Color DarkViolet = new Color (0.580392156862745f, 0f, 0.827450980392157f, 1);
        public static Color DeepPink = new Color (1f, 0.0784313725490196f, 0.576470588235294f, 1);
        public static Color DeepSkyBlue = new Color (0f, 0.749019607843137f, 1f, 1);
        public static Color DimGray = new Color (0.411764705882353f, 0.411764705882353f, 0.411764705882353f, 1);
        public static Color DodgerBlue = new Color (0.117647058823529f, 0.564705882352941f, 1f, 1);
        public static Color FireBrick = new Color (0.698039215686275f, 0.133333333333333f, 0.133333333333333f, 1);
        public static Color FloralWhite = new Color (1f, 0.980392156862745f, 0.941176470588235f, 1);
        public static Color ForestGreen = new Color (0.133333333333333f, 0.545098039215686f, 0.133333333333333f, 1);
        public static Color Fuchsia = new Color (1f, 0f, 1f, 1);
        public static Color Gainsboro = new Color (0.862745098039216f, 0.862745098039216f, 0.862745098039216f, 1);
        public static Color GhostWhite = new Color (0.972549019607843f, 0.972549019607843f, 1f, 1);
        public static Color Gold = new Color (1f, 0.843137254901961f, 0f, 1);
        public static Color GoldenRod = new Color (0.854901960784314f, 0.647058823529412f, 0.125490196078431f, 1);
        public static Color Gray = new Color (0.501960784313726f, 0.501960784313726f, 0.501960784313726f, 1);
        public static Color Green = new Color (0f, 0.501960784313726f, 0f, 1);
        public static Color GreenYellow = new Color (0.67843137254902f, 1f, 0.184313725490196f, 1);
        public static Color HoneyDew = new Color (0.941176470588235f, 1f, 0.941176470588235f, 1);
        public static Color HotPink = new Color (1f, 0.411764705882353f, 0.705882352941177f, 1);
        public static Color IndianRed = new Color (0.803921568627451f, 0.36078431372549f, 0.36078431372549f, 1);
        public static Color Indigo = new Color (0.294117647058824f, 0f, 0.509803921568627f, 1);
        public static Color Ivory = new Color (1f, 1f, 0.941176470588235f, 1);
        public static Color Khaki = new Color (0.941176470588235f, 0.901960784313726f, 0.549019607843137f, 1);
        public static Color Lavender = new Color (0.901960784313726f, 0.901960784313726f, 0.980392156862745f, 1);
        public static Color LavenderBlush = new Color (1f, 0.941176470588235f, 0.96078431372549f, 1);
        public static Color LawnGreen = new Color (0.486274509803922f, 0.988235294117647f, 0f, 1);
        public static Color LemonChiffon = new Color (1f, 0.980392156862745f, 0.803921568627451f, 1);
        public static Color LightBlue = new Color (0.67843137254902f, 0.847058823529412f, 0.901960784313726f, 1);
        public static Color LightCoral = new Color (0.941176470588235f, 0.501960784313726f, 0.501960784313726f, 1);
        public static Color LightCyan = new Color (0.87843137254902f, 1f, 1f, 1);
        public static Color LightGoldenRodYellow = new Color (0.980392156862745f, 0.980392156862745f, 0.823529411764706f, 1);
        public static Color LightGray = new Color (0.827450980392157f, 0.827450980392157f, 0.827450980392157f, 1);
        public static Color LightGreen = new Color (0.564705882352941f, 0.933333333333333f, 0.564705882352941f, 1);
        public static Color LightPink = new Color (1f, 0.713725490196079f, 0.756862745098039f, 1);
        public static Color LightSalmon = new Color (1f, 0.627450980392157f, 0.47843137254902f, 1);
        public static Color LightSeaGreen = new Color (0.125490196078431f, 0.698039215686275f, 0.666666666666667f, 1);
        public static Color LightSkyBlue = new Color (0.529411764705882f, 0.807843137254902f, 0.980392156862745f, 1);
        public static Color LightSlateGray = new Color (0.466666666666667f, 0.533333333333333f, 0.6f, 1);
        public static Color LightSteelBlue = new Color (0.690196078431373f, 0.768627450980392f, 0.870588235294118f, 1);
        public static Color LightYellow = new Color (1f, 1f, 0.87843137254902f, 1);
        public static Color Lime = new Color (0f, 1f, 0f, 1);
        public static Color LimeGreen = new Color (0.196078431372549f, 0.803921568627451f, 0.196078431372549f, 1);
        public static Color Linen = new Color (0.980392156862745f, 0.941176470588235f, 0.901960784313726f, 1);
        public static Color Magenta = new Color (1f, 0f, 1f, 1);
        public static Color Maroon = new Color (0.501960784313726f, 0f, 0f, 1);
        public static Color MediumAquaMarine = new Color (0.4f, 0.803921568627451f, 0.666666666666667f, 1);
        public static Color MediumBlue = new Color (0f, 0f, 0.803921568627451f, 1);
        public static Color MediumOrchid = new Color (0.729411764705882f, 0.333333333333333f, 0.827450980392157f, 1);
        public static Color MediumPurple = new Color (0.576470588235294f, 0.43921568627451f, 0.858823529411765f, 1);
        public static Color MediumSeaGreen = new Color (0.235294117647059f, 0.701960784313726f, 0.443137254901961f, 1);
        public static Color MediumSlateBlue = new Color (0.482352941176471f, 0.407843137254902f, 0.933333333333333f, 1);
        public static Color MediumSpringGreen = new Color (0f, 0.980392156862745f, 0.603921568627451f, 1);
        public static Color MediumTurquoise = new Color (0.282352941176471f, 0.819607843137255f, 0.8f, 1);
        public static Color MediumVioletRed = new Color (0.780392156862745f, 0.0823529411764706f, 0.52156862745098f, 1);
        public static Color MidnightBlue = new Color (0.0980392156862745f, 0.0980392156862745f, 0.43921568627451f, 1);
        public static Color MintCream = new Color (0.96078431372549f, 1f, 0.980392156862745f, 1);
        public static Color MistyRose = new Color (1f, 0.894117647058824f, 0.882352941176471f, 1);
        public static Color Moccasin = new Color (1f, 0.894117647058824f, 0.709803921568628f, 1);
        public static Color NavajoWhite = new Color (1f, 0.870588235294118f, 0.67843137254902f, 1);
        public static Color Navy = new Color (0f, 0f, 0.501960784313726f, 1);
        public static Color OldLace = new Color (0.992156862745098f, 0.96078431372549f, 0.901960784313726f, 1);
        public static Color Olive = new Color (0.501960784313726f, 0.501960784313726f, 0f, 1);
        public static Color OliveDrab = new Color (0.419607843137255f, 0.556862745098039f, 0.137254901960784f, 1);
        public static Color Orange = new Color (1f, 0.647058823529412f, 0f, 1);
        public static Color OrangeRed = new Color (1f, 0.270588235294118f, 0f, 1);
        public static Color Orchid = new Color (0.854901960784314f, 0.43921568627451f, 0.83921568627451f, 1);
        public static Color PaleGoldenRod = new Color (0.933333333333333f, 0.909803921568628f, 0.666666666666667f, 1);
        public static Color PaleGreen = new Color (0.596078431372549f, 0.984313725490196f, 0.596078431372549f, 1);
        public static Color PaleTurquoise = new Color (0.686274509803922f, 0.933333333333333f, 0.933333333333333f, 1);
        public static Color PaleVioletRed = new Color (0.858823529411765f, 0.43921568627451f, 0.576470588235294f, 1);
        public static Color PapayaWhip = new Color (1f, 0.937254901960784f, 0.835294117647059f, 1);
        public static Color PeachPuff = new Color (1f, 0.854901960784314f, 0.725490196078431f, 1);
        public static Color Peru = new Color (0.803921568627451f, 0.52156862745098f, 0.247058823529412f, 1);
        public static Color Pink = new Color (1f, 0.752941176470588f, 0.796078431372549f, 1);
        public static Color Plum = new Color (0.866666666666667f, 0.627450980392157f, 0.866666666666667f, 1);
        public static Color PowderBlue = new Color (0.690196078431373f, 0.87843137254902f, 0.901960784313726f, 1);
        public static Color Purple = new Color (0.501960784313726f, 0f, 0.501960784313726f, 1);
        public static Color RebeccaPurple = new Color (0.4f, 0.2f, 0.6f, 1);
        public static Color Red = new Color (1f, 0f, 0f, 1);
        public static Color RosyBrown = new Color (0.737254901960784f, 0.56078431372549f, 0.56078431372549f, 1);
        public static Color RoyalBlue = new Color (0.254901960784314f, 0.411764705882353f, 0.882352941176471f, 1);
        public static Color SaddleBrown = new Color (0.545098039215686f, 0.270588235294118f, 0.0745098039215686f, 1);
        public static Color Salmon = new Color (0.980392156862745f, 0.501960784313726f, 0.447058823529412f, 1);
        public static Color SandyBrown = new Color (0.956862745098039f, 0.643137254901961f, 0.376470588235294f, 1);
        public static Color SeaGreen = new Color (0.180392156862745f, 0.545098039215686f, 0.341176470588235f, 1);
        public static Color SeaShell = new Color (1f, 0.96078431372549f, 0.933333333333333f, 1);
        public static Color Sienna = new Color (0.627450980392157f, 0.32156862745098f, 0.176470588235294f, 1);
        public static Color Silver = new Color (0.752941176470588f, 0.752941176470588f, 0.752941176470588f, 1);
        public static Color SkyBlue = new Color (0.529411764705882f, 0.807843137254902f, 0.92156862745098f, 1);
        public static Color SlateBlue = new Color (0.415686274509804f, 0.352941176470588f, 0.803921568627451f, 1);
        public static Color SlateGray = new Color (0.43921568627451f, 0.501960784313726f, 0.564705882352941f, 1);
        public static Color Snow = new Color (1f, 0.980392156862745f, 0.980392156862745f, 1);
        public static Color SpringGreen = new Color (0f, 1f, 0.498039215686275f, 1);
        public static Color SteelBlue = new Color (0.274509803921569f, 0.509803921568627f, 0.705882352941177f, 1);
        public static Color Tan = new Color (0.823529411764706f, 0.705882352941177f, 0.549019607843137f, 1);
        public static Color Teal = new Color (0f, 0.501960784313726f, 0.501960784313726f, 1);
        public static Color Thistle = new Color (0.847058823529412f, 0.749019607843137f, 0.847058823529412f, 1);
        public static Color Tomato = new Color (1f, 0.388235294117647f, 0.27843137254902f, 1);
        public static Color Turquoise = new Color (0.250980392156863f, 0.87843137254902f, 0.815686274509804f, 1);
        public static Color Violet = new Color (0.933333333333333f, 0.509803921568627f, 0.933333333333333f, 1);
        public static Color Wheat = new Color (0.96078431372549f, 0.870588235294118f, 0.701960784313726f, 1);
        public static Color White = new Color (1f, 1f, 1f, 1);
        public static Color WhiteSmoke = new Color (0.96078431372549f, 0.96078431372549f, 0.96078431372549f, 1);
        public static Color Yellow = new Color (1f, 1f, 0f, 1);
        public static Color YellowGreen = new Color (0.603921568627451f, 0.803921568627451f, 0.196078431372549f, 1);
    }
}