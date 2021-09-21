// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:59 by seancooper
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawksbill
{
    public static class BoundsX
    {
        public static Bounds Encapsulate(this Bounds bounds, IEnumerable<Vector3> positions)
        {
            Vector3 min, max;
            if (bounds.size.sqrMagnitude == 0) min = max = positions.First ();
            else { min = bounds.min; max = bounds.max; }
            foreach (Vector3 p in positions)
            {
                if (p.x < min.x) min.x = p.x; else if (p.x > max.x) max.x = p.x;
                if (p.y < min.y) min.y = p.y; else if (p.y > max.y) max.y = p.y;
                if (p.z < min.z) min.z = p.z; else if (p.z > max.z) max.z = p.z;
            }
            return new Bounds ((min + max) / 2, max - min);
        }
    }
}