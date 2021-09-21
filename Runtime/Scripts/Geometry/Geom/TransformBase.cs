// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 18/03/2021 17:22:33 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Linq;

namespace Hawksbill.Geometry
{
    [Serializable]
    public class TransformBase
    {
        public Vector3 position;
        public Vector3 eulerAngles;
        public Vector3 scale = Vector3.one;

        public TransformBase() { }
        public TransformBase(Matrix4x4 matrix)
        {
            this.position = new Vector3 (matrix[0, 3], matrix[1, 3], matrix[2, 3]);
            this.rotation = matrix.rotation;
            this.scale = matrix.lossyScale;
        }
        public TransformBase(Vector3 position)
        {
            this.position = position;
        }

        public TransformBase(Vector3 position, Vector3 eulerAngles)
        {
            this.position = position;
            this.eulerAngles = eulerAngles;
        }

        public TransformBase(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public TransformBase(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public TransformBase(Transform transform)
        {
            this.position = transform.position;
            this.rotation = transform.rotation;
            this.scale = transform.lossyScale;
        }

        public void setPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        // Copy functions
        public void copyTo(Transform transform)
        {
            transform.SetPositionAndRotation (position, rotation);
            transform.localScale = scale;
        }

        public static TransformBase Lerp(TransformBase t1, TransformBase t2, float t) =>
            new TransformBase (
                Vector3.Lerp (t1.position, t2.position, t),
                Quaternion.Lerp (t1.rotation, t2.rotation, t),
                Vector3.Lerp (t1.scale, t2.scale, t)
            );

        public static implicit operator TransformBase(Transform t) => new TransformBase (t);

        public Vector3 forward => rotation * Vector3.forward;
        public Vector3 back => rotation * Vector3.back;
        public Vector3 right => rotation * Vector3.right;
        public Vector3 left => rotation * Vector3.left;
        public Vector3 up => rotation * Vector3.up;
        public Vector3 down => rotation * Vector3.down;

        public Vector3 inverseScale => new Vector3 (1 / scale.x, 1 / scale.y, 1 / scale.z);
        public Quaternion rotation { get => Quaternion.Euler (eulerAngles); set => eulerAngles = value.eulerAngles; }
        public Matrix4x4 matrix => Matrix4x4.TRS (position, rotation, scale);
        public Matrix4x4 inverseMatrix => matrix.inverse;
        public Matrix4x4 matrixTR => Matrix4x4.TRS (position, rotation, Vector3.one);
        public Matrix4x4 matrixR => Matrix4x4.Rotate (rotation);
        public Matrix4x4 matrixRS => Matrix4x4.TRS (Vector3.zero, rotation, scale);

        // Transform functions
        public Vector3 multiply(Vector3 v) => matrix.MultiplyPoint (v);
        public Vector3 multiplyVector(Vector3 v) => matrix.MultiplyVector (v);
        public Vector3[] multiply(Vector3[] v) { var m = matrix; return v.Select (p => m.MultiplyPoint (p)).ToArray (); }
        public Quaternion multiply(Quaternion q) => Quaternion.Euler (q.eulerAngles + eulerAngles);
        public Vector3 inverseMultiply(Vector3 v) => inverseMatrix.MultiplyPoint (v);
        public Vector3 inverseMultiplyVector(Vector3 v) => inverseMatrix.MultiplyVector (v);
        public Quaternion inverseMultiply(Quaternion q) => Quaternion.Euler (q.eulerAngles - eulerAngles);

        public static implicit operator bool(TransformBase empty) => empty != null;
    }
}