// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using Hawksbill.Geometry;
using System.Linq;
using static Hawksbill.Sequencing.SplineData;
using V3 = UnityEngine.Vector3;

namespace Hawksbill.Sequencing
{
    [RequireComponent (typeof (SplinePlayable))]
    public class SplineTransform : SplineExtension, ISplineExTransformable
    {
        public TransformBase splineTransform;

#if UNITY_EDITOR
        [Button] public bool bakeData = false;
        internal override void OnValidate()
        {
            if (bakeData)
            {
                bakeData = false;
                Bake (GetComponent<SplinePlayable> ()?.data, splineTransform.matrix);
                splineTransform = new TransformBase ();
            }
            base.OnValidate ();
        }

        public static void Bake(SplineData data, Matrix4x4 matrix)
        {
            Node transform(Node n)
            {
                V3 cpIn = matrix.MultiplyPoint (n.cpIn), cpOut = matrix.MultiplyPoint (n.cpOut);
                V3 position = matrix.MultiplyPoint (n.position);
                V3 direction = matrix.MultiplyVector (n.rotation * V3.forward);
                return new Node (position, Quaternion.LookRotation (direction).eulerAngles, new Node.Scale ((cpIn - position).magnitude, (cpOut - position).magnitude), n.index);
            }
            data.nodes = data.nodes.Select (n => transform (n)).ToList ();
        }
#endif

        public int priority => 2;
        public V3 transformPosition(V3 v) => splineTransform.matrix.MultiplyPoint (v);
        public V3[] transformPositions(V3[] vs)
        {
            Matrix4x4 m = splineTransform.matrix;
            return vs.Select (v => m.MultiplyPoint (v)).ToArray ();
        }

    }
}