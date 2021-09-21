// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 13/02/2021 10:27:01 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    public class ArrayItemAttribute : PropertyAttribute
    {
        public readonly string elementName = "";
        public readonly float labelWidth;

        public ArrayItemAttribute() { }
        public ArrayItemAttribute(string elementName) => this.elementName = elementName;
        public ArrayItemAttribute(string elementName, float labelWidth) : this (elementName) => this.labelWidth = labelWidth;
        public ArrayItemAttribute(bool hideLabel)
        {
            if (hideLabel) this.labelWidth = 0.1f;
        }
    }
}