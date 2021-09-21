// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

public class HelpBoxAttribute : PropertyAttribute
{
    public string text;
    public Type type;
    public int lines;

    public HelpBoxAttribute(string text, Type type = Type.Info, int lines = 2)
    {
        this.text = text;
        this.type = type;
        this.lines = lines;
    }

    public enum Type
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }
}

public class HelpBoxIfAttribute : HelpBoxAttribute
{
    public string member;
    public bool showProperty;
    public HelpBoxIfAttribute(string member, string text, Type type = Type.Info, int lines = 2) : this (member, text, true, type, lines) { }
    public HelpBoxIfAttribute(string member, string text, bool showProperty, Type type = Type.Info, int lines = 2) : base (text, type, lines)
    {
        this.member = member;
        this.showProperty = showProperty;
    }
}