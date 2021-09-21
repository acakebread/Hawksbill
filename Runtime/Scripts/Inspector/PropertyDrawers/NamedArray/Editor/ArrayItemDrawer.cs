// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 13/02/2021 10:27:27 by seantcooper
using UnityEngine;
using UnityEditor;

namespace Hawksbill
{
    [CustomPropertyDrawer (typeof (ArrayItemAttribute))]
    public class ArrayItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var attr = ((ArrayItemAttribute) this.attribute);

            if (attr.labelWidth > 0) EditorGUIUtility.labelWidth = attr.labelWidth;

            try
            {
                int pos = int.Parse (property.propertyPath.Split ('[', ']')[1]);
                EditorGUI.ObjectField (rect, property, new GUIContent (attr.elementName + pos));
            }
            catch
            {
                EditorGUI.ObjectField (rect, property, label);
            }
        }
    }
}