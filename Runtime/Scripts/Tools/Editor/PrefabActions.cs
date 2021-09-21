// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 16/07/2021 09:08:08 by seantcooper
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hawksbill
{
    public static class PrefabActions
    {
        [MenuItem ("Assets/Create/Hawksbill/Prefab/Create Prefab from Object", true)]
        private static bool CreateMaterials_Validation() => ObjectActions.GetItems<GameObject> ().Count () > 0;

        [MenuItem ("Assets/Create/Hawksbill/Prefab/Create Prefab from Object", false, 0)]
        private static void CreateMaterials()
        {
            var items = ObjectActions.GetItems<GameObject> ();
            if (EditorUtility.DisplayDialog ("Create Prefabs", "Create prefabs from Objects?\n" + String.Join ("\n", items.Select (i => i.name)), "Yes", "No"))
            {
                IEnumerable<GameObject> _createAssets()
                {
                    foreach (var item in items)
                    {
                        GameObject obj = item.obj as GameObject;
                        GameObject gameObject = new GameObject (obj.name);
                        PrefabUtility.InstantiatePrefab (obj, gameObject.transform).name = obj.name;

                        GameObject prefab = PrefabUtility.SaveAsPrefabAsset (gameObject, item.folder + "/" + item.name + ".prefab", out bool success);
                        UnityEngine.Object.DestroyImmediate (gameObject);
                        if (success) yield return prefab;
                    }
                }
                Selection.objects = _createAssets ().ToArray ();
            }
        }
    }
}