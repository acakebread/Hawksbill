// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (ReadOnlyAttribute))]
public class ReadOnly_PropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
        EditorGUI.GetPropertyHeight (property, label, true);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ReadOnlyAttribute attribute = this.attribute as ReadOnlyAttribute;

        switch (attribute.state)
        {
            case ReadOnlyAttribute.State.Always:
                GUI.enabled = false;
                break;
            case ReadOnlyAttribute.State.Editor:
                GUI.enabled = Application.isPlaying;
                break;
            case ReadOnlyAttribute.State.Runtime:
                GUI.enabled = !Application.isPlaying;
                break;
        }
        EditorGUI.PropertyField (position, property, label, true);
        GUI.enabled = true;
    }
}