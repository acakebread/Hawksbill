// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 12/02/2021 09:13:58 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    public static class ObjectX
    {
        public static void SetActive(UnityEngine.Object target, bool state)
        {
            if (target is UnityEngine.GameObject) (target as UnityEngine.GameObject).SetActive (state);
            else if (target is UnityEngine.Component) target.GetType ().GetProperty ("enabled")?.SetValue (target, state);
            else
            {
                Debug.LogWarning ("Object '" + target + "' cannot be disabled/enabled!");
            }
        }

        public static bool GetActive(UnityEngine.Object target)
        {
            if (target is UnityEngine.GameObject)
                return (target as UnityEngine.GameObject).activeSelf;
            else if (target is UnityEngine.Component && target.GetType ().GetProperty ("enabled") != null)
                return (bool) target.GetType ().GetProperty ("enabled").GetValue (target);
            Debug.LogWarning ("Object '" + target + "' has no enabling interface!");
            return false;
        }

        public static void destroy(this UnityEngine.Object _object)
        {
            if (Application.isPlaying) UnityEngine.Object.Destroy (_object);
            else UnityEngine.Object.DestroyImmediate (_object);
        }

        public static void destroyInEditor(this UnityEngine.Object _object)
        {
#if UNITY_EDITOR
            void destroy() => UnityEngine.Object.DestroyImmediate (_object);
            UnityEditor.EditorApplication.delayCall += destroy;
#endif
        }
    }
}
