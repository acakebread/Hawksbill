// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

[AttributeUsage (AttributeTargets.Field)]
public class RangePopupAttribute : PropertyAttribute
{
    public readonly string[] values;
    public readonly string member;
    public readonly bool sort = true;
    public RangePopupAttribute(string member)
    {
        this.member = member;
    }
    public RangePopupAttribute(string[] values)
    {
        this.values = values;
    }
}
