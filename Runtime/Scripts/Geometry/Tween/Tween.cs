// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 21/01/2021 15:51:49 by seantcooper
using UnityEngine;
using Hawksbill;
using Unity.Mathematics;

namespace Hawksbill.Geometry
{
    ///<summary>Tween maths</summary>
    // EaseIn - Leaving/Moving out (0)
    // EaseOut - Arriving/Moving in (1)
    public static class Tween
    {
        public static float SimpleInOut(float unit) => Smooth (Mathf.Clamp01 (unit));
        public static float InOut(float unit, float easeIn = 0.5f, float easeOut = 0.5f) =>
            math.lerp (unit, Smooth (unit), GetEase (unit, easeIn, easeOut));

        static float Smooth(float unit) => BezierBlend (BezierBlend (BezierBlend (unit)));
        static float GetEase(float unit, float easeIn, float easeOut) => Mathf.Clamp01 (easeOut) * unit + Mathf.Clamp01 (easeIn) * (1 - unit);

        static float SmoothBlend(float f) => 3.0F * f * f - 2.0F * f * f * f;
        static float InOutQuadBlend(float t) => t <= 0.5f ? 2.0f * t * t : 2.0f * (t - 0.5f) * (1.0f - (t - 0.5f)) + 0.5f;
        static float BezierBlend(float t) => t * t * (3.0f - 2.0f * t);
        static float ParametricBlend(float t) => (t * t) / (2.0f * ((t * t) - t) + 1.0f);

        public class UnitTime
        {
            float startTime, endTime;
            public UnitTime(float duration) => endTime = (startTime = Time.time) + duration;
            public float unit => Mathf.Clamp01 (((Time.time - startTime) / (endTime - startTime)));
            public static implicit operator float(UnitTime t) => t.unit;
        }
    }
}

// // Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 21/01/2021 15:51:49 by seantcooper
// using UnityEngine;
// using Hawksbill;
// using Unity.Mathematics;

// namespace Hawksbill.Geometry
// {
//     ///<summary>Tween maths</summary>
//     public static class Tween
//     {
//         public static float SimpleInOut(float unit) => smooth (Mathf.Clamp01 (unit));
//         public static float InOut(float unit, float inv = 0.5f, float outv = 0.5f) =>
//             math.lerp (unit, smooth (unit), unit < 0.5f ? Mathf.Clamp01 (outv) : Mathf.Clamp01 (inv));

//         static float smooth(float unit, int iterations = 3)
//         {
//             for (int i = math.clamp (iterations, 1, 5); i > 0; --i)
//                 unit = 3.0F * unit * unit - 2.0F * unit * unit * unit;
//             return unit;
//         }

//         public class UnitTime
//         {
//             float startTime, endTime;
//             public UnitTime(float duration) => endTime = (startTime = Time.time) + duration;
//             public float unit => Mathf.Clamp (((Time.time - startTime) / (endTime - startTime)), 0, 1);
//             public static implicit operator float(UnitTime t) => t.unit;
//         }
//     }
// }