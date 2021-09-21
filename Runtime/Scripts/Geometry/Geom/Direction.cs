// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 09/06/2021 18:22:47 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill.Geometry
{
    ///<summary>Direction vector</summary>
    public struct Direction
    {
        public Vector3 vector;
        public Vector3 normalize => vector.normalized;
        public float length => vector.magnitude;
        public Quaternion rotation => Quaternion.LookRotation (vector);
        public Direction(Vector3 vector)
        {
            this.vector = vector;
        }
        public static implicit operator Vector3(Direction d) => d.vector;
        public static implicit operator Direction(Vector3 v) => new Direction (v);
    }
}