// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (ButtonAttribute))]
public class Button_PropertyDrawer : PropertyDrawer
{
    static Color HilightColor = new Color (0.39f, 0.9f, 0.51f);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new GUIColor ())
        {
            EditorGUI.BeginProperty (position, label, property);
            if (property.boolValue) GUI.color = HilightColor;
            if (GUI.Button (position, property.displayName))
                property.boolValue = !property.boolValue;
            EditorGUI.EndProperty ();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight (property, label);
    }
}

public class GUIColor : IDisposable
{
    Color _color;
    public GUIColor() => _color = GUI.color;
    public void Dispose() => GUI.color = _color;
}