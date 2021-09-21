// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 11/09/2021 08:33:13 by seantcooper
using UnityEngine;

namespace Hawksbill.Configurator
{
    public class FieldString : ScriptableObject
    {
        public string value;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer (typeof (FieldString), true)]
    public class FieldStringDrawer : ScriptableObjectInline_Drawer<FieldString> { }
#endif
}