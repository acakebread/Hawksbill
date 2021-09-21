// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 08/02/2021 08:48:23 by seantcooper
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Hawksbill
{
    [UnityEditor.InitializeOnLoad]
    public class AssetDrawer : Editor
    {
        static AssetDrawer()
        {
            EditorApplication.projectWindowItemOnGUI += projectWindowItemOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += hierarchyWindowItemOnGUI;
        }

        static void hierarchyWindowItemOnGUI(int instanceID, Rect rect)
        {
            if (rect.height > 20) return;
            drawMarkers (rect, EditorUtility.InstanceIDToObject (instanceID) as GameObject);
        }

        static void projectWindowItemOnGUI(string guid, Rect rect)
        {
            if (rect.height > 20) return;
            if (AssetCache.LoadPrefab (guid, out AssetCache.Info info))
                drawMarkers (rect, info.obj as GameObject);
        }

        static void drawMarkers(Rect rect, GameObject gameObject)
        {
            if (null == AssetFinder.data) return;
            if (gameObject == null) return;
            //GUILayoutOption width = GUILayout.MaxWidth (rect.height * 1.2f), height = GUILayout.MaxHeight (rect.height);
            var line = new Rect (0, rect.y, Screen.width - 18, rect.height);
            var marker = new Rect (line.x + 1, line.y + 1, (line.height - 2) / 2, line.height - 2);

            if (SearchComponents (AssetFinder.data.searchText, gameObject))
            {
                EditorGUI.DrawRect (marker, new Color (1, 0.65f, 0, 0.5f));
                marker.x += marker.width + 1;
            }
        }

        static bool SearchComponents(string search, GameObject gameObject)
        {
            if (String.IsNullOrEmpty (search) || !gameObject) return false;

            var components = gameObject.activeInHierarchy ?
                gameObject.GetComponents<UnityEngine.Component> () :
                gameObject.GetComponentsInChildren<UnityEngine.Component> (true);

            var text = String.Join ("$", components.Where (c => c).Select (c => c.GetType ().FullName));
            return text.IndexOf (search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        static GUIStyle _ComponentCount;
        static GUIStyle ComponentCount => _ComponentCount == null ?
            new GUIStyle (GUI.skin.label) { fontSize = 8, alignment = TextAnchor.MiddleRight } : _ComponentCount; //normal = { background = TextureUtility.GetColor (0, 1, 0, 0.15f) }

    }
}

// var counter = new Rect (line.xMax - rect.height * 1.2f, rect.y, rect.height * 1.2f, rect.height);
// GUI.Label (counter, prefab.GetComponentsInChildren<Transform> ().Length.ToString (), ComponentCount);
