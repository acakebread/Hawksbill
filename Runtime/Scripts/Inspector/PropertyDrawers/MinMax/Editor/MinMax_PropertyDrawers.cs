// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/03/2021 19:03:47 by seantcooper
using UnityEngine;
using UnityEditor;
using System.Collections;
using Hawksbill;

[CustomPropertyDrawer (typeof (MinMaxAttribute))]
public class MinMax_PropertyDrawer : PropertyDrawer
{
    const float Spacing = 2;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        const float W = 50, P = 5;
        //if (Event.current.type != EventType.Repaint) return;

        var attr = (MinMaxAttribute) this.attribute;

        var minProperty = property.FindPropertyRelative ("min");
        var maxProperty = property.FindPropertyRelative ("max");

        Rect p = EditorGUI.PrefixLabel (position, label);
        var minRect = new Rect (p.x + p.width - (W * 2 + P), p.y, W, p.height);
        var maxRect = new Rect (minRect.max.x + P, p.y, W, p.height);

        var sliderRect = new Rect (p.x, p.y, minRect.min.x - (p.x + P), p.height);

        using (new GUIHelpers.Indent (0))
        {
            if (minProperty.type == "int")
            {
                float minValue = minProperty.intValue, maxValue = maxProperty.intValue;
                EditorGUI.MinMaxSlider (sliderRect, ref minValue, ref maxValue, attr.min, attr.max);
                minProperty.intValue = EditorGUI.IntField (minRect, (int) minValue);
                maxProperty.intValue = EditorGUI.IntField (maxRect, (int) maxValue);
            }
            else if (minProperty.type == "float")
            {
                float places(float n) => float.Parse (n.ToString ("F3"));
                float minValue = places (minProperty.floatValue);
                float maxValue = places (maxProperty.floatValue);
                EditorGUI.MinMaxSlider (sliderRect, ref minValue, ref maxValue, attr.min, attr.max);
                minProperty.floatValue = EditorGUI.FloatField (minRect, minValue);
                maxProperty.floatValue = EditorGUI.FloatField (maxRect, maxValue);
            }
        }
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