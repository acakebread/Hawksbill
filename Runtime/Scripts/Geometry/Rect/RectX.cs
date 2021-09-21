// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 13/07/2021 20:16:26 by seantcooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V2I = UnityEngine.Vector2Int;
using V2 = UnityEngine.Vector2;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class RectX
    {
        public static IEnumerable<V2I> getPositions(this RectInt b)
        {
            foreach (var p in b.allPositionsWithin) yield return p;
        }

        public static RectInt intersection(this RectInt b1, RectInt b2)
        {
            int xMin = Mathf.Max (b1.xMin, b2.xMin), xMax = Mathf.Min (b1.xMax, b2.xMax);
            int yMin = Mathf.Max (b1.yMin, b2.yMin), yMax = Mathf.Min (b1.yMax, b2.yMax);
            return (xMin < xMax && yMin < yMax) ? new RectInt (xMin, yMin, xMax - xMin, yMax - yMin) : new RectInt ();
        }

        public static IEnumerable<V2> getPositions(this Rect r, float size, bool center = false, bool stretch = false) =>
            r.getPositions (new V2 (size, size), center, stretch);

        public static IEnumerable<V2> getPositions(this Rect r, V2 size, bool center = false, bool stretch = false)
        {
            if (size.x == 0 || size.y == 0) yield break;
            V2 step = !stretch ? size : new V2 (r.size.x / Mathf.Round (r.size.x / size.x), r.size.y / Mathf.Round (r.size.y / size.y));
            V2 offset = center ? step / 2 : V2.zero;
            V2 max = r.max - offset;
            for (V2 x = r.min + offset; x.x <= max.x; x.x += step.x)
                for (V2 y = x; y.y <= max.y; y.y += step.y)
                    yield return y;
        }

        public static Rect inflate(this Rect r, float amount) =>
            new Rect (r.xMin - amount, r.yMin - amount, r.width + amount * 2, r.height + amount * 2);
    }
}