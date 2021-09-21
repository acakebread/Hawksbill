// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (LabelAttribute))]
public class Label_PropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
        EditorGUI.GetPropertyHeight (property, label, true);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = ((LabelAttribute) this.attribute);
        switch (attr.position)
        {
            case LabelAttribute.Position.Post: label = new GUIContent (label.text + " " + attr.label); break;
            case LabelAttribute.Position.Replace: label = new GUIContent (attr.label); break;
            case LabelAttribute.Position.Pre: label = new GUIContent (attr.label + " " + label.text); break;
        }
        EditorGUI.PropertyField (position, property, label, true);
    }
}