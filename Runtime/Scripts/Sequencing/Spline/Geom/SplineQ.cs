// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 30/04/2021 10:08:35 by seantcooper
using UnityEngine;
using Hawksbill;
using V3 = UnityEngine.Vector3;
using Hawksbill.Analytics;

namespace Hawksbill.Sequencing
{
    public class SplineQ
    {
        public const int Resolution = 100; // divisible by 10
        public readonly V3 A, B, C, D;

        public readonly float[] distanceTime;
        public RangeList<V3> positionLookup;
        public V3[] positions => positionLookup.items;
        public readonly float length;
        public readonly int initFrame;

        public SplineQ(V3 A, V3 B, V3 C, V3 D)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;

            length = getLength (0f, 1f);
            distanceTime = new float[Resolution + 1];

            for (int i = 0; i < Resolution; i++)
                distanceTime[i] = findT (length * ((float) i / Resolution), length);
            distanceTime[Resolution] = 1;


            positionLookup = new RangeList<V3> (0, 1, 1f / Resolution, _getPositionAt, V3.Lerp);
            // positions = new V3[Resolution + 1];
            // for (int i = 0; i < Resolution; i++)
            //     positions[i] = getPositionAt (i / Resolution);
            // positions[Resolution] = D;

            initFrame = Time.frameCount;
        }

        public bool equals(V3 A, V3 B, V3 C, V3 D) =>
            this.A == A && this.B == B && this.C == C && this.D == D;

        public V3 getPositionAt(float t) => _getPositionAt (t); //positionLookup[t];
        V3 _getPositionAt(float t)
        {
            // float t1 = 1f - t;
            // V3 Q = t1 * A + t * B;
            // V3 R = t1 * B + t * C;
            // V3 S = t1 * C + t * D;
            // V3 P = t1 * Q + t * R;
            // V3 T = t1 * R + t * S;
            // V3 U = t1 * P + t * T;
            // return U;

            //convert distance to time
            if (t <= 0) return A;
            else if (t >= 1) return D;
            float fi = t * Resolution;
            t = Mathf.LerpUnclamped (distanceTime[(int) fi], distanceTime[(int) fi + 1], fi % 1);

            // get spline position
            float u = 1 - t, uu = u * u, uuu = uu * u;
            float tt = t * t, ttt = tt * t;
            return uuu * A + 3 * uu * t * B + 3 * u * tt * C + ttt * D;
        }

        V3 getDerivative(float t)
        {
            V3 dU = t * t * (-3f * (A - 3f * (B - C) - D));
            dU += t * (6f * (A - 2f * B + C));
            dU += -3f * (A - B);
            return dU;
        }

        float getArcLength(float t) => getDerivative (t).magnitude;

        float getLength(float tStart, float tEnd)
        {
            int n = 20;
            float delta = (tEnd - tStart) / (float) n;
            float endPoints = getArcLength (tStart) + getArcLength (tEnd);
            float x2 = 0f, x4 = 0f;
            for (int i = 1; i < n; x4 += getArcLength (tStart + delta * i), i += 2) ;
            for (int i = 2; i < n; x2 += getArcLength (tStart + delta * i), i += 2) ;
            return (delta / 3f) * (endPoints + 4f * x4 + 2f * x2);
        }

        float findT(float d, float totalLength)
        {
            float t = d / totalLength;
            for (int iterations = 1000; iterations > 0; --iterations)
            {
                float tNext = t - ((getLength (0f, t) - d) / getArcLength (t));
                if (Mathf.Abs (tNext - t) < 0.001f) break;
                t = tNext;
            }
            return t;
        }

        public static implicit operator bool(SplineQ empty) => empty != null;
    }
}

// V3 QBezier(V3 p0, V3 p1, V3 p2, V3 p3, float f)
// {
//     float u = 1 - f, uu = u * u, uuu = uu * u;
//     float ff = f * f, fff = ff * f;
//     return uuu * p0 + 3 * uu * f * p1 + 3 * u * ff * p2 + fff * p3;
// }