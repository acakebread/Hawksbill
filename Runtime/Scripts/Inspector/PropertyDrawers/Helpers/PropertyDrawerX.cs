// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 07/06/2021 21:27:36 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

namespace Hawksbill
{
    ///<summary>PropertyDrawer Helper functions</summary>
    public static class PropertyDrawerX
    {
        static Func<SerializedProperty, PropertyDrawer> getDrawer;

        public static PropertyDrawer GetDrawer(this SerializedProperty property)
        {
            if (getDrawer == null)
            {
                var mtd = typeof (PropertyDrawer).GetMethod ("GetDrawer", BindingFlags.NonPublic | BindingFlags.Static);
                getDrawer = (Func<SerializedProperty, PropertyDrawer>) Delegate.CreateDelegate (typeof (Func<SerializedProperty, PropertyDrawer>), null, mtd);
            }
            return getDrawer (property);
        }

        ///<summary> Get target from SerializedProperty</summary>
        public static object getTarget(this SerializedProperty prop)
        {
            if (prop == null) return null;

            var path = prop.propertyPath.Replace (".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split ('.');
            foreach (var element in elements)
            {
                if (element.Contains ("["))
                {
                    var elementName = element.Substring (0, element.IndexOf ("["));
                    var index = System.Convert.ToInt32 (element.Substring (element.IndexOf ("[")).Replace ("[", "").Replace ("]", ""));
                    obj = GetValue (obj, elementName, index);
                }
                else
                {
                    obj = GetValue (obj, element);
                }
            }
            return obj;
        }

        static object GetValue(object source, string name)
        {
            if (source == null) return null;

            for (var type = source.GetType (); type != null; type = type.BaseType)
            {
                var f = type.GetField (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null) return f.GetValue (source);

                var p = type.GetProperty (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null) return p.GetValue (source, null);
            }
            return null;
        }

        static object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue (source, name) as IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator ();
            for (int i = 0; i <= index; i++)
                if (!enm.MoveNext ()) return null;
            return enm.Current;
        }

        #region "Get SerializedProperty's parent value"
        const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        public static object getValue(this SerializedProperty property, string member)
        {
            var parent = GetParentObject (property);

            MethodInfo methodInfo = parent.GetType ().GetMethod (member, Flags);
            if (methodInfo == null)
            {
                var propInfo = parent.GetType ().GetProperty (member, Flags);
                if (propInfo == null)
                {
                    var fieldInfo = parent.GetType ().GetField (member, Flags);
                    if (fieldInfo != null) return fieldInfo.GetValue (parent);
                }
                else return propInfo.GetValue (parent);
            }
            else return methodInfo.Invoke (parent, null);
            Debug.LogWarning ("member '" + member + "' not found!");
            return null;
        }

        public static object GetParentObject(SerializedProperty prop)
        {
            var objects = GetObjectsTo (prop).ToArray ();
            return objects.Length > 1 ? objects[objects.Length - 2] : null;
        }

        static IEnumerable<object> GetObjectsTo(SerializedProperty prop)
        {
            if (prop == null) yield break;
            var path = prop.propertyPath.Replace (".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            yield return obj;
            foreach (var element in path.Split ('.'))
            {
                if (element.Contains ("["))
                {
                    var name = element.Substring (0, element.IndexOf ("["));
                    var index = System.Convert.ToInt32 (element.Substring (element.IndexOf ("[")).Replace ("[", "").Replace ("]", ""));
                    obj = GetValue_Imp (obj, name, index);
                    yield return obj;
                }
                else
                {
                    obj = GetValue_Imp (obj, element);
                    yield return obj;
                }
            }
        }

        static object GetValue_Imp(object source, string name)
        {
            if (source == null) return null;
            var type = source.GetType ();
            while (type != null)
            {
                var f = type.GetField (name, Flags);
                if (f != null) return f.GetValue (source);

                var p = type.GetProperty (name, Flags | BindingFlags.IgnoreCase);
                if (p != null) return p.GetValue (source, null);
                type = type.BaseType;
            }
            return null;
        }

        static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp (source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator ();
            for (int i = 0; i <= index; i++)
                if (!enm.MoveNext ()) return null;
            return enm.Current;
        }
        #endregion
    }
}
#endif