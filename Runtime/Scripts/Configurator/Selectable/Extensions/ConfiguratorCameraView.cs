// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 12/09/2021 09:08:59 by seantcooper
using System;
using UnityEngine;

namespace Hawksbill.Configurator
{
    public class ConfiguratorCameraView : ScriptableObject
    {
        public FieldObject panelID;
        public View[] views;
        public int initialIndex;
        public View initialView => this[initialIndex];

        public View this[int index] => views != null && index >= 0 && index < views.Length ? views[index] : null;

        [Serializable]
        public class View
        {
            public ConfiguratorGUIControl control;
            [Range (-180, 180)] public float verticalValue;
            [Range (-180, 180)] public float horizontalValue;
            [Range (0, 100)] public float ZoomValue;
            public static implicit operator bool(View empty) => empty != null;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer (typeof (ConfiguratorCameraView), true)]
    public class ConfiguratorCameraViewDrawer : ScriptableObjectInline_Drawer<ConfiguratorCameraView> { }
#endif
}