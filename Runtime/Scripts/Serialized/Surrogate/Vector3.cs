// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using System.Runtime.Serialization;

namespace Hawksbill.Serialization
{
    internal sealed class Vector3_SerializationSurrogate : ISurrogate
    {
        public void AddToSelector(SurrogateSelector selector) =>
            selector.AddSurrogate (typeof (Vector3), new StreamingContext (StreamingContextStates.All), this);

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 v = (Vector3) obj;
            info.AddValue ("x", v.x);
            info.AddValue ("y", v.y);
            info.AddValue ("z", v.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 v = (Vector3) obj;
            v.x = info.GetSingle ("x");
            v.y = info.GetSingle ("y");
            v.z = info.GetSingle ("z");
            return v;
        }
    }
}
