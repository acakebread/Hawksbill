// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Hawksbill;
using UnityEditor;
using UnityEngine;
// using Unity.Mathematics;

[
    CustomPropertyDrawer (typeof (CompactAttribute)),
    CustomPropertyDrawer (typeof (Vector3)), CustomPropertyDrawer (typeof (Vector2)),
    CustomPropertyDrawer (typeof (Vector3Int)), CustomPropertyDrawer (typeof (Vector2Int)),
// CustomPropertyDrawer (typeof (float3)), CustomPropertyDrawer (typeof (float2)),
// CustomPropertyDrawer (typeof (bool3)), CustomPropertyDrawer (typeof (bool2)),
]
public class Compact_PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new GUIHelpers.WideMode (true))
            EditorGUI.PropertyField (position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        using (new GUIHelpers.WideMode (true))
            return base.GetPropertyHeight (property, label);
    }
}