// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

[AttributeUsage (AttributeTargets.Field)]
public class ObjectColumnsAttribute : PropertyAttribute
{
    const float DefaultLabelWidth = 14;
    public readonly bool hideLabels;
    public readonly float labelSize;

    public ObjectColumnsAttribute()
    {
        this.hideLabels = false;
        this.labelSize = DefaultLabelWidth;
    }

    public ObjectColumnsAttribute(float labelSize)
    {
        this.hideLabels = false;
        this.labelSize = labelSize;
    }

    public ObjectColumnsAttribute(bool hideLabels)
    {
        this.hideLabels = hideLabels;
        this.labelSize = 0;
    }


}
