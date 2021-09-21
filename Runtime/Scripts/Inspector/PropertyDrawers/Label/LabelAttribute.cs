// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

public class LabelAttribute : PropertyAttribute
{
    public string label;
    public Position position;
    public LabelAttribute(string label, Position position = Position.Replace)
    {
        this.label = label;
        this.position = position;
    }
    public enum Position { Replace, Pre, Post }
}