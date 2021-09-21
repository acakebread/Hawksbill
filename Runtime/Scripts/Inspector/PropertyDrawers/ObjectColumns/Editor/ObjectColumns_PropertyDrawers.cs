// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/03/2021 19:03:47 by seantcooper
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof (ObjectColumnsAttribute))]
public class VectorPropertyDrawer : PropertyDrawer
{
    const float Spacing = 2;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //if (Event.current.type != EventType.Repaint) return;

        var attr = ((ObjectColumnsAttribute) this.attribute);
        var fieldCount = GetFieldCount (property);
        Rect contentPosition = EditorGUI.PrefixLabel (position, label);

        EditorGUIUtility.labelWidth = Mathf.Max (1, attr.labelSize);
        bool hideLabels = contentPosition.width < 185 || attr.hideLabels;
        contentPosition.width = (contentPosition.width - Spacing * (fieldCount - 1)) / fieldCount;
        float fieldWidth = contentPosition.width + Spacing;

        using (var indent = new EditorGUI.IndentLevelScope (-EditorGUI.indentLevel))
        {
            for (int i = 0; i < fieldCount; i++)
            {
                if (!property.NextVisible (true)) break;
                label = EditorGUI.BeginProperty (contentPosition, new GUIContent (property.displayName), property);
                EditorGUI.PropertyField (contentPosition, property, hideLabels ? GUIContent.none : label);
                EditorGUI.EndProperty ();
                contentPosition.x += fieldWidth;
            }
        }
    }

    private static int GetFieldCount(SerializedProperty property)
    {
        int count = 0;
        IEnumerator children = property.Copy ().GetEnumerator ();
        while (children.MoveNext ()) count++;
        return count;
    }
}


// using UnityEditor;
// using UnityEngine;

// [CustomPropertyDrawer (typeof (Point))]
// public class PointDrawer : PropertyDrawer
// {
//     SerializedProperty X, Y;
//     string name;
//     bool cache = false;

//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         if (!cache)
//         {
//             //get the name before it's gone
//             name = property.displayName;

//             //get the X and Y values
//             property.Next (true);
//             X = property.Copy ();
//             property.Next (true);
//             Y = property.Copy ();

//             cache = true;
//         }

//         Rect contentPosition = EditorGUI.PrefixLabel (position, new GUIContent (name));

//         //Check if there is enough space to put the name on the same line (to save space)
//         if (position.height > 16f)
//         {
//             position.height = 16f;
//             EditorGUI.indentLevel += 1;
//             contentPosition = EditorGUI.IndentedRect (position);
//             contentPosition.y += 18f;
//         }

//         float half = contentPosition.width / 2;
//         GUI.skin.label.padding = new RectOffset (3, 3, 6, 6);

//         //show the X and Y from the point
//         EditorGUIUtility.labelWidth = 14f;
//         contentPosition.width *= 0.5f;
//         EditorGUI.indentLevel = 0;

//         // Begin/end property & change check make each field
//         // behave correctly when multi-object editing.
//         EditorGUI.BeginProperty (contentPosition, label, X);
//         {
//             EditorGUI.BeginChangeCheck ();
//             int newVal = EditorGUI.IntField (contentPosition, new GUIContent ("X"), X.intValue);
//             if (EditorGUI.EndChangeCheck ())
//                 X.intValue = newVal;
//         }
//         EditorGUI.EndProperty ();

//         contentPosition.x += half;

//         EditorGUI.BeginProperty (contentPosition, label, Y);
//         {
//             EditorGUI.BeginChangeCheck ();
//             int newVal = EditorGUI.IntField (contentPosition, new GUIContent ("Y"), Y.intValue);
//             if (EditorGUI.EndChangeCheck ())
//                 Y.intValue = newVal;
//         }
//         EditorGUI.EndProperty ();
//     }

//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         return Screen.width < 333 ? (16f + 18f) : 16f;
//     }
// }