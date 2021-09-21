// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 11/01/2021 08:11:55 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    [CustomEditor (typeof (HierarchyReport))]
    public class HierarchyReport_Editor : Editor
    {
        HierarchyReport hierarchyReport => (target as HierarchyReport);
        GameObject gameObject => hierarchyReport.gameObject;

        // IEnumerable<Type> types => hierarchyReport.types.Select (s => GetType (s)).Where (t => t != null);

        //new Type[] { typeof (Camera), typeof (Light), typeof (Collider), typeof (Projector), typeof (MeshRenderer), };
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector ();
            EditorGUILayout.BeginVertical ("box");
            EditorGUILayout.IntField ("Total Transforms:", gameObject.GetComponentsInChildren<Transform> ().Length);
            GUILayout.Label ("Report:");
            hierarchyReport.types.ForAll (t => drawItems (t));
            EditorGUILayout.EndVertical ();
        }

        static HashSet<string> foldouts = new HashSet<string> ();

        void drawItems(string stype)
        {
            try
            {
                var type = GetType (stype);
                var items = gameObject.GetComponentsInChildren (type);

                EditorGUILayout.BeginVertical ();

                if (EditorGUILayout.Foldout (foldouts.Contains (stype), stype + " (" + items.Length + ")")) foldouts.Add (stype);
                else foldouts.Remove (stype);

                var rect = GUILayoutUtility.GetLastRect ();

                var w = rect.width;
                rect.width = rect.height * 1.2f;
                rect.x += w - rect.width;


                if (GUI.Button (rect, "⊙"))
                    Selection.instanceIDs = items.Select (i => i.GetInstanceID ()).ToArray ();
                EditorGUILayout.EndVertical ();

                if (foldouts.Contains (stype))
                {
                    EditorGUILayout.IntField ("Count", items.Length);
                    foreach (var item in items) EditorGUILayout.ObjectField (item, type, false);
                }
            }
            catch (Exception x)
            {
                var color = GUI.color;
                GUI.color = new Color (1, 0.4f, 0.4f);
                GUILayout.Label ("'" + stype + "' " + "Type is not valid!\n" + x);
                GUI.color = color;
            }
        }

        static Dictionary<string, Type> _ComponentTypes;
        static Dictionary<string, Type> ComponentTypes => _ComponentTypes == null ? _ComponentTypes =
            Searcher.GetTypesImplementing<UnityEngine.Component> ().ToDictionary (t => t.FullName, t => t) : _ComponentTypes;

        public static Type GetType(string name) => ComponentTypes[name];
    }
}