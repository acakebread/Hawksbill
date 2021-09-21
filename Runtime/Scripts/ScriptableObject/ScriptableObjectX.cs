// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Linq;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    public static class ScriptableObjectX
    {
#if UNITY_EDITOR
        /// <summary> This makes it easy to create, name and place unique new ScriptableObject asset files. </summary>
        public static T CreateInstance<T>(string path = "Assets") where T : ScriptableObject
        {
            string name = typeof (T).Name;
            path = EditorUtility.SaveFilePanel ("Save " + name, path, "New " + name, "asset");
            if (path.Length > 0) return (T) CreateAsset (ScriptableObject.CreateInstance<T> (), path);
            return null;
        }

        public static ScriptableObject CreateInstance(ScriptableObject target, string path = "")
        {
            string name = target.GetType ().Name;
            path = EditorUtility.SaveFilePanel ("Save " + name, path == "" ? AssetDatabase.GetAssetPath (target) : path, target.name, "asset");
            return path.Length == 0 ? null : CreateAsset (UnityEngine.Object.Instantiate (target), path);
        }

        static ScriptableObject CreateAsset(ScriptableObject instance, string path)
        {
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath (((Hawksbill.IO.Path) path).assetPath);
            AssetDatabase.CreateAsset (instance, uniquePath);
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh ();
            return instance;
        }

        // /// <summary> This makes it easy to create, name and place unique new ScriptableObject asset files. </summary>
        // public static T CreateAsset<T>() where T : ScriptableObject
        // {
        //     var path = SceneUtility.GetDataAssetPath ();
        //     var fullpath = SceneUtility.GetDataAssetFullPath ();

        //     if (!Directory.Exists (fullpath))
        //     {
        //         Debug.Log (fullpath);
        //         Directory.CreateDirectory (fullpath);
        //     }

        //     T asset = ScriptableObject.CreateInstance<T> ();
        //     string namedPath = path + "/New " + typeof (T).Name + ".asset";
        //     string uniquePath = AssetDatabase.GenerateUniqueAssetPath (namedPath);
        //     AssetDatabase.CreateAsset (asset, uniquePath);
        //     AssetDatabase.SaveAssets ();
        //     AssetDatabase.Refresh ();
        //     EditorUtility.FocusProjectWindow ();
        //     Selection.activeObject = asset;
        //     return asset;
        // }
#endif
    }
}