// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 18/09/2021 09:13:43 by seantcooper
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Hawksbill.Configurator
{
    [CustomEditor (typeof (ConfiguratorBrain), true), CanEditMultipleObjects]
    public class ConfiguratorBrain_Editor : Editor
    {
        static string SearchText;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI ();

            EditorGUILayout.BeginVertical ("box");
            EditorGUILayout.BeginHorizontal ();
            SearchText = GUILayout.TextField (SearchText);
            if (String.IsNullOrEmpty (SearchText))
            {
                using (new GUIHelpers.Enable (false))
                    GUI.Label (GUILayoutUtility.GetLastRect (), "Search for Name or Type?");
            }
            if (GUILayout.Button ("◉", GUILayout.MaxWidth (24)))
            {
                Selection.objects = getSelection ().ToArray ();
            }

            EditorGUILayout.EndHorizontal ();
            getSelectables ().ForAll (drawSelectable);
            EditorGUILayout.EndVertical ();
        }

        IEnumerable<ConfiguratorSelectable> getSelectables() =>
            FindObjectsOfType<ConfiguratorSelectable> ().OrderBy (s => s.transform.getHierarchyOrder ());

        IEnumerable<UnityEngine.Object> getSelection() =>
            getSelectables ().Where (s => searchMatching (s)).Cast<UnityEngine.Component> ().Concat (
            getSelectables ().SelectMany (s => s.GetComponents<ConfiguratorExtension> ()).Cast<UnityEngine.Component> ().Where (e => searchMatching (e))).
            Select (o => o.gameObject).Distinct ();

        void drawSelectable(ConfiguratorSelectable selectable)
        {
            EditorGUILayout.BeginVertical ("box");
            drawObjectField (selectable);
            using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
                selectable.GetComponents<ConfiguratorExtension> ().ForAll (e => drawObjectField (e));
            EditorGUILayout.EndVertical ();
        }

        void drawObjectField(UnityEngine.Object o)
        {
            using (new GUIHelpers.Enable (searchMatching (o)))
                EditorGUILayout.ObjectField ("", o, o.GetType (), true);
        }

        bool searchMatching(UnityEngine.Object o)
        {
            if (String.IsNullOrEmpty (SearchText)) return true;
            var search = SearchText.ToLower ();
            return o.name.ToLower ().IndexOf (search) != -1 || o.GetType ().Name.ToLower ().IndexOf (search) != -1;
        }


    }
}