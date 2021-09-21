// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 14/07/2021 10:01:21 by seantcooper
using UnityEngine;
using Hawksbill;
using System;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class GizmosX
    {
        public class Color : IDisposable
        {
            UnityEngine.Color color;
            public Color(UnityEngine.Color color)
            {
                this.color = Gizmos.color;
                Gizmos.color = color;
            }
            public void Dispose() => Gizmos.color = color;
        }

        public class Matrix : IDisposable
        {
            UnityEngine.Matrix4x4 matrix;
            public Matrix(UnityEngine.Matrix4x4 matrix)
            {
                this.matrix = Gizmos.matrix;
                Gizmos.matrix = matrix;
            }
            public void Dispose() => Gizmos.matrix = matrix;
        }
    }
}