// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:59 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V3I = UnityEngine.Vector3Int;

namespace Hawksbill
{
    public static class BoundsIntX
    {
        public static BoundsInt offset(this BoundsInt b, V3I offset) { b.position += offset; return b; }
        public static BoundsInt expand(this BoundsInt b, int offset) => b.expand (new V3I (offset, offset, offset));
        public static BoundsInt expand(this BoundsInt b, V3I offset) => new BoundsInt (b.min - offset, b.size + offset * 2);

        public static bool intersects(this BoundsInt b1, BoundsInt b2) =>
            b1.xMin < b2.xMax && b2.xMin < b1.xMax && b1.yMin < b2.yMax && b2.yMin < b1.yMax && b1.zMin < b2.zMax && b2.zMin < b1.zMax;

        public static BoundsInt intersection(this BoundsInt b1, BoundsInt b2)
        {
            int xMin = Mathf.Max (b1.xMin, b2.xMin), xMax = Mathf.Min (b1.xMax, b2.xMax);
            int yMin = Mathf.Max (b1.yMin, b2.yMin), yMax = Mathf.Min (b1.yMax, b2.yMax);
            int zMin = Mathf.Max (b1.zMin, b2.zMin), zMax = Mathf.Min (b1.zMax, b2.zMax);
            if (xMin < xMax && yMin < yMax && zMin < zMax) return new BoundsInt (xMin, yMin, zMin, xMax - xMin, yMax - yMin, zMax - zMin);
            return new BoundsInt ();
        }

        public static IEnumerable<V3I> getPositions(this BoundsInt b1, BoundsInt b2) => b1.intersection (b2).getPositions ();
        public static IEnumerable<V3I> getPositions(this BoundsInt b)
        {
            foreach (var p in b.allPositionsWithin)
                yield return p;
        }

        public static BoundsInt GetBoundsIntFromSphere(Vector3 pos, float radius)
        {
            int xMin = Mathf.FloorToInt (pos.x - radius), xMax = Mathf.CeilToInt (pos.x + radius);
            int yMin = Mathf.FloorToInt (pos.y - radius), yMax = Mathf.CeilToInt (pos.y + radius);
            int zMin = Mathf.FloorToInt (pos.z - radius), zMax = Mathf.CeilToInt (pos.z + radius);
            return new BoundsInt (xMin, yMin, zMin, xMax - xMin, yMax - yMin, zMax - zMin);
        }
    }
}