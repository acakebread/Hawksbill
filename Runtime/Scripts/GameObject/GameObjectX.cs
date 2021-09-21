// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:04:23 by seancooper
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Hawksbill
{
    public static class GameObjectX
    {
        public static bool inHierarchy(this GameObject g) => g.scene.IsValid ();

        public static bool GetComponent<T>(this GameObject c, out T component, bool add) where T : Component
        {
            component = c.GetComponent<T> ();
            if (add && !component) component = c.AddComponent<T> ();
            return component;
        }

        public static T instantiate<T>(this T prefab, Action<T> initialize = null) where T : UnityEngine.Object =>
            prefab.instantiate (Vector3.zero, Quaternion.identity, null, initialize);

        public static T instantiate<T>(this T prefab, Transform parent, Action<T> initialize = null) where T : UnityEngine.Object =>
            parent ? prefab.instantiate (parent.position, parent.rotation, parent, initialize) : prefab.instantiate (initialize);

        public static T instantiate<T>(this T prefab, Vector3 position, Quaternion rotation, Transform parent = null, Action<T> initialize = null) where T : UnityEngine.Object
        {
            var o = UnityEngine.Object.Instantiate (prefab, position, rotation, parent);
            o.name = prefab.name;
            initialize?.Invoke (o);
            return o;
        }

        static Dictionary<string, Transform> groups = new Dictionary<string, Transform> ();

        public static T instantiateInGroup<T>(this T prefab, Action<T> initialize = null) where T : UnityEngine.Object =>
            prefab.instantiateInGroup<T> (Vector3.zero, Quaternion.identity, initialize);

        public static T instantiateInGroup<T>(this T prefab, Vector3 position, Quaternion rotation, Action<T> initialize = null) where T : UnityEngine.Object
        {
            if (!groups.ContainsKey (prefab.name)) groups[prefab.name] = new GameObject (prefab.name + " (group)").transform;
            return prefab.instantiate (position, rotation, groups[prefab.name], initialize);
        }

        public static IEnumerable<GameObject> children(this GameObject gameObject)
        {
            foreach (var child in gameObject.transform)
                yield return ((Transform) child).gameObject;
        }
        public static void children(this GameObject gameObject, Action<GameObject> action)
        {
            foreach (var child in gameObject.transform)
                action (((Transform) child).gameObject);
        }

        /// <summary>Invokes a Private/Public method in all Components in this GameObject that are type T!</summary>
        public static void message<T>(this GameObject gameObject, string methodName, params object[] parameters) =>
            gameObject.GetComponents<Component> ().Where (c => c is T).ForAll (c => c.message<T> (methodName, parameters));

        //EDITOR
#if UNITY_EDITOR
        public static bool isSelectedInEditor(this GameObject gameObject) =>
            UnityEditor.Selection.activeGameObject == gameObject;
#else
         public static bool isSelectedInEditor(this GameObject gameObject) => false;
#endif

    }
}

// public static void destroyImmediate(this GameObject gameObject) => GameObject.DestroyImmediate (gameObject);

// public static string getSceneFolder(this GameObject gameObject)
// {
//     // var scenePath = gameObject.scene.path;
//     // var folder = Path.Combine (Path.GetDirectoryName (scenePath), Path.GetFileNameWithoutExtension (scenePath));
//     // if (!Directory.Exists (folder)) Directory.CreateDirectory (folder);
//     // return Path.Combine (Path.GetDirectoryName (scenePath), Path.GetFileNameWithoutExtension (scenePath)) + "/";
// }