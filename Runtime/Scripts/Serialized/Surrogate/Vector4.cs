// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using System.Runtime.Serialization;

namespace Hawksbill.Serialization
{
    internal sealed class Vector4_SerializationSurrogate : ISurrogate
    {
        public void AddToSelector(SurrogateSelector selector) =>
            selector.AddSurrogate (typeof (Vector4), new StreamingContext (StreamingContextStates.All), this);

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector4 v = (Vector4) obj;
            info.AddValue ("x", v.x);
            info.AddValue ("y", v.y);
            info.AddValue ("z", v.z);
            info.AddValue ("w", v.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector4 v = (Vector4) obj;
            v.x = info.GetSingle ("x");
            v.y = info.GetSingle ("y");
            v.z = info.GetSingle ("z");
            v.w = info.GetSingle ("w");
            return v;
        }
    }
}
