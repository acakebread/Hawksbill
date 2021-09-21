// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hawksbill
{
    public static class MaterialActions
    {
        [MenuItem ("Assets/Create/Hawksbill/Material/Create Material from Texture", true)]
        private static bool CreateMaterials_Validation() => ObjectActions.GetItems<Texture2D> ().Count () > 0;

        [MenuItem ("Assets/Create/Hawksbill/Material/Create Material from Texture", false, 0)]
        private static void CreateMaterials()
        {
            var items = ObjectActions.GetItems<Texture2D> ();
            if (EditorUtility.DisplayDialog ("Create Materials", "Create materials from Texture?\n" + String.Join ("\n", items.Select (i => i.name)), "Yes", "No"))
            {
                IEnumerable<Material> _createAssets()
                {
                    foreach (var item in items)
                    {
                        Material material = new Material (Shader.Find ("Standard")) { mainTexture = item.obj as Texture2D };
                        AssetDatabase.CreateAsset (material, item.folder + "/" + item.name + ".mat");
                        yield return material;
                    }
                }
                Selection.objects = _createAssets ().ToArray ();
            }
        }
    }
}
