// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 19/09/2021 14:43:22 by seantcooper
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
#if UNITY_EDITOR
    public class Editor : UnityEditor.Editor
    {
        static string[] dontIncludeMe = new string[] { "m_Script" };
        public void OnInspectorGUI(bool compact)
        {
            if (compact)
            {
                serializedObject.Update ();
                // using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
                DrawPropertiesExcluding (serializedObject, dontIncludeMe);
                serializedObject.ApplyModifiedProperties ();
            }
            else base.OnInspectorGUI ();
        }
    }
#endif
}