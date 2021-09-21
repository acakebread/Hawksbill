// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using Hawksbill.Geometry;
using V3 = UnityEngine.Vector3;
using System;
using System.Linq;

namespace Hawksbill.Sequencing
{
    [RequireComponent (typeof (SplinePlayable))]
    public class SplineGroundRaycast : SplineCollider //, ISplineExTransformable
    {
        public LayerMask layer;
        [ObjectColumns (12)] public SizeXZ size;
        [Line]
        public bool hide = false;

        public override void collide(TransformBase transform)
        {
            if (layer == 0) return;
            if (size.x > 0) throw new Exception ("Width not implemented!");
            if (size.z > 0)
            {
                transform.eulerAngles.x = 0;
                V3 f = transform.rotation * new V3 (0, 0, size.z);
                if (raycast (transform.position - f, out RaycastHit hit1) && raycast (transform.position + f, out RaycastHit hit2))
                {
                    transform.eulerAngles.x = Mathf.DeltaAngle (Quaternion.LookRotation (hit1.point - hit2.point).eulerAngles.x, 0);
                    transform.position = (hit1.point + hit2.point) / 2;
                }
            }
            else
            {
                V3 p = transform.position;
                if (raycast (p, out RaycastHit hit))
                    transform.position.y = hit.point.y;
            }
        }

        bool raycast(V3 position, out RaycastHit hit)
        {
            if (!hide) Debug.DrawRay (position, V3.down, Color.green);
            return Physics.Raycast (position + V3.up * 100, V3.down, out hit, 200, layer);
        }

        [Serializable]
        public struct SizeXZ
        {
            public float x;
            public float z;
        }
    }
}