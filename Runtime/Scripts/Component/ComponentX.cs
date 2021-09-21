// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 17/01/2021 12:29:09 by seantcooper
using System.Reflection;
using UnityEngine;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class ComponentX
    {
        public static bool GetComponent<T>(this Component c, out T component, bool add = false) where T : Component =>
            c.gameObject.GetComponent<T> (out component, add);

        public static T AddComponent<T>(this Component c) where T : Component =>
            c.gameObject.AddComponent<T> ();

        // public static bool TryGetComponentInChildren<T>(this MonoBehaviour c, bool includeInactive, out T result) => TryGetComponentInChildren (c, includeInactive, out result);
        public static bool TryGetComponentInChildren<T>(this Component c, bool includeInactive, out T result) where T : Component
        {
            result = c.GetComponentInChildren<T> (includeInactive);
            return result;
        }

        // public static bool TryGetComponentInParent<T>(this MonoBehaviour c, out T result) => TryGetComponentInParent (c, out result);
        public static bool TryGetComponentInParent<T>(this Component c, out T result) where T : Component
        {
            result = c.GetComponentInParent<T> ();
            return result;
        }

        /// <summary>Invokes a Private/Public method in this Component!</summary>
        public static void message<T>(this Component component, string methodName, params object[] parameters)
        {
            typeof (T).GetMethod (methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.
                Invoke (component, parameters);
        }

    }
}