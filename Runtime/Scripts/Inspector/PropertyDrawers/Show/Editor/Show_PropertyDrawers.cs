// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using Hawksbill;
using static ShowAttribute;

[CustomPropertyDrawer (typeof (ShowAttribute))]
public class Show_PropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var value = (bool) PropertyDrawerX.getValue (property, (attribute as ShowAttribute).member);
        switch ((attribute as ShowAttribute).action)
        {
            case Action.Hide:
                if (!value) return -EditorGUIUtility.standardVerticalSpacing;
                break;

            case Action.Disable:
                break;
        }
        return EditorGUI.GetPropertyHeight (property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var value = (bool) PropertyDrawerX.getValue (property, (attribute as ShowAttribute).member);

        switch ((attribute as ShowAttribute).action)
        {
            case Action.Hide:
                if (value) EditorGUI.PropertyField (position, property, label, true);
                break;

            case Action.Disable:
                using (new GUIHelpers.Enable (value))
                    EditorGUI.PropertyField (position, property, label, true);
                break;
        }
    }
}