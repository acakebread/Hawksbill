// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 07/08/2021 10:17:43 by seantcooper
using UnityEngine;
using UnityEditor;
using Hawksbill;
using Hawksbill.Geometry;
using static Hawksbill.DynamicDescription;
using static Hawksbill.DynamicDescription.Field;

namespace Hawksbill
{
    [CanEditMultipleObjects, CustomPropertyDrawer (typeof (FieldValueCollection))]
    public class FieldValueCollection_PropertyDrawer : PropertyDrawer
    {
        const float Padding = 2;
        static Color DefaultColor = new Color (0, 0, 0, 0.1f);
        static Color SelectedColor = new Color (0, 0.3f, 1, 0.1f);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var values = property.FindPropertyRelative ("values");
            int count = values.arraySize;

            Rect b = new Rect (0, position.y, Screen.width, GetPropertyHeight (property, label)).inflate (Padding);
            EditorGUI.DrawRect (b, new Color (0, 0, 0, 0.1f));

            Rect line = new Rect (position) { height = EditorGUIUtility.singleLineHeight, y = position.y + Padding };
            for (int i = 0; i < count; i++)
            {
                var element = values.GetArrayElementAtIndex (i);
                var field = element.FindPropertyRelative ("field");
                if (field == null) continue;

                var xvalue = element.FindPropertyRelative ("value");
                var name = field.FindPropertyRelative ("name");
                var type = field.FindPropertyRelative ("type");

                var rangep = field.FindPropertyRelative ("range");
                MinMaxFloat range = new MinMaxFloat (rangep.FindPropertyRelative ("min").floatValue, rangep.FindPropertyRelative ("max").floatValue);
                Value newValue, value = (Value) xvalue.getTarget ();

                var content = new GUIContent (name.stringValue);
                EditorGUI.BeginProperty (line, content, xvalue);
                EditorGUI.BeginChangeCheck ();
                switch ((Value.Type) type.intValue)
                {
                    case Value.Type.Bool:
                        newValue = EditorGUI.Toggle (line, content, value);
                        break;

                    case Value.Type.Int:
                        if ((int) range.min == (int) range.max) newValue = EditorGUI.DelayedIntField (line, content, value);
                        else newValue = EditorGUI.IntSlider (line, content.text, value, (int) range.min, (int) range.max);
                        break;

                    case Value.Type.Float:
                        if (range.min == range.max) newValue = EditorGUI.DelayedFloatField (line, content, value);
                        else newValue = EditorGUI.Slider (line, content.text, value, range.min, range.max);
                        break;

                    default:
                    case Value.Type.String: newValue = EditorGUI.TextField (line, content, value); break;
                }
                if (EditorGUI.EndChangeCheck ())
                    xvalue.FindPropertyRelative ("rawValue").stringValue = newValue;
                EditorGUI.EndProperty ();
                line.y += line.height + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) *
                property.FindPropertyRelative ("values").arraySize + Padding * 2;
        }
    }
}