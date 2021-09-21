// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hawksbill.IO;

namespace Hawksbill
{
    // public class CreateClassEditor : EditorWindow
    // {
    //     [MenuItem ("Assets/Create/C# Class with Namspace", true)]
    //     private static bool CreateClassValidation() => !IsSelectedPathEmpty;
    //     [MenuItem ("Assets/Create/C# Class with Namspace", false, 0)]
    //     private static void _CreateClass()
    //     {
    //         CreateClassEditor window = ScriptableObject.CreateInstance<CreateClassEditor> ();
    //         //window.position = new Rect (Screen.width / 2, Screen.height / 2, 250, 150);
    //         window.ShowPopup ();

    //         // var paths = new string[] { "Audio", "Materials", "Models", "Prefabs", "Scenes", "Scripts", "Textures" };
    //         // if (EditorUtility.DisplayDialog ("Asset Folders", "Are you sure you want to create all assets folders: " + String.Join (",", paths) + "?", "Yes", "No"))
    //         // {
    //         //     var folder = SelectedPath.folderPath;
    //         //     foreach (var path in paths)
    //         //     {
    //         //         if ((folder + path).exists) continue;
    //         //         Debug.Log ("Creating Folder: " + folder + "/" + path);
    //         //         AssetDatabase.CreateFolder (folder.assetPath, path);
    //         //     }
    //         // }
    //     }

    //     void OnGUI()
    //     {
    //         EditorGUILayout.TextField ("Namespace", "Hawksbill");
    //         EditorGUILayout.TextField ("Class Name", "");
    //         GUILayout.Space (70);
    //         EditorGUILayout.BeginHorizontal ();
    //         if (GUILayout.Button ("Cancel"))
    //         {
    //             this.Close ();
    //         }
    //         if (GUILayout.Button ("Ok"))
    //         {
    //             this.Close ();
    //         }
    //         EditorGUILayout.EndHorizontal ();
    //     }

    //     static Path SelectedPath => new Path (Application.dataPath + AssetDatabase.GetAssetPath (Selection.activeObject).Substring ("assets".Length));
    //     static bool IsSelectedPathEmpty => AssetDatabase.GetAssetPath (Selection.activeObject) == "";
    // }
}
