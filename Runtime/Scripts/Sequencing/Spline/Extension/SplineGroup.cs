// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 21/05/2021 13:37:30 by seantcooper
using UnityEngine;
using Hawksbill;
using V3 = UnityEngine.Vector3;
using static Hawksbill.Sequencing.SplineData;
using Hawksbill.Geometry;
using System.Linq;
using Hawksbill.Analytics;
using System.Collections.Generic;
using Hawksbill.Sequencing.SplineEdit;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill.Sequencing
{
    ///<summary>Put text here to describe the Class</summary>
    [RequireComponent (typeof (SplinePlayable))]
    public class SplineGroup : SplineExtension
    {
        const float PreviewRear = 0.05f, PreviewForward = 0.2f, PreviewStep = 0.015f;
        [Range (1, 100)] public int count = 16;
        [Range (0, 1000000000)] public uint seed = 1000;
        [Line]
        [Range (0, 1)] public float deviation = 0f;
        [Range (0, 15)] public float seperation = 0f;
        [Range (0.0001f, 0.05f)] public float frequency = 0.01f;
        [Range (0, 50)] public float length = 2f;
        public V3 scale = V3.one;
        [Line]
        public V3 sizeVariate = V3.zero;
        [Line]
        [Range (1, 10)] public int seperationIterations = 5;
        public bool hide;

        Point[] _points;
        public Point[] points => _points = calculatePoints (playable.position, _points);
        public IEnumerable<TransformBase> transforms => points.Select (p => p.transform);

        internal override void OnValidate()
        {
            base.OnValidate ();
            _points = null;
        }

        Point[] calculatePoints(float f, Point[] points = null)
        {
            Profiler.Start ("CPoints");
            points = getPoints (points);
            points.ForAll (p => p.transform.setPositionAndRotation (getPosition (f, p), Quaternion.LookRotation (getDirection (f, p))));
            applySeperation (points);
            var collider = GetComponent<SplineCollider> ();
            if (collider) points.ForAll (p => collider.collide (p.transform));
            Profiler.Stop ("CPoints");
            return points;
        }

        /// <summary>For preview of gizmos</summary>
        V3[] calculatePointsQ(float f, Point[] points, bool rotations = true)
        {
            points.ForAll (p => p.transform.position = getPosition (f, p));
            applySeperation (points);
            return points.Select (p => p.transform.position).ToArray ();
        }

        Point[] getPoints(Point[] points = null)
        {
            if (points == null || points.Length == 0)
            {
                var rnd = new Rnd (seed);
                //float deviationStep = ;
                points = Enumerable.Range (0, count).Select (i => new Point (this, rnd, i)).ToArray ();
            }
            return points;
        }

        void applySeperation(Point[] points)
        {
            if (seperation <= 0) return;
            for (int i = seperationIterations; i > 0; --i)
            {
                for (int i1 = 0; i1 < count; i1++)
                {
                    Point c1 = points[i1];
                    for (int i2 = i1 + 1; i2 < count; i2++)
                    {
                        Point c2 = points[i2];
                        V3 d = c2.transform.position - c1.transform.position;
                        float dist = d.sqrMagnitude;
                        if (dist < seperation * seperation * 4)
                        {
                            V3 delta = d.normalized * ((seperation - Mathf.Sqrt (dist) * 0.5f));
                            c1.transform.position -= delta;
                            c2.transform.position += delta;
                        }
                    }
                }
            }
        }

        V3 noise(V3 v, V3 offset) => Vector3.Scale (Noise.Perlin (v, frequency, offset), scale * length);

        V3 getPosition(float f, Point p)
        {
            f += p.deviation;
            TransformBase t = playable.getTransformAt (f);
            return t.position + t.rotation * noise (t.position, p.offset);
        }

        V3 getDirection(float f, Point p) => getPosition (f + RotationDistance, p) - getPosition (f - RotationDistance, p);

        public class Point
        {
            public float unit, deviation;
            public Vector3 offset;
            public int index;
            [HideInInspector] public TransformBase transform;

            public bool invalid => transform.scale.x == 0;
            public int getFrameIndex(int frameCount) => (int) (frameCount * this.unit);
            public Point(SplineGroup group, Rnd rnd, int index)
            {
                this.unit = rnd.value;
                this.offset = rnd.value3 * 100;
                this.index = index;
                this.deviation = index * ((group.deviation * 2) / group.count) - group.deviation;
                this.transform = new TransformBase (V3.zero, Quaternion.identity, rnd.value3 * (float3) group.sizeVariate + new float3 (1));
            }
        }

        void OnDrawGizmosSelected()
        {
            if (hide || !data.hasNodes) return;
#if UNITY_EDITOR
            float f = playable.position;

            var points = calculatePoints (f);
            foreach (var point in points)
            {
                SplineDraw.DrawPosition (point.transform.position, data.color);
                if (seperation > 0) Gizmos.DrawWireSphere (point.transform.position, seperation);
            }

            Handles.color = new Color (data.color.r, data.color.g, data.color.b, data.color.a * 0.25f);
            if (count <= 30 && Camera.current.isPositionVisible (playable.transform.position, 50, 0.2f))
            {
                var groups = EnumerableUtility.Range (f - PreviewRear, f + PreviewForward, PreviewStep).Select (f => calculatePointsQ (f, points)).ToArray ();
                for (int i = 0; i < count; i++)
                    Handles.DrawPolyLine (groups.Select (path => path[i]).ToArray ());
            }
            else points.ForAll (p => Handles.DrawLine (p.transform.position, p.transform.position + p.transform.forward));

#endif
        }
    }
}