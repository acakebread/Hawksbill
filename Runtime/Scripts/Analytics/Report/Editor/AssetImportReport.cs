using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawksbill.IO;
using UnityEditor;
using UnityEngine;

class AssetImportReport : AssetPostprocessor
{
    // public const string DefaultHeader = "// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created #DATETIME# by #USERNAME#";
    // static bool IncludeAllImported = false;
    // static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    // {
    //     foreach (Path path in assets)
    //     {
    //         Debug.Log("Importing " + )
    //     }
    // }
    void OnPostprocessTexture(Texture2D texture)
    {
    }

    // void OnPreprocessModel()
    // {
    //     ModelImporter importer = assetImporter as ModelImporter;
    //     if (null != importer) importer.meshCompression = ModelImporterMeshCompression.High;
    // }

    void OnPostprocessModel(GameObject model)
    {
        var parent = PrefabUtility.GetCorrespondingObjectFromSource (model);
        string path = AssetDatabase.GetAssetPath (parent);
        Debug.Log ("Importing model " + model.name + " " + path);

        int totalVertex = 0, totalTriangles = 0;
        foreach (var mesh in model.GetComponentsInChildren<MeshFilter> ().Select (f => f.sharedMesh))
        {
            totalVertex += mesh.vertexCount;
            totalTriangles += mesh.triangles.Length / 3;
        }
        var text = "Importing model " + model.name + " " + path + " total vertices: " + totalVertex + " total triangles: " + totalTriangles;
        if (totalTriangles > 100000) Debug.LogError ("[POLYCOUNT > 100000] " + text);
        else if (totalTriangles >= 50000) Debug.LogWarning ("[POLYCOUNT > 50000] " + text);
    }

}