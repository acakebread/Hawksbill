// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 14/05/2021 08:33:56 by seantcooper
using UnityEngine;
using V3 = UnityEngine.Vector3;
using System.Linq;
using Unity.Mathematics;

namespace Hawksbill.Geometry
{
    [RequireComponent (typeof (LineRenderer))]
    public class Rope : MonoBehaviour
    {
        public Transform anchor1;
        public Transform anchor2;

        [Range (2, 100)] public int pointCount = 32;
        [Range (1, 100)] public float totalLength = 10;
        [Range (0, 2)] public float gravityScale = 0.15f;
        [Range (2, 100)] public int iterations = 50;

        LineRenderer lineRenderer => GetComponent<LineRenderer> ();

        Point[] points;
        float slen;

        void OnValidate() => initializePoints ();
        void Start() => initializePoints ();

        void initializePoints()
        {
            V3[] endPoints = anchor1 && anchor2 ? new V3[] { anchor1.transform.position, anchor2.transform.position } :
                new V3[] { anchor1.transform.position, anchor1.transform.position - anchor1.transform.up * totalLength };

            points = EnumerableUtility.Range (0, 1, 1f / (pointCount - 1)).Select (f => new Point (V3.Lerp (endPoints[0], endPoints[1], f))).ToArray ();
            slen = totalLength / (lineRenderer.positionCount - 1);

            applyToRenderer ();
        }

        void Update() => applyToRenderer ();
        void FixedUpdate() => simulate ();

        void simulate()
        {
            V3 force = UnityEngine.Physics.gravity * gravityScale;
            points.Skip (1).ForAll (s => s.apply (force));
            for (int i = iterations; i > 0; constrain (), --i) ;
        }

        void constrain()
        {
            Point p1 = points[0], p2 = points[1];
            p2.pos += Point.Constrain (p2.pos - p1.pos, slen);
            for (int i = 1; i < points.Length - 1; i++)
            {
                p1 = p2;
                p2 = points[i + 1];
                V3 magnitude = Point.Constrain (p2.pos - p1.pos, slen) * 0.5f;
                p1.pos -= magnitude;
                p2.pos += magnitude;
            }
            if (anchor1) points[0].pos = anchor1.transform.position;
            if (anchor2) points[points.Length - 1].pos = anchor2.transform.position;
        }

        void applyToRenderer()
        {
            lineRenderer.positionCount = pointCount;
            lineRenderer.SetPositions (points.Select (r => r.pos).ToArray ());
            transform.position = (points.First ().pos + points.Last ().pos) / 2;
        }

        class Point
        {
            public V3 pos, last;
            public Point(V3 pos) { this.pos = pos; this.last = pos; }
            public void apply(V3 force)
            {
                V3 velocity = pos - last;
                last = pos;
                pos += velocity;
                pos += force * Time.fixedDeltaTime;
            }

            public static V3 Constrain(V3 direction, float length)
            {
                float distance = direction.magnitude;
                return distance == 0 ? V3.zero : (direction.normalized * math.sign (length - distance) * math.abs (distance - length));
            }
        }
    }
}




//     public Transform anchor1;
//     public Transform anchor2;
//     public float maxLength = 10;
//     [Range (0, 1)] public float tension = 1;
//     [Range (0, 2)] public float gravityScale = 1;

//     LineRenderer lineRenderer => GetComponent<LineRenderer> ();
//     int pointCount => lineRenderer.positionCount;
//     IEnumerable<float> unitSteps => EnumerableUtility.Range (0, 1, 1f / (pointCount - 1));
//     Point[] linePoints => unitSteps.Select ((f, i) => new Point (i, V3.Lerp (anchor1.transform.position, anchor2.transform.position, f))).ToArray ();
//     Point[] points;

//     void OnValidate()
//     {
//         initializePositions ();
//     }

//     void Start()
//     {
//         initializePositions ();
//     }


//     void initializePositions()
//     {
//         V3 p1 = anchor1.transform.position, p2 = anchor2.transform.position;
//         transform.position = (p1 + p2) / 2;
//         points = linePoints;
//         lineRenderer.SetPositions (points.Select (p => p.position).ToArray ());
//     }

//     void Update()
//     {
//         //lineRenderer.GetPositions (positions);
//         //positions = applyVelocities ();
//         applyGravity ();

//         if (Time.frameCount == 18)
//             Debug.Log ("DEBUG");

//         applyConstraints ();

//         points.ForAll (p => p.position += p.velocity);
//         lineRenderer.SetPositions (points.Select (p => p.position).ToArray ());
//     }

//     // Point[] applyVelocities()
//     // {
//     //     V3 p1 = anchor1.transform.position, p2 = anchor2.transform.position;
//     //     V3 d1 = p1 - positions.First (), d2 = p2 - positions.Last ();
//     //     return unitSteps.Select ((f, i) => positions[i] + V3.Lerp (d1, d2, f)).ToArray ();
//     // }

//     void applyGravity()
//     {
//         V3 g = Physics.gravity * gravityScale * Time.deltaTime;
//         points.Skip (1).Take (pointCount - 2).ForAll (p => p.velocity += g);
//     }

//     void applyConstraints()
//     {
//         float dist = maxLength / (pointCount - 1);
//         void constrain(Point p)
//         {
//             V3 d1 = points[p.index - 1].position - p.position, d2 = points[p.index + 1].position - p.position;
//             if (d1.magnitude > dist) p.velocity += (d1.normalized * (d1.magnitude - dist)) * tension;
//             if (d2.magnitude > dist) p.velocity += (d2.normalized * (d2.magnitude - dist)) * tension;
//             //  || d2.magnitude > dist)
//             // {
//             //     p.velocity = V3.zero;
//             // }
//         }
//         points.Skip (1).Take (pointCount - 2).ForAll (constrain);
//     }

//     class Point
//     {
//         public int index;
//         public V3 position, velocity;
//         public Point(int index, V3 position)
//         {
//             this.index = index;
//             this.position = position;
//             this.velocity = V3.zero;
//         }
//     }
// }

