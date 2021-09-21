// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 11/09/2021 08:33:13 by seantcooper
using UnityEngine;

namespace Hawksbill.Configurator
{
    public class FieldObject : ScriptableObject
    {
        public UnityEngine.Object value;
        public T getValue<T>() where T : UnityEngine.Object => value ? (T) value : null;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer (typeof (FieldObject), true)]
    public class FieldObjectDrawer : ScriptableObjectInline_Drawer<FieldObject> { }
#endif
}