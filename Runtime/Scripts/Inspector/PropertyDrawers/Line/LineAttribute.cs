// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

public class LineAttribute : PropertyAttribute
{
    public float thickness, padding;
    public float height => padding * 2 + thickness;
    public LineAttribute(float thickness = 1, float padding = 4)
    {
        this.thickness = thickness;
        this.padding = padding;
    }
}