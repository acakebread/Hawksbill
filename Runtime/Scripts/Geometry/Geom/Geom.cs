// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:59 by seancooper
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using Unity.Mathematics;

namespace Hawksbill
{
    public static partial class Geom
    {
        public static Bounds GetBoundsFromPoints(IEnumerable<Vector3> points)
        {
            if (points.Count () == 0) return new Bounds ();
            var min = new V3 (points.Min (p => p.x), points.Min (p => p.y), points.Min (p => p.z));
            var max = new V3 (points.Max (p => p.x), points.Max (p => p.y), points.Max (p => p.z));
            return new Bounds ((max - min) / 2, max - min);
        }

        public static float Circumference(float radius) => 2 * Mathf.PI * radius;

        public static float GetTraingleArea(V3 p1, V3 p2, V3 p3)
        {
            float a = (p2 - p1).magnitude, b = (p3 - p2).magnitude, c = (p1 - p3).magnitude;
            float s = (a + b + c) / 2;
            return Mathf.Sqrt (s * (s - a) * (s - b) * (s - c));
        }


        // LINES
        /// <summary>Get the intersection of 2 line segments</summary>
        public static bool GetIntersectionSegment(V3 p1a, V3 p1b, V3 p2a, V3 p2b, out V3 intersection, float tolerance = 0)
        {
            intersection = V3.zero;
            V3 d1 = p1b - p1a, d2 = p2b - p2a;
            float d = d1.x * d2.z - d1.z * d2.x;
            if (d >= -tolerance && d < tolerance) return false;
            float u = ((p2a.x - p1a.x) * (d2.z) - (p2a.z - p1a.z) * (d2.x)) / d;
            if (u < 0.0f || u > 1.0f) return false;
            float v = ((p2a.x - p1a.x) * (d1.z) - (p2a.z - p1a.z) * (d1.x)) / d;
            if (v < 0.0f || v > 1.0f) return false;
            intersection = p1a + (p1b - p1a) * u;
            return true;
        }

        /// <summary>Get the intersection of 2 lines</summary>
        public static bool GetIntersection(V3 p1a, V3 p1b, V3 p2a, V3 p2b, out V3 intersection, float tolerance = 0)
        {
            intersection = V3.zero;
            V3 d1 = p1b - p1a, d2 = p2b - p2a;
            float d = d1.x * d2.z - d1.z * d2.x;
            if (d >= -tolerance && d < tolerance) return false;
            float u = ((p2a.x - p1a.x) * (p2b.z - p2a.z) - (p2a.z - p1a.z) * (p2b.x - p2a.x)) / d;
            intersection = p1a + (p1b - p1a) * u;
            return true;
        }

        public static double places(this double n, int places)
        {
            double f = math.pow (10, places);
            return math.round (n * f) / f;
        }
        public static float places(this float n, int places)
        {
            float f = math.pow (10, places);
            return math.round (n * f) / f;
        }

        // // Curve
        // public static IEnumerable<V3> Curve(V3 p1, V3 d1, V3 p2, V3 d2, int count, bool straightStart = false)
        // {
        //     if (Geom.GetIntersection (p1, p1 + d1 * 1000, p2, p2 - d2 * 1000, out V3 intersection))
        //     {
        //         V3 lerp(V3 p1, V3 p2, float f) => V3.Lerp (V3.Lerp (p1, intersection, f), V3.Lerp (intersection, p2, f), f);
        //         return EnumerableUtility.Range (0, 1, 1f / (count - 1)).Select (f => lerp (p1, p2, f));
        //     }
        //     return new V3[] { p1, p2 };
        // }

        // /// <summary>Get the intersection of 2 lines</summary>
        // public static bool GetIntersection(V3 p1, V3 d1, V3 p2, V3 d2, out V3 intersection, float tolerance = 0)
        // {
        //     intersection = V3.zero;
        //     float d = d1.x * d2.z - d1.z * d2.x;
        //     if (d >= -tolerance && d < tolerance) return false;
        //     float u = ((p2.x - p1.x) * (d2.z) - (p2.z - p1.z) * (p2.x)) / d;
        //     intersection = p1 + d1 * u;
        //     return true;
        // }

        // Curve
        public static IEnumerable<V3> Curve(V3 position1, V3 direction1, V3 position2, V3 direction2, int count, bool straightStart = false)
        {
            if (Geom.GetIntersection (position1, position1 + direction1 * 1000, position2, position2 + direction2 * 1000, out V3 intersection))
            {
                V3 p1 = position1, p2 = position2;
                if (straightStart)
                {
                    if (count < 3) straightStart = false;
                    else
                    {
                        position1 += (intersection - position1) / count;
                        position2 += (intersection - position2) / count;
                        count -= 2;
                        yield return p1;
                    }
                }

                foreach (var f in EnumerableUtility.Range (0, 1, 1f / (count - 1)))
                    yield return V3.Lerp (V3.Lerp (position1, intersection, f), V3.Lerp (intersection, position2, f), f);

                if (straightStart) yield return p2;
            }
            else
            {
                // throw new Exception ("Cannot curve with no intersection!");
                yield return position1;
                yield return position2;
            }
        }

        // Triangles
        public static V3 GetTriangleNormal(V3 p1, V3 p2, V3 p3) => (Vector3.Cross (p2 - p1, p3 - p1)).normalized;
    }
}



