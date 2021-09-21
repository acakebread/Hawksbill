// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Hawksbill.Serialization
{
    public class PersistentObject : ScriptableObject
    {
        [Range (1f, 60f)] public float serializeFrequency = 5;
        protected virtual Persistent serializationData { get; }
        protected Serializer serializer => new Serializer ();

        // // MANAGEMENT
        // void OnEnable()
        // {
        //     Console.Log ("OnEnable", DateTime.Now.Ticks);
        //     if (Application.isPlaying)
        //     {
        //     }
        //     else
        //     {
        //         EditorSceneManager.sceneSaved += (s) => save ();
        //     }
        // }

        // // this called when the Built App quits
        // void OnDisable()
        // {
        //     Console.Log ("OnDisable", Application.isPlaying);
        //     // save to relevent media
        //     save ();
        // }

        // void OnValidate()
        // {
        //     Console.Log ("OnValidate", DateTime.Now.Ticks);
        // }

        // SAVE/LOAD
        public void save()
        {
            Debug.Log ("PersistentObject::Save");
            serializationData.write ();
            serializationData.read ();
        }

        public void load()
        {
            Debug.Log ("PersistentObject::Load");
            //serializer.serialize(serializationData);
        }

        // // SERIALIZATION
        // protected virtual SurrogateSelector getSurrogateSelector()
        // {
        //     var selector = new SurrogateSelector ();
        //     new Vector3_SerializationSurrogate ().AddToSelector (selector);
        //     new Texture2D_SerializationSurrogate ().AddToSelector (selector);
        //     return selector;
        // }

        // protected virtual IFormatter getFormatter() =>
        //     new BinaryFormatter (getSurrogateSelector (), new StreamingContext (StreamingContextStates.All));

        // byte[] serialize()
        // {
        //     using (MemoryStream stream = new MemoryStream ())
        //     {
        //         getFormatter ().Serialize (stream, serializationData);
        //         return stream.ToArray ();
        //     }
        // }

        // void deserialize(byte[] bytes)
        // {
        //     using (MemoryStream stream = new MemoryStream (bytes))
        //         copyFrom (getFormatter ().Deserialize (stream));
        // }

        // void copyFrom(object source)
        // {
        //     foreach (MemberInfo memberInfo in FormatterServices.GetSerializableMembers (serializationData.GetType ()))
        //     {
        //         Debug.Log ("Copy " + memberInfo.MemberType + " " + memberInfo.Name);
        //         if (memberInfo.MemberType == MemberTypes.Field)
        //             (memberInfo as FieldInfo).SetValue (serializationData, (memberInfo as FieldInfo).GetValue (source));
        //         else if (memberInfo.MemberType == MemberTypes.Property)
        //             (memberInfo as PropertyInfo).SetValue (serializationData, (memberInfo as PropertyInfo).GetValue (source));
        //     }
        // }
    }
}