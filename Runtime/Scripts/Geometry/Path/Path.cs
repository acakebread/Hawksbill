// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 20/04/2021 18:19:06 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections.Generic;
using V3 = UnityEngine.Vector3;
using System;
using System.Linq;
using Unity.Mathematics;

namespace Hawksbill.Geometry
{
    ///<summary>Put text here to describe the Class</summary>
    public static class Path
    {
        public static Bounds getBounds(this V3[] path)
        {
            if (path.Length == 0) return new Bounds ();
            V3 min = new V3 (path.Min (p => p.x), path.Min (p => p.y), path.Min (p => p.z));
            V3 max = new V3 (path.Max (p => p.x), path.Max (p => p.y), path.Max (p => p.z));
            return new Bounds ((max + min) / 2, max - min);
        }

        public static IEnumerable<V3> distributePointsAlongPath(this V3[] path, float spacing, bool loop = true)
        {
            foreach (IndexRange range in distributeUnitsAlongPath (path, spacing, loop))
                yield return V3.Lerp (path[range.index1], path[range.index2], range.f);
        }

        public static IEnumerable<IndexRange> distributeUnitsAlongPath(this V3[] path, float spacing, bool loop = true)
        {
            if (path.Length >= 2)
            {
                float dTotal = path.getPathDistance (loop);
                spacing = dTotal / Mathf.Round (dTotal / spacing);
                float dCurrent = 0, dIndex = 0;
                for (int i = 0, n = path.Length - (loop ? 1 : 0); i < n; i++)
                {
                    int i2 = (i + 1) % path.Length;
                    float d = V3.Distance (path[i2], path[i]);
                    for (float dEnd = dIndex + d; dCurrent < dEnd; dCurrent += spacing)
                    {
                        //float index = (float) i + (dCurrent - dIndex) / d;
                        yield return new IndexRange (i, i2, (dCurrent - dIndex) / d);
                    }
                    dIndex += d;
                }
            }
        }

        public struct IndexRange
        {
            ///<summary>First Object index</summary>
            public int index1;
            ///<summary>Second Object index</summary>
            public int index2;
            ///<summary>Distance between index1 and index2</summary>
            public float f;
            public IndexRange(int index1, int index2, float f)
            {
                this.index1 = index1;
                this.index2 = index2;
                this.f = f;
            }
        }

        public static float getPathDistance(this V3[] path, bool loop = true)
        {
            if (path.Length < 2) return 0;
            float distance = loop ? V3.Distance (path[path.Length - 1], path[0]) : 0;
            for (int i = path.Length - 2; i >= 0; i--)
                distance += V3.Distance (path[i], path[i + 1]);
            return distance;
        }

        public static float getShortestDistance(this V3[] path, V3 position, bool loop = true)
        {
            if (path.Length == 0) return float.MaxValue;
            if (path.Length == 1) return V3.Distance (position, path[0]);

            V3 ProjectPointLine(Vector3 point, Vector3 line1, Vector3 line2)
            {
                V3 p = point - line1, lineDirection = line2 - line1;
                float length = lineDirection.magnitude;
                if (length > .000001f) lineDirection /= length;
                return line1 + lineDirection * Mathf.Clamp (V3.Dot (lineDirection, p), 0.0F, length);
            }

            var pts = loop ? path.Concat (new V3[] { path[0] }).ToArray () : path;
            return pts.Skip (1).Select ((p, i) => V3.Distance (position, ProjectPointLine (position, p, pts[i]))).Min ();
        }

        public static float getShortestDistance(this V3[] path, V3[] line, out V3 point, bool loop = true)
        {
            V3 result = point = V3.zero;
            if (path.Length < 2) return float.MaxValue;
            //var pts = loop ? path.Concat (new V3[] { path[0] }).ToArray () : path;
            float minDistance = float.MaxValue, d = 0;

            void line2line(V3[] pathLine)
            {
                var ps = LineLineClosestPoints (pathLine, line);
                if ((d = V3.Distance (ps[0], ps[1])) < minDistance)
                {
                    minDistance = d;
                    result = ps[0];
                }
            }

            for (int i = 0, n = path.Length - 1; i < n; line2line (new V3[] { path[i], path[i + 1] }), i++) ;
            if (loop) line2line (new V3[] { path[0], path.Last () });

            point = result;
            return minDistance;
        }

        public static V3[] LineLineClosestPoints(V3[] line1, V3[] line2)
        {
            V3 u = line1[1] - line1[0];
            V3 v = line2[1] - line2[0];
            V3 a = line1[0], b = line2[0];

            V3 r = b - a;

            float ru = V3.Dot (r, u);

            float rv = V3.Dot (r, v);

            float uu = V3.Dot (u, u);
            float uv = V3.Dot (u, v);
            float vv = V3.Dot (v, v);

            float det = uu * vv - uv * uv;
            float s = (ru * vv - rv * uv) / det;
            float t = (ru * uv - rv * uu) / det;

            s = Mathf.Clamp01 (s);
            t = Mathf.Clamp01 (t);

            float S = (t * uv + ru) / uu;
            float T = (s * uv - rv) / vv;

            S = Mathf.Clamp01 (s);
            T = Mathf.Clamp01 (t);
            return new V3[] { a + S * u, b + T * v };
        }



        // //https://wiki.unity3d.com/index.php/3d_Math_functions
        // public static float LineLineDistance(V3[] line1, V3[] line2)
        // {
        //     V3 d1 = line1[1] - line1[0];
        //     V3 d2 = line2[1] - line2[0];

        //     float a = Vector3.Dot (d1, d1);
        //     float b = Vector3.Dot (d1, d2);
        //     float e = Vector3.Dot (d2, d2);
        //     float d = a * e - b * b;

        //     if (d != 0.0f)
        //     {
        //         V3 r = line1[0] - line2[0];
        //         float c = Vector3.Dot (d1, r);
        //         float f = Vector3.Dot (d2, r);
        //         V3 c1 = line1[0] + d1 * ((b * f - c * e) / d);
        //         V3 c2 = line2[0] + d2 * ((a * f - c * b) / d);
        //         var dist = V3.Distance (c1, c2);

        //         return dist;
        //     }

        //     // V3 ProjectPointOnLine(V3 linePoint, V3 lineVec, Vector3 point)
        //     // {
        //     //     Vector3 linePointToPoint = line2[0] - line1[0];
        //     //     float t = Vector3.Dot (linePointToPoint, d1);
        //     //     return line1[0] + d1 * t;
        //     // }
        //     return V3.Distance (line2[0], line1[0] + d1 * V3.Dot (line2[0] - line1[0], d1));
        // }

    }
}