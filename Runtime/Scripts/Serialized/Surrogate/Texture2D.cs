// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using System.Runtime.Serialization;

namespace Hawksbill.Serialization
{
    internal sealed class Texture2D_SerializationSurrogate : ISurrogate
    {
        public void AddToSelector(SurrogateSelector selector) =>
            selector.AddSurrogate (typeof (Texture2D), new StreamingContext (StreamingContextStates.All), this);

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Texture2D texture = (Texture2D) obj;
            if (texture)
            {
                if (!texture.isReadable)
                    texture = texture.CreateReadable ();
                info.AddValue ("data", texture.EncodeToJPG ());
            }
            else info.AddValue ("data", null);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Texture2D texture = new Texture2D (2, 2);
            texture.LoadImage ((byte[]) info.GetValue ("data", typeof (byte[])));
            return texture;
        }
    }
}


