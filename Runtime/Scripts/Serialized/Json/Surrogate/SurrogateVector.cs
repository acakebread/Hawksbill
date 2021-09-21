// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Hawksbill.Serialization.Json
{
    public class SurrogateVector3 : ISurrogate
    {
        public Type type => typeof (Vector3);
        public object getObjectData(object obj, SurrogateSelector selector)
        {
            Vector3 v = (Vector3) obj;
            return new float[] { v.x, v.y, v.z };
        }

        public object setObjectData(object obj, SurrogateSelector selector)
        {
            var v = (float[]) obj;
            return new Vector3 (v[0], v[1], v[2]);
        }
    }

    public class SurrogateVector2 : ISurrogate
    {
        public Type type => typeof (Vector2);
        public object getObjectData(object obj, SurrogateSelector selector)
        {
            Vector2 v = (Vector2) obj;
            return new float[] { v.x, v.y };
        }

        public object setObjectData(object obj, SurrogateSelector selector)
        {
            var v = (float[]) obj;
            return new Vector2 (v[0], v[1]);
        }
    }

    public class SurrogateVector4 : ISurrogate
    {
        public Type type => typeof (Vector4);
        public object getObjectData(object obj, SurrogateSelector selector)
        {
            Vector4 v = (Vector4) obj;
            return new float[] { v.x, v.y, v.z, v.w };
        }

        public object setObjectData(object obj, SurrogateSelector selector)
        {
            var v = (float[]) obj;
            return new Vector4 (v[0], v[1], v[2], v[3]);
        }
    }
}