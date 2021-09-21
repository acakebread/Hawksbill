// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:01 by seancooper
using UnityEngine;

namespace Hawksbill
{
    public class SingletonMonoBehaviour<T> : UnityEngine.MonoBehaviour where T : UnityEngine.MonoBehaviour
    {
        public const bool ShowWarnings = false;
        public static T HasInstance => instance;
        public static T GetInstance()
        {
            var r = instance ? instance : GameObject.FindObjectOfType<T> ();
            if (ShowWarnings && !r) Debug.LogWarning ("SingletonMonoBehaviour<" + typeof (T) + "> has no instance!");
            return r;
        }
        protected static T instance;
        protected virtual void Awake()
        {
            if (instance) throw new System.Exception (this + " is a singleton and is being added more than once.");
            instance = this as T;
        }
        protected virtual void OnDestroy() => instance = null;

        public static Transform Transform => GetInstance ().transform;
        public static GameObject GameObject => Transform.gameObject;
    }
}
