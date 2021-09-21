// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

public class ShowAttribute : PropertyAttribute
{
    public string member;
    public Action action;
    public ShowAttribute(string member, Action action = Action.Hide)
    {
        this.member = member;
        this.action = action;
    }
    public enum Action { Hide, Disable }
}