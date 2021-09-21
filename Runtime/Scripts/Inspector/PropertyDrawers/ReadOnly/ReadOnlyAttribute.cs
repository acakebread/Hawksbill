// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:38 by seancooper
using UnityEngine;
using System;

public class ReadOnlyAttribute : PropertyAttribute
{
    public State state = State.Always;
    public ReadOnlyAttribute(State state = State.Always)
    {
        this.state = state;
    }

    public enum State
    {
        Always, Editor, Runtime
    }
}