// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (EnumMaskAttribute))]
public class EnumMask_PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) =>
        _property.intValue = EditorGUI.MaskField (_position, _label, _property.intValue, _property.enumNames);
}
