// using System;
// using System.Collections;
// using System.Linq;
// using System.Reflection;
// using UnityEditor;
// using UnityEngine;
// using System.IO;
// using System.Runtime.Serialization.Formatters.Binary;

// public static class SerializedHelper
// {
// #if UNITY_EDITOR

//     // public static SerializedProperty FindPropertyFull(this SerializedObject obj, string path)
//     // {
//     //     var elements = path.Split ('.');
//     //     SerializedProperty prop = null;
//     //     if (elements.Length > 1)
//     //     {
//     //         Debug.Log ("Relative");
//     //     }
//     //     foreach (var element in elements)
//     //         if (prop == null)
//     //             prop = obj.FindProperty (element);
//     //         else
//     //             prop = prop.FindPropertyRelative (element);
//     //     return prop;
//     // }

//     public static Type getFieldType(this SerializedProperty prop) => prop.getFieldInfo ().FieldType;
//     public static FieldInfo getFieldInfo(this SerializedProperty prop)
//     {
//         System.Type currentType = prop.serializedObject.targetObject.GetType ();
//         FieldInfo info = null;
//         foreach (var part in prop.propertyPath.Split ('.'))
//             currentType = (info = currentType.GetField (part)).FieldType;
//         return info;
//     }

//     public static bool hasAttribute<T>(SerializedProperty prop1, SerializedProperty prop2) where T : Attribute
//     {
//         T attr = SerializedHelper.getAttribute<T> (prop1) as T;
//         if (attr == null) attr = SerializedHelper.getAttribute<T> (prop2) as T;
//         return attr != null;
//     }

//     public static T getAttribute<T>(SerializedProperty prop) where T : Attribute
//     {
//         var info = getFieldInfo (prop);
//         if (info != null)
//         {
//             T attribute = info.GetCustomAttribute (typeof (T)) as T;
//             return attribute;
//         }

//         return null;
//     }

//     public static object GetParent(this SerializedProperty prop)
//     {
//         var path = prop.propertyPath.Replace (".Array.data[", "[");
//         object obj = prop.serializedObject.targetObject;
//         var elements = path.Split ('.');
//         foreach (var element in elements.Take (elements.Length - 1))
//         {
//             if (element.Contains ("["))
//             {
//                 var elementName = element.Substring (0, element.IndexOf ("["));
//                 var index = Convert.ToInt32 (element.Substring (element.IndexOf ("[")).Replace ("[", "").Replace ("]", ""));
//                 obj = GetValue (obj, elementName, index);
//             }
//             else
//             {
//                 obj = GetValue (obj, element);
//             }
//         }
//         return obj;
//     }

//     public static object GetObject(this SerializedProperty prop)
//     {
//         var path = prop.propertyPath.Replace (".Array.data[", "[");
//         object obj = prop.serializedObject.targetObject;
//         foreach (var element in path.Split ('.'))
//         {
//             if (element.Contains ("["))
//             {
//                 var elementName = element.Substring (0, element.IndexOf ("["));
//                 var index = Convert.ToInt32 (element.Substring (element.IndexOf ("[")).Replace ("[", "").Replace ("]", ""));
//                 obj = GetValue (obj, elementName, index);
//             }
//             else
//             {
//                 obj = GetValue (obj, element);
//             }
//         }
//         return obj;
//     }

//     public static object GetValue(object source, string name)
//     {
//         if (source == null)
//             return null;
//         var type = source.GetType ();
//         var f = type.GetField (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
//         if (f == null)
//         {
//             var p = type.GetProperty (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
//             if (p == null)
//                 return null;
//             return p.GetValue (source, null);
//         }
//         return f.GetValue (source);
//     }

//     public static object GetValue(object source, string name, int index)
//     {
//         var enumerable = GetValue (source, name) as IEnumerable;
//         var enm = enumerable.GetEnumerator ();
//         while (index-- >= 0)
//             enm.MoveNext ();
//         return enm.Current;
//     }

// #endif

//     public static object Clone(System.Object source)
//     {
//         using (MemoryStream stream = new MemoryStream ())
//         {
//             if (source.GetType ().IsSerializable)
//             {
//                 BinaryFormatter formatter = new BinaryFormatter ();
//                 formatter.Serialize (stream, source);
//                 stream.Position = 0;
//                 return formatter.Deserialize (stream);
//             }
//             return null;
//         }
//     }
// }