// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Hawksbill.Serialization
{
    public class Serializer
    {
        // SERIALIZATION
        protected virtual SurrogateSelector getSurrogateSelector()
        {
            var selector = new SurrogateSelector ();
            new Quaternion_SerializationSurrogate ().AddToSelector (selector);
            new Vector2_SerializationSurrogate ().AddToSelector (selector);
            new Vector3_SerializationSurrogate ().AddToSelector (selector);
            new Vector4_SerializationSurrogate ().AddToSelector (selector);
            new Texture2D_SerializationSurrogate ().AddToSelector (selector);
            return selector;
        }

        protected virtual IFormatter getFormatter() =>
            new BinaryFormatter (getSurrogateSelector (), new StreamingContext (StreamingContextStates.All));

        internal byte[] serialize(object data)
        {
            using (MemoryStream stream = new MemoryStream ())
            {
                getFormatter ().Serialize (stream, data);
                return stream.ToArray ();
            }
        }

        internal object deserialize(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream (bytes))
                return getFormatter ().Deserialize (stream);
        }

        internal void deserializeOver(byte[] bytes, object target)
        {
            using (MemoryStream stream = new MemoryStream (bytes))
                copyFrom (getFormatter ().Deserialize (stream), target);
        }

        internal void copyFrom(object source, object target)
        {
            foreach (MemberInfo memberInfo in FormatterServices.GetSerializableMembers (target.GetType ()))
            {
                Debug.Log ("Copy " + memberInfo.MemberType + " " + memberInfo.Name);
                if (memberInfo.MemberType == MemberTypes.Field)
                    (memberInfo as FieldInfo).SetValue (target, (memberInfo as FieldInfo).GetValue (source));
                else if (memberInfo.MemberType == MemberTypes.Property)
                    (memberInfo as PropertyInfo).SetValue (target, (memberInfo as PropertyInfo).GetValue (source));
            }
        }
    }
}
