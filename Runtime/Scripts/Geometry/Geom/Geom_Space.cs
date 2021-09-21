// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 21/01/2021 15:51:49 by seantcooper
using UnityEngine;
using Hawksbill;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace Hawksbill
{
    public static partial class Geom
    {
        public class Space
        {
            public static IEnumerable<Vector3> GetCircularPoints(float radius, float spacing, int count)
            {
                IEnumerable<float> angles()
                {
                    float angularSpacing = 360 * spacing / Geom.Circumference (radius);
                    for (float angle = -(angularSpacing * (count - 1)) / 2; count > 0; --count, angle += angularSpacing)
                        yield return angle;
                }
                foreach (float angle in angles ().OrderBy (a => Mathf.Abs (a)))
                    yield return Quaternion.Euler (0, angle + 180, 0) * Vector3.forward * radius;
            }

        }
    }
}
