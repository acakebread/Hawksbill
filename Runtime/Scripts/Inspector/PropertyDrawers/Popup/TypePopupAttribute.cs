// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

[AttributeUsage (AttributeTargets.Field)]
public class TypePopupAttribute : PropertyAttribute
{
    public readonly Type type;
    public TypePopupAttribute(Type type)
    {
        this.type = type;
    }
}
