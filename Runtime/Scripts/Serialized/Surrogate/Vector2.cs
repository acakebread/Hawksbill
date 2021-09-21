// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using System.Runtime.Serialization;

namespace Hawksbill.Serialization
{
    internal sealed class Vector2_SerializationSurrogate : ISurrogate
    {
        public void AddToSelector(SurrogateSelector selector) =>
            selector.AddSurrogate (typeof (Vector2), new StreamingContext (StreamingContextStates.All), this);

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector2 v = (Vector2) obj;
            info.AddValue ("x", v.x);
            info.AddValue ("y", v.y);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector2 v = (Vector2) obj;
            v.x = info.GetSingle ("x");
            v.y = info.GetSingle ("y");
            return v;
        }
    }
}
