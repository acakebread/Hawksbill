// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 28/06/2021 20:22:05 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEditor;

namespace Hawksbill
{
    public static class MeshX
    {
        // public static Mesh CreateInstance(Mesh mesh, string path = "")
        // {
        //     path = EditorUtility.SaveFilePanel ("Save Spline Data", path == "" ? AssetDatabase.GetAssetPath (target) : path, target.name, "asset");
        //     return path.Length == 0 ? null : CreateAsset (UnityEngine.Object.Instantiate (target), path);
        // }

        // static ScriptableObject CreateAsset(ScriptableObject instance, string path)
        // {
        //     string uniquePath = AssetDatabase.GenerateUniqueAssetPath (((Hawksbill.IO.Path) path).assetPath);
        //     AssetDatabase.CreateAsset (instance, uniquePath);
        //     AssetDatabase.SaveAssets ();
        //     AssetDatabase.Refresh ();
        //     return instance;
        // }
    }
}