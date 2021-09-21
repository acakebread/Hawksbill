// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof (LineAttribute))]
internal sealed class Line_PropertyDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        var attr = ((LineAttribute) this.attribute);
        position.yMin += attr.padding;
        position.yMax -= attr.padding;
        position = EditorGUI.IndentedRect (position);
        EditorGUI.DrawRect (position, new Color (0.5f, 0.5f, 0.5f, 0.5f));
    }

    public override float GetHeight()
    {
        var attr = ((LineAttribute) this.attribute);
        return attr.height;
    }
}