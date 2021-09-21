// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using Hawksbill;

[CustomPropertyDrawer (typeof (HelpBoxIfAttribute))]
public class HelpBoxIf_PropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attribute = this.attribute as HelpBoxIfAttribute;
        var value = (bool) property.getValue (attribute.member);
        var height = attribute.showProperty ? EditorGUI.GetPropertyHeight (property, label, true) : 0;
        if (!value) height += attribute.lines * EditorGUIUtility.singleLineHeight;
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attribute = this.attribute as HelpBoxIfAttribute;
        var value = (bool) property.getValue (attribute.member);
        if (!value)
        {
            var rect = new Rect (position) { height = attribute.lines * EditorGUIUtility.singleLineHeight };
            EditorGUI.HelpBox (rect, attribute.text, (MessageType) (int) attribute.type);
            position.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
        }
        if (attribute.showProperty) EditorGUI.PropertyField (position, property, label, true);
    }
}