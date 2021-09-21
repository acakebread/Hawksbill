// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 09/02/2021 09:00:27 by seantcooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hawksbill
{
    public class AssetFinder : EditorWindow
    {
        [MenuItem ("Hawksbill/Window/Asset Finder")]
        static void OpenWindow()
        {
            AssetFinder window = GetWindow<AssetFinder> ();
            window.titleContent = new GUIContent ("Asset Finder");
            window.minSize = new Vector2 (window.minSize.x, 24);
        }

        protected void OnEnable() => (data = new Data ()).load ();
        protected void OnDisable() => data.save ();

        public static Data data;

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck ();
            data.searchText = EditorGUILayout.TextField (data.searchText);
            if (EditorGUI.EndChangeCheck ()) EditorApplication.RepaintProjectWindow ();
        }

        [Serializable]
        public class Data : Prefs<Data>
        {
            public string searchText = "";
        }

        public class Prefs<T>
        {
            string path => "Hawksill.editor.prefs." + typeof (T).FullName;
            public void load()
            {
                var data = EditorPrefs.GetString (path, JsonUtility.ToJson (this, false));
                JsonUtility.FromJsonOverwrite (data, this);
            }
            public void save()
            {
                var data = JsonUtility.ToJson (this, false);
                EditorPrefs.SetString (path, data);
            }
        }
    }
}