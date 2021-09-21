using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hawksbill.Geometry
{
    [Serializable]
    public struct MinMaxInt
    {
        public int min, max;
        public int range => max - min;
        public MinMaxInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        public int clamp(int v) => Mathf.Clamp (v, min, max);
    }

    [Serializable]
    public struct MinMaxFloat
    {
        public float min, max;
        public float range => max - min;
        public MinMaxFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public float clamp(float v) => Mathf.Clamp (v, min, max);
    }

}
