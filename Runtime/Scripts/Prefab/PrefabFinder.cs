// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 16/01/2021 16:30:56 by seantcooper
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
namespace Hawksbill
{
    public static class PrefabFinder
    {
        public static IEnumerable<GameObject> GetPrefabs()
        {
            return Directory.GetFiles (Application.dataPath, "*", SearchOption.AllDirectories).
                Where (p => Path.GetExtension (p).ToLower () == ".prefab").
                Select (p => (UnityEngine.GameObject) AssetDatabase.LoadAssetAtPath (p.Substring (Application.dataPath.Length - 6), typeof (UnityEngine.GameObject)));
        }

        public static IEnumerable<T> GetPrefabs<T>() where T : MonoBehaviour => GetPrefabs ().Where (p => p).Select (p => p.GetComponent<T> ()).Where (t => t);
    }
}
#endif