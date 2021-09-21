// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 26/08/2021 16:27:17 by seantcooper
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill.Configurator
{
    public class ProjectSettings : ScriptableObject
    {
        public const string PathToAsset = "Assets/Resources/Settings/Configurator/";
        public const string AssetPath = PathToAsset + "settings.asset";
        public static string ResourcePath => AssetPath.Substring ("Assets/Resources/".Length);

        static ProjectSettings _Instance;
        public static ProjectSettings Instance => _Instance ? _Instance : _Instance = Load ();

        //-----------------------------------------
        #region "Serialized Data"

        public CinemachineSettings cinemachine;
        public static CinemachineSettings Cinemachine => Instance.cinemachine;
        [Serializable]
        public class CinemachineSettings
        {
            public Cinemachine.CinemachineVirtualCamera defaultCamera;
            public Cinemachine.CinemachineBlenderSettings defaultCustomBlends;
        }

        [Line]
        public MediaSettings media;
        public static MediaSettings Media => Instance.media;
        [Serializable]
        public class MediaSettings
        {
            public Texture2DList playControls;
        }

        [Line]
        public EditorSettings editor;
        public static EditorSettings Editor => Instance.editor;
        [Serializable]
        public class EditorSettings
        {
            public Material defaultSelectionMaterial;
        }

        #endregion
        //-----------------------------------------

        // System
        static ProjectSettings Load()
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<ProjectSettings> (AssetPath);
#else
            return Resources.Load<ProjectSettings> (ResourcePath);
#endif
        }

#if UNITY_EDITOR
        public static ProjectSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<ProjectSettings> (AssetPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ProjectSettings> ();
                System.IO.Directory.CreateDirectory (PathToAsset);
                AssetDatabase.CreateAsset (settings, AssetPath);
                AssetDatabase.SaveAssets ();
            }
            return settings;
        }
        internal static SerializedObject GetSerializedSettings() => new SerializedObject (GetOrCreateSettings ());
#endif
    }

#if UNITY_EDITOR
    [CustomEditor (typeof (ProjectSettings))]
    public class SplineDataEditor : Editor
    {
        static readonly string[] _dontIncludeMe = new string[] { "m_Script" };
        public override void OnInspectorGUI() => draw ();
        void draw()
        {
            serializedObject.Update ();
            using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
                DrawPropertiesExcluding (serializedObject, _dontIncludeMe);
            serializedObject.ApplyModifiedProperties ();
        }
    }

    static class ProjectSettings_Provider
    {
        static SerializedObject settings;
        private class Provider : SettingsProvider
        {
            public Provider(string path, SettingsScope scope = SettingsScope.User) : base (path, scope) { }
            public override void OnGUI(string searchContext) => DrawGUI ();
        }

        [SettingsProvider] static SettingsProvider Settings() => new Provider ("Project/Configurator", SettingsScope.Project);

        static void DrawGUI()
        {
            const float Width = 640;

            var editor = Editor.CreateEditor (ProjectSettings.GetOrCreateSettings ());

            var indent = EditorGUI.indentLevel;
            EditorGUILayout.BeginVertical ("box"); //, GUILayout.MaxWidth (Width));
            EditorGUI.indentLevel++;
            EditorGUIUtility.labelWidth = Width / 2;

            // static readonly string[] _dontIncludeMe = new string[] { "m_Script" };
            // //         public override void OnInspectorGUI() => DrawDefaultInspector ();
            // //         public void DrawFromPropertyDrawer()
            // //         {
            // //             serializedObject.Update ();
            // using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
            //     editor.DrawPropertiesExcluding (serializedObject, new string[] { "m_Script" });

            editor.OnInspectorGUI ();

            EditorGUI.indentLevel = indent;
            EditorGUILayout.EndVertical ();

            if (GUI.changed)
            {
                settings.ApplyModifiedProperties ();
                AssetDatabase.SaveAssets ();
                Debug.Log ("Changed");
            }
        }
    }
#endif
}
