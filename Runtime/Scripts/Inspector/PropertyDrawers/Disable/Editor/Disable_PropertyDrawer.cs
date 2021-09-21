// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (DisableAttribute))]
public class Disable_PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new Enable (false)) EditorGUI.PropertyField (position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => base.GetPropertyHeight (property, label);
}

public class Enable : IDisposable
{
    bool enabled;
    public Enable(bool enabled) { enabled = GUI.enabled; GUI.enabled = this.enabled; }
    public void Dispose() => GUI.enabled = enabled;
}