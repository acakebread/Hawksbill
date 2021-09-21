// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:59 by seancooper
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hawksbill
{
    [CustomEditor (typeof (FolderSync))]
    [CanEditMultipleObjects]
    public class LookAtPointEditor : Editor
    {
        const float BUTTON_SIZE = 20;

        FolderSync folderSync => this.target as FolderSync;
        int iNewer = -1, iDelete = -1, iCopy = -1;

        // public override VisualElement CreateInspectorGUI()
        // {
        //     VisualElement container = new VisualElement ();
        //     return container;
        // }

        bool isOSX => Application.platform == RuntimePlatform.OSXEditor;
        string fileSystemName => isOSX ? "Finder" : "Explorer";

        public override void OnInspectorGUI()
        {

            // paths
            EditorGUILayout.BeginVertical ("box");
            folderSync.sourcePath = drawPath ("Source", folderSync.sourcePath);
            folderSync.destinationPath = drawPath ("Destination", folderSync.destinationPath);
            EditorGUILayout.EndVertical ();

            // buttons
            EditorGUILayout.BeginHorizontal ("box");
            if (GUILayout.Button ("List")) folderSync.build ();
            if (GUILayout.Button ("Sync to Destination")) folderSync.sync ();
            EditorGUILayout.EndHorizontal ();

            // status
            if (folderSync.status != "")
            {
                EditorGUILayout.BeginVertical ("box");
                EditorGUILayout.LabelField (folderSync.status);
                EditorGUILayout.EndVertical ();
            }

            if (drawList ("Newer on destination:", folderSync.newer, ref iNewer)) iDelete = iCopy = -1;
            if (drawList ("Deleting from destination:", folderSync.delete, ref iDelete)) iNewer = iCopy = -1;
            if (drawList ("Copy from source to destination:", folderSync.copy, ref iCopy)) iDelete = iNewer = -1;
        }

        string getPath(string title, string path)
        {
            var p = EditorUtility.OpenFolderPanel (title, path, path);
            return p == "" ? path : p;
        }

        string drawPath(string title, string path)
        {
            EditorGUILayout.BeginHorizontal ();
            EditorGUILayout.TextField (title, path);
            if (GUILayout.Button ("...", GUILayout.MaxWidth (BUTTON_SIZE))) path = getPath (title, path);
            EditorGUILayout.EndHorizontal ();
            return path;
        }

        bool drawList(string title, string[] paths, ref int selected)
        {
            if (paths == null) paths = new string[] { };
            bool change = false;
            EditorGUILayout.BeginVertical ("box");
            EditorGUILayout.LabelField ("(" + paths.Length + ") " + title);
            for (int i = 0; i < paths.Length; i++)
            {
                if (GUILayout.Button (paths[i], i == selected ? styles.selected : styles.normal))
                {
                    if (selected != i) change = true;
                    selected = i;
                    showMenu (paths[i]);
                }
            }
            EditorGUILayout.EndVertical ();
            return change;
        }

        void revealInFinder(string path)
        {
            EditorUtility.RevealInFinder (folderSync.sourcePath + path);
            EditorUtility.RevealInFinder (folderSync.destinationPath + path);
        }

        void showMenu(string path)
        {
            if (Event.current.button != 1) return;
            GenericMenu menu = new GenericMenu ();
            menu.AddItem (new GUIContent ("Reveal file in " + fileSystemName), false, () => revealInFinder (path));
            //menu.AddItem (new GUIContent ("Remove file from sync"), false, () => { });
            menu.ShowAsContext ();
        }

        static Styles _styles;
        static Styles styles => _styles == null ? _styles = new Styles () : _styles;
        public class Styles
        {
            public GUIStyle normal;
            public GUIStyle selected;
            public Styles()
            {
                Debug.Log ("Get styles");
                normal = new GUIStyle (GUI.skin.label);
                selected = new GUIStyle (GUI.skin.label);
                selected.normal.background = getColor (GUI.skin.settings.selectionColor);
            }

            Texture2D getColor(Color color)
            {
                Texture2D texture = new Texture2D (1, 1);
                texture.SetPixels (0, 0, 1, 1, new Color[] { color });
                texture.Apply ();
                return texture;
            }
        }
    }
}