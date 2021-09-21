// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using Hawksbill.Geometry;
using V3 = UnityEngine.Vector3;
using System;
using System.Linq;

namespace Hawksbill.Sequencing
{
    [RequireComponent (typeof (SplinePlayable))]
    public class SplineNoise : SplineExtension, ISplineExTransformable
    {
        public static bool Enabled = true;

        [Range (0.0001f, 0.2f)] public float frequency = 0.01f;
        [Range (0, 50)] public float length = 2f;
        [Range (1, 1000000000)] public uint seed = 1000;
        public V3 scale = V3.one;
        [HideInInspector, SerializeField] V3 offset;

        internal override void OnValidate()
        {
            base.OnValidate ();
            offset = new Rnd (seed).value3 * 1000;
        }

        static readonly V3 half = V3.one / 2;

        V3 apply(V3 v) => v + Vector3.Scale (Noise.Perlin (v, frequency, offset), scale * length);

        public int priority => 1;
        public V3 transformPosition(V3 v) => Enabled && length > 0 ? apply (v) : v;
        public V3[] transformPositions(V3[] vs) => Enabled && length > 0 ? vs.Select (v => apply (v)).ToArray () : vs.ToArray ();
    }
}