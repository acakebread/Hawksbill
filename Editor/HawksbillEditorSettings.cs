// // Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 17:58:10 by seancooper
// using UnityEngine;
// using UnityEditor;
// using System.Collections.Generic;
// using System;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using System.Text.RegularExpressions;

// namespace Hawksbill
// {
//     // [HelpURL ("This is a url")]
//     class HawksbillEditorSettings : ScriptableObject
//     {
//         const string pathToAsset = "Assets/Editor/Hawksbill/";
//         const string assetPath = pathToAsset + "settings.asset";

//         [InspectorName ("This is the settings 1")]
//         [SerializeField] [Tooltip ("This is the setting 1")] internal bool setting1 = false;
//         [SerializeField] internal bool setting2 = false;
//         [SerializeField] internal bool setting3 = false;
//         [Header ("Local settings")]
//         [SerializeField] internal bool setting4 = false;

//         [SerializeField] internal bool[] group = new bool[6];

//         [Delayed]
//         [ContextMenuItem ("RandomValue", "RandomizeValueFromRightClick")]
//         [SerializeField]
//         private float randomValue;

//         private void RandomizeValueFromRightClick()
//         {
//             randomValue = UnityEngine.Random.Range (-5f, 5f);
//         }

//         internal static HawksbillEditorSettings GetOrCreateSettings()
//         {
//             var settings = AssetDatabase.LoadAssetAtPath<HawksbillEditorSettings> (assetPath);
//             if (settings == null)
//             {
//                 settings = ScriptableObject.CreateInstance<HawksbillEditorSettings> ();
//                 Directory.CreateDirectory (pathToAsset);
//                 AssetDatabase.CreateAsset (settings, assetPath);
//                 AssetDatabase.SaveAssets ();
//             }
//             return settings;
//         }
//         internal static SerializedObject GetSerializedSettings() => new SerializedObject (GetOrCreateSettings ());
//     }

//     static class HawksbillSettingsProvider
//     {
//         static SerializedObject settings;
//         private class Provider : SettingsProvider
//         {
//             public Provider(string path, SettingsScope scope = SettingsScope.User) : base (path, scope) { }
//             public override void OnGUI(string searchContext) => DrawGUI ();
//         }

//         [SettingsProvider] static SettingsProvider HawksbillPref() => new Provider ("Preferences/Hawksbill");

//         static void DrawGUI()
//         {
//             const float Width = 640;

//             var editor = Editor.CreateEditor (HawksbillEditorSettings.GetOrCreateSettings ());

//             var indent = EditorGUI.indentLevel;
//             EditorGUILayout.BeginVertical ("box", GUILayout.MaxWidth (Width));
//             EditorGUI.indentLevel++;
//             EditorGUIUtility.labelWidth = Width / 2;
//             editor.OnInspectorGUI ();
//             EditorGUI.indentLevel = indent;
//             EditorGUILayout.EndVertical ();

//             if (GUI.changed)
//             {
//                 settings.ApplyModifiedProperties ();
//                 AssetDatabase.SaveAssets ();
//                 Debug.Log ("Changed");
//             }
//         }
//     }
// }
