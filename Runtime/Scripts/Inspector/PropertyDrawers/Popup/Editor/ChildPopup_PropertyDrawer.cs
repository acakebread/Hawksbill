// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Hawksbill;

[CustomPropertyDrawer (typeof (ChildPopupAttribute))]
public class ChildPopup_PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = ((ChildPopupAttribute) this.attribute);

        var target = property.serializedObject.targetObject as UnityEngine.Component;
        string[] objects;
        if (target) objects = target.GetComponentsInChildren<Transform> ().Where (t => t.parent == target.transform).Select (t => t.name).ToArray ();
        else objects = new string[] { "<invalid>" };

        int index = Array.IndexOf (objects, property.stringValue);
        index = EditorGUI.Popup (position, label.text, index, objects);

        property.stringValue = index == -1 ? "" : objects[index];
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => base.GetPropertyHeight (property, label);

    IEnumerable<string> getImplementingTypes(Type type)
    {
        var types = Assembly.GetAssembly (type).GetTypes ();
        return types.Where (t => t.BaseType == type).Select (t => t.FullName);
    }

}