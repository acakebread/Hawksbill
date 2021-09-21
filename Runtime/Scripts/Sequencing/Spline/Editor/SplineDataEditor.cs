// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 07/06/2021 20:28:14 by seantcooper
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using Hawksbill;
// using System;
// using static Hawksbill.Sequencing.SplineData;
// using V3 = UnityEngine.Vector3;

// namespace Hawksbill.Sequencing.SplineEdit
// {
//     [CustomEditor (typeof (SplineData))]
//     public class SplineDataEditor : Editor
//     {
//         static readonly string[] _dontIncludeMe = new string[] { "m_Script" };
//         public override void OnInspectorGUI() => DrawDefaultInspector ();
//         public void DrawFromPropertyDrawer()
//         {
//             serializedObject.Update ();
//             using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
//                 DrawPropertiesExcluding (serializedObject, _dontIncludeMe);
//             serializedObject.ApplyModifiedProperties ();
//         }
//     }

//     [CustomPropertyDrawer (typeof (SplineData), true)]
//     public class SplineDataDrawer : PropertyDrawer
//     {
//         const float ButtonWidth = 44;
//         Editor editor = null;

//         string getPath(SerializedProperty property, UnityEngine.Object data)
//         {
//             if (data) return AssetDatabase.GetAssetPath (data);
//             var target = property.serializedObject.targetObject as UnityEngine.Component;
//             return target && target.gameObject.scene != null ? target.gameObject.scene.path : "Assets";
//         }

//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             int buttonCount = property.objectReferenceValue == null ? 1 : 2;
//             Rect r1 = new Rect (position) { width = position.width - ButtonWidth * buttonCount + 1 };
//             Rect rb = new Rect (position) { x = r1.xMax, width = ButtonWidth };
//             EditorGUI.PropertyField (r1, property, label, true);

//             int iNew = EditorGUI.Popup (rb, 0, primitives, GUI.skin.button);
//             bool bClone = property.objectReferenceValue != null && GUI.Button (new Rect (rb) { x = rb.x + ButtonWidth - 1 }, "Clone");

//             if (iNew > 0 || bClone)
//             {
//                 SplineData splineData = null;
//                 SplineData currentData = property.getTarget () as SplineData;
//                 if (iNew > 0)
//                 {
//                     splineData = ScriptableObjectX.CreateInstance<SplineData> (getPath (property, currentData));
//                     setPrimitive (splineData, iNew);
//                 }
//                 else if (bClone) splineData = ScriptableObjectX.CreateInstance (property.getTarget () as SplineData) as SplineData;

//                 if (splineData)
//                 {
//                     property.serializedObject.Update ();
//                     property.objectReferenceValue = splineData;
//                     EditorUtility.SetDirty (property.serializedObject.targetObject);
//                     property.serializedObject.ApplyModifiedProperties ();
//                 }
//             }

//             if (property.objectReferenceValue != null)
//                 property.isExpanded = EditorGUI.Foldout (position, property.isExpanded, GUIContent.none);

//             if (property.isExpanded)
//             {
//                 if (!editor) Editor.CreateCachedEditor (property.objectReferenceValue, null, ref editor);
//                 if (editor) (editor as SplineDataEditor).DrawFromPropertyDrawer ();
//             }
//         }

//         string[] primitives = new string[] { "New", "Line", "Circle", "Square" };

//         void setPrimitive(SplineData data, int index)
//         {
//             switch (index)
//             {
//                 case 0: break;
//                 case 1:
//                     {
//                         float len = 50, scale = len / 2;
//                         data.nodes = new List<Node>
//                         {
//                             new Node (new V3 (0, 0, 0), new V3 (0, 0, 0), new Node.Scale (scale), 0),
//                             new Node (new V3 (0, 0, len), new V3 (0, 0, 0), new Node.Scale (scale), 1),
//                         };
//                         data.loop = false;
//                     }
//                     break;
//                 case 2:
//                     {
//                         float radius = 50, scale = 4 * (Mathf.Sqrt (2) - 1) / 3 * radius;
//                         data.nodes = new List<Node>
//                         {
//                             new Node (new V3 (0, 0, radius), new V3 (0, 90, 0), new Node.Scale (scale), 0),
//                             new Node (new V3 (radius, 0, 0), new V3 (0, 180, 0), new Node.Scale (scale), 1),
//                             new Node (new V3 (0, 0, -radius), new V3 (0, 270, 0), new Node.Scale (scale), 2),
//                             new Node (new V3 (-radius, 0, 0), new V3 (0, 0, 0), new Node.Scale (scale), 3),
//                         };
//                         data.loop = true;
//                     }
//                     break;
//                 case 3:
//                     {
//                         float scale = 0.01f;
//                         data.nodes = new List<Node>
//                         {
//                             new Node (new V3 (50, 0, 50), new V3 (0, 135, 0), new Node.Scale (scale), 0),
//                             new Node (new V3 (50, 0, -50), new V3 (0, 225, 0), new Node.Scale (scale), 1),
//                             new Node (new V3 (-50, 0, -50), new V3 (0, 315, 0), new Node.Scale (scale), 2),
//                             new Node (new V3 (-50, 0, 50), new V3 (0, 45, 0), new Node.Scale (scale), 3),
//                         };
//                         data.loop = true;
//                     }
//                     break;
//             }
//         }
//     }
// }