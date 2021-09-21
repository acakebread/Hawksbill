// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 15/07/2021 08:51:08 by seantcooper
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Hawksbill.IO;
using System;

namespace Hawksbill
{
    //[UnityEditor.CustomPropertyDrawer (typeof (T))]
    public class ScriptableObjectInline_Drawer<T> : PropertyDrawer where T : ScriptableObject
    {
        static float LineHeight => EditorGUIUtility.singleLineHeight;
        static float Spacing => EditorGUIUtility.standardVerticalSpacing;
        const float ButtonWidth = 44;
        string getPath(SerializedProperty property, T data)
        {
            if (data) return AssetDatabase.GetAssetPath (data);
            var target = property.serializedObject.targetObject as UnityEngine.Component;
            return target && target.gameObject.scene != null ? target.gameObject.scene.path : "Assets";
        }

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            int buttonCount = property.objectReferenceValue == null ? 1 : 2;
            Rect r1 = new Rect (pos) { width = pos.width - ButtonWidth * buttonCount + 1, height = LineHeight };
            Rect rb = new Rect (pos) { x = r1.xMax, width = ButtonWidth, height = LineHeight };
            EditorGUI.PropertyField (r1, property, label, true);

            bool iNew = GUI.Button (rb, "New");
            bool bClone = property.objectReferenceValue != null && GUI.Button (new Rect (rb) { x = rb.x + ButtonWidth - 1 }, "Clone");

            if (iNew || bClone)
            {
                T data = null;
                T currentData = property.getTarget () as T;
                if (iNew) data = ScriptableObjectX.CreateInstance<T> (getPath (property, currentData));
                else if (bClone) data = ScriptableObjectX.CreateInstance (property.getTarget () as T) as T;
                if (data)
                {
                    property.serializedObject.Update ();
                    assign (property, data);
                    EditorUtility.SetDirty (property.serializedObject.targetObject);
                    property.serializedObject.ApplyModifiedProperties ();
                }
            }

            showInEditor (pos, property);
        }

        // Editor editor = null;
        void showInEditor(Rect position, SerializedProperty property)
        {
            if (!property.objectReferenceValue) return;
            if (property.isExpanded = EditorGUI.Foldout (new Rect (position) { height = LineHeight }, property.isExpanded, GUIContent.none))
            {
                using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
                    iterateVisibleProperties (position, property, (rect, prop) => EditorGUI.PropertyField (rect, prop, true));
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            iterateVisibleProperties (new Rect (), property);

        static float iterateVisibleProperties(Rect position, SerializedProperty property, Action<Rect, SerializedProperty> action = null)
        {
            Rect pos = new Rect (position) { y = position.y + LineHeight + Spacing };
            if (property.objectReferenceValue && property.isExpanded)
            {
                var data = property.objectReferenceValue as ScriptableObject;
                if (data == null) return LineHeight;
                SerializedObject serializedObject = new SerializedObject (data);
                SerializedProperty prop = serializedObject.GetIterator ();
                for (bool firstTime = true; prop.NextVisible (firstTime); firstTime = false)
                {
                    if (prop.name == "m_Script") continue;
                    pos.height = EditorGUI.GetPropertyHeight (serializedObject.FindProperty (prop.name), null, true) + Spacing;
                    action?.Invoke (pos, prop);
                    pos.y += pos.height + Spacing;
                }
                if (GUI.changed)
                {
                    serializedObject.ApplyModifiedProperties ();
                    // property.serializedObject.Update ();
                    // EditorUtility.SetDirty (property.serializedObject.targetObject);
                }
            }
            return pos.y - position.y;
        }

        public virtual void assign(SerializedProperty property, T objectReferenceValue) =>
            property.objectReferenceValue = objectReferenceValue;
    }

    // public class ScriptableObjectInlineEditor<T> : Editor where T : ScriptableObject
    // {
    //     protected static readonly string[] dontIncludeMe = new string[] { "m_Script" };
    //     public override void OnInspectorGUI() => DrawDefaultInspector ();
    //     public void DrawFromPropertyDrawer()
    //     {
    //         serializedObject.Update ();
    //         using (new GUIHelpers.Indent (EditorGUI.indentLevel + 1))
    //             DrawPropertiesExcluding (serializedObject, dontIncludeMe);
    //         serializedObject.ApplyModifiedProperties ();
    //     }
    // }

    // public class ScriptableObjectInlineDrawer<T> : PropertyDrawer where T : ScriptableObject
    // {
    //     const float ButtonWidth = 44;
    //     string getPath(SerializedProperty property, T data)
    //     {
    //         if (data) return AssetDatabase.GetAssetPath (data);
    //         var target = property.serializedObject.targetObject as UnityEngine.Component;
    //         return target && target.gameObject.scene != null ? target.gameObject.scene.path : "Assets";
    //     }

    //     public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    //     {
    //         int buttonCount = property.objectReferenceValue == null ? 1 : 2;
    //         Rect r1 = new Rect (pos) { width = pos.width - ButtonWidth * buttonCount + 1 };
    //         Rect rb = new Rect (pos) { x = r1.xMax, width = ButtonWidth };
    //         EditorGUI.PropertyField (r1, property, label, true);

    //         bool iNew = GUI.Button (rb, "New");
    //         bool bClone = property.objectReferenceValue != null && GUI.Button (new Rect (rb) { x = rb.x + ButtonWidth - 1 }, "Clone");

    //         if (iNew || bClone)
    //         {
    //             T data = null;
    //             T currentData = property.getTarget () as T;
    //             if (iNew) data = ScriptableObjectX.CreateInstance<T> (getPath (property, currentData));
    //             else if (bClone) data = ScriptableObjectX.CreateInstance (property.getTarget () as T) as T;

    //             if (data)
    //             {
    //                 property.serializedObject.Update ();
    //                 assign (property, data);
    //                 EditorUtility.SetDirty (property.serializedObject.targetObject);
    //                 property.serializedObject.ApplyModifiedProperties ();
    //             }
    //         }

    //         showInEditor (pos, property);

    //     }

    //     Editor editor = null;
    //     void showInEditor(Rect pos, SerializedProperty property)
    //     {
    //         if (property.objectReferenceValue != null)
    //             property.isExpanded = EditorGUI.Foldout (pos, property.isExpanded, GUIContent.none);

    //         if (property.isExpanded)
    //         {
    //             if (!editor) Editor.CreateCachedEditor (property.objectReferenceValue, null, ref editor);
    //             if (editor is ScriptableObjectInlineEditor<T>)
    //                 (editor as ScriptableObjectInlineEditor<T>).DrawFromPropertyDrawer ();
    //             else if (editor) editor.OnInspectorGUI ();
    //         }
    //     }

    //     public virtual void assign(SerializedProperty property, T objectReferenceValue)
    //     {
    //         property.objectReferenceValue = objectReferenceValue;
    //     }
    // }
}
#endif
