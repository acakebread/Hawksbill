// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 02/09/2021 21:00:29 by seantcooper
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    [ExecuteInEditMode]
    public class HierarchyTag : PrettyEditorAttribute
    {

#if UNITY_EDITOR
        void OnValidate() => OnEnable ();
        void OnEnable()
        {
            hideFlags |= HideFlags.HideInInspector;
            color = new Color32 (0, 153, 255, 128);
            errorColor = new Color (1, 0, 0, 0.5f);
            error = false;
            type = Type.Margin;
        }

#endif
    }
}