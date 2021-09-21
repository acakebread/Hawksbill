// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hawksbill;
using Hawksbill.Analytics;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (RangePopupAttribute))]
public class RangePopup_PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Profiler.Start ("Slow");
        var values = getValues (property);
        int index = Array.IndexOf (values, property.stringValue);
        index = EditorGUI.Popup (position, label.text, index, values);
        property.stringValue = index == -1 ? "" : values[index];
        // Debug.Log (Profiler.Stop ("Slow"));
    }

    string[] getValues(SerializedProperty property)
    {
        var attribute = (this.attribute as RangePopupAttribute);
        if (attribute.values != null) return attribute.values;
        return ((string[]) property.getValue (attribute.member)).OrderBy (s => s).ToArray ();
    }
}