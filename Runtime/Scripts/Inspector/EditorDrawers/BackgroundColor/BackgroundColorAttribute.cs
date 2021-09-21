// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

public class BackgroundColorAttribute : Attribute
{
    public Color color;
    public BackgroundColorAttribute(float r, float g, float b, float a) =>
        color = new Color (r, g, b, a);
}