// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer (typeof (TypePopupAttribute))]
public class TypePopup_PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = ((TypePopupAttribute) this.attribute);
        var types = getImplementingTypes (attr.type).ToList ();
        types.Insert (0, "None");
        int index = EditorGUI.Popup (position, label.text, types.IndexOf (property.stringValue), types.ToArray ());
        property.stringValue = index == -1 ? "" : types[index];
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => base.GetPropertyHeight (property, label);

    IEnumerable<string> getImplementingTypes(Type type)
    {
        var types = Assembly.GetAssembly (type).GetTypes ();
        return types.Where (t => t.BaseType == type).Select (t => t.FullName);
    }

}