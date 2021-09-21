// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 10/06/2021 18:43:20 by seantcooper
using UnityEngine;
using Hawksbill;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
#if UNITY_EDITOR
    public static class SelectionX
    {
        public static void ForceSelection(GameObject gameObject)
        {
            void forceSelection()
            {
                if (Selection.activeGameObject == gameObject) EditorApplication.update -= forceSelection;
                Selection.activeGameObject = gameObject;
            }
            EditorApplication.update += forceSelection;
        }
    }
#endif
}