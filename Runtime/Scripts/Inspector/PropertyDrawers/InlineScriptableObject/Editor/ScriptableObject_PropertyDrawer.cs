// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 10/03/2021 08:41:10 by seantcooper
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using Hawksbill;

// [CustomPropertyDrawer (typeof (ScriptableObject), true)]
// public class ScriptableObjectDrawer : PropertyDrawer
// {
//     // Cached scriptable object editor
//     private Editor editor = null;

//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         EditorGUI.PropertyField (position, property, label, true);

//         if (property.objectReferenceValue != null)
//             property.isExpanded = EditorGUI.Foldout (position, property.isExpanded, GUIContent.none);

//         if (property.isExpanded)
//         {
//             if (!editor) Editor.CreateCachedEditor (property.objectReferenceValue, null, ref editor);
//             if (editor)
//             {
//                 EditorGUI.indentLevel++;
//                 editor.OnInspectorGUI ();
//                 //                EditorGUI.DrawRect (GUILayoutUtility.GetLastRect (), color);

//                 EditorGUI.indentLevel--;
//             }
//         }
//     }

//     Color color = new Color (0, 0, 1, 0.05f);
// }