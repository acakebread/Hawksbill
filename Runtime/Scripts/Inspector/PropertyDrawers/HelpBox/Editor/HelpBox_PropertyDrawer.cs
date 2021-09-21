// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (HelpBoxAttribute))]
public class HelpBox_PropertyDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        var attribute = this.attribute as HelpBoxAttribute;
        var rect = new Rect (position) { height = attribute.lines * EditorGUIUtility.singleLineHeight };
        EditorGUI.HelpBox (rect, attribute.text, (MessageType) (int) attribute.type);
    }

    public override float GetHeight()
    {
        var attribute = this.attribute as HelpBoxAttribute;
        return attribute.lines * EditorGUIUtility.singleLineHeight + 1;
    }
}
