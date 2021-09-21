// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

[AttributeUsage (AttributeTargets.Field)]
public class MinMaxAttribute : PropertyAttribute
{
    public readonly float min, max;
    public MinMaxAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
