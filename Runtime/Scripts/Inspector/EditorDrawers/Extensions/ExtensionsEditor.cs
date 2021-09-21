// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/08/2021 18:05:52 by seantcooper
#if UNITY_EDITOR
using UnityEngine;
using Hawksbill.Reflection;
using System;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace Hawksbill
{
    public class ExtensionsEditor<T> : Hawksbill.Editor where T : Component
    {
        // Extensions<T> extensions = new Extensions<T> ();
        protected Component component => target as Component;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI ();
            draw ();
        }

        Type[] extensions = AssemblyX.GetAllTypesInheriting (typeof (T)).
            Where (t => !t.IsAbstract && !t.IsDefined (typeof (ExtensionExcludeAttribute), false)).ToArray ();

        internal void draw()
        {
            EditorGUILayout.BeginVertical ("box");
            GUILayout.Label ("Extensions", EditorStyles.boldLabel);
            var extensions = this.extensions.Where (e => component.GetComponent (e) == null).ToArray ();
            var names = new string[] { "(select)" }.Concat (extensions.Select (e => e.Name)).ToArray ();
            int index = EditorGUILayout.Popup ("Add Extension", 0, names);
            if (index != 0) addExtension (extensions[index - 1]);
            EditorGUILayout.EndVertical ();
        }

        public virtual void addExtension(Type extension)
        {
            // Debug.Log ("Selection.gameObjects.Length " + Selection.gameObjects.Length);
            Selection.gameObjects.Where (g => g.GetComponent (target.GetType ())).ForAll (g => g.AddComponent (extension));
        }
    }


    // public static class ReflectiveEnumerator
    // {
    //     static ReflectiveEnumerator() { }

    //     public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class, IComparable<T>
    //     {
    //         List<T> objects = new List<T> ();
    //         foreach (Type type in
    //             Assembly.GetAssembly (typeof (T)).GetTypes ()
    //             .Where (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf (typeof (T))))
    //         {
    //             objects.Add ((T) Activator.CreateInstance (type, constructorArgs));
    //         }
    //         objects.Sort ();
    //         return objects;
    //     }
    // }
}
#endif
