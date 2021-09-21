// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 08/02/2021 10:51:24 by seantcooper
using UnityEngine;
using Hawksbill;
using System;

namespace Hawksbill
{
    public class AssetColorAttribute : Attribute
    {
        public Color color;

        public AssetColorAttribute(float r, float g, float b)
        {
            color = new Color (r, g, b);
        }
    }
}