// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    public class GlobalManager : SingletonMonoBehaviour<GlobalManager>
    {
        public new static GlobalManager GetInstance() => instance ? instance : CreateInstance ();
        static GlobalManager CreateInstance() => (new GameObject ("Hawksbill.GlobalManager")).AddComponent<GlobalManager> ();

        public static bool HasComponent<T>() where T : MonoBehaviour =>
            GetInstance ().GetComponent<T> ();

        public static T GetOrAddComponent<T>() where T : MonoBehaviour =>
            GetInstance ().TryGetComponent (out T component) ? component : GetInstance ().gameObject.AddComponent<T> ();

    }
}
