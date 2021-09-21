// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Hawksbill.IO;
using UnityEngine.Timeline;
// using System.IO;

namespace Hawksbill
{
    public static class AssetFolders
    {
        [MenuItem ("Assets/Create/Hawksbill/Folders/Create Asset Folders", true)]
        private static bool CreateAssetFoldersValidation() => !IsSelectedPathEmpty;
        [MenuItem ("Assets/Create/Hawksbill/Folders/Create Asset Folders", false, 0)]
        private static void CreateAssetFolders()
        {
            var paths = new string[] {
                "Animation",
                "Audio",
                "Data",
                // "Data/Lighting",
                // "Data/Splines",
                // "Data/Volumes",
                // "Data/Waves",
                "Materials",
                "Models",
                "Prefabs",
                "Scenes",
                "Scripts",
                "Shaders",
                "Textures",
            };

            if (EditorUtility.DisplayDialog ("Asset Folders", "Are you sure you want to create all assets folders: " + String.Join (",", paths) + "?", "Yes", "No"))
            {
                var folder = SelectedPath.folderPath;
                foreach (var path in paths)
                {
                    var split = Path.GetPathAndFilename (path);
                    var nfolder = split[0] != "" ? folder + split[0] : folder;
                    var npath = split[1];
                    if ((nfolder + npath).exists) continue;
                    Debug.Log ("Creating Folder: " + nfolder.assetPath + "/" + npath);
                    AssetDatabase.CreateFolder (nfolder.assetPath, npath);
                    System.IO.File.WriteAllText (nfolder + npath + "/.gitkeep", "");
                }
            }
        }

        [MenuItem ("Assets/Create/Hawksbill/Folders/Organise Assets into Folders", true)]
        private static bool OrganiseAssetsIntoFoldersValidation() => !IsSelectedPathEmpty;
        [MenuItem ("Assets/Create/Hawksbill/Folders/Organise Assets into Folders", false, 0)]
        private static void OrganiseAssetsIntoFolders()
        {
            var folders = new Dictionary<string, Type[]> ()
            {
                { "Animation", new Type[] { typeof(AnimationClip), typeof(TimelineAsset) } },
                { "Prefabs", new Type[] { typeof(GameObject) } },
                { "Textures", new Type[] { typeof(Texture) } },
                { "Materials", new Type[] { typeof(Material) } },
                { "Data", new Type[] { typeof(ScriptableObject) } },
            };

            Debug.Log ("SelectedPath.folderPath = " + SelectedPath.folderPath);

            var objects = AssetDatabase.LoadAllAssetsAtPath (SelectedPath.folderPath);
            var files = System.IO.Directory.GetFiles (SelectedPath.folderPath);
            Debug.Log ("List all files:");
            foreach (var file in files)
            {
                if (!System.IO.File.Exists (file)) continue;
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object> (((Path) file).assetPath);
                Debug.Log (file + " " + (asset ? asset.GetType ().FullName : "null"));
                if (!asset) continue;
                {
                    foreach (var kv in folders)
                    {
                        string newPath = null;
                        foreach (var type in kv.Value)
                        {
                            if (asset.GetType () == type || asset.GetType ().IsSubclassOf (type))
                            {
                                newPath = kv.Key;
                                break;
                            }
                        }

                        if (newPath != null)
                        {
                            Debug.Log ("--" + newPath);
                            AssetDatabase.CreateFolder (SelectedPath.folderPath, newPath);
                            break;
                        }
                    }
                }
            }

            // var paths = new string[] {
            //     "Animation",
            //     "Audio",
            //     "Data",
            //     // "Data/Lighting",
            //     // "Data/Splines",
            //     // "Data/Volumes",
            //     // "Data/Waves",
            //     "Materials",
            //     "Models",
            //     "Prefabs",
            //     "Scenes",
            //     "Scripts",
            //     "Shaders",
            //     "Textures",
            // };

            // if (EditorUtility.DisplayDialog ("Asset Folders", "Are you sure you want to create all assets folders: " + String.Join (",", paths) + "?", "Yes", "No"))
            // {
            //     var folder = SelectedPath.folderPath;
            //     foreach (var path in paths)
            //     {
            //         var split = Path.GetPathAndFilename (path);
            //         var nfolder = split[0] != "" ? folder + split[0] : folder;
            //         var npath = split[1];
            //         if ((nfolder + npath).exists) continue;
            //         Debug.Log ("Creating Folder: " + nfolder.assetPath + "/" + npath);
            //         AssetDatabase.CreateFolder (nfolder.assetPath, npath);
            //         System.IO.File.WriteAllText (nfolder + npath + "/.gitkeep", "");
            //     }
            // }
        }

        [MenuItem ("Assets/Create/Hawksbill/Folders/Remove Empty Folders (recursively)", true)]
        private static bool RemoveEmptyFoldersValidation() => !IsSelectedPathEmpty;

        [MenuItem ("Assets/Remove Empty Folders (recursively)", false, 0)] //Create/Hawksbill/Folders/
        private static void RemoveEmptyFolders()
        {
            if (EditorUtility.DisplayDialog ("Empty Folders", "Are you sure you want to remove all empty folders recursively?", "Yes", "No"))
            {
                IEnumerable<Path> paths;
                while ((paths = GetEmptyFolders (SelectedPath)).Count () > 0)
                {
                    foreach (var path in paths)
                    {
                        Debug.Log ("Removing Folder: " + path);
                        AssetDatabase.DeleteAsset (path.assetPath);
                    }
                }
            }
        }

        static IEnumerable<Path> GetEmptyFolders(Path path)
        {
            var folders = System.IO.Directory.GetDirectories (path, "*", System.IO.SearchOption.AllDirectories).Select (p => new Path (p));
            return folders.Where (p => System.IO.Directory.GetFiles (p).Count (f => !f.Contains (".gitkeep")) == 0);
        }

        // public IEnumerable<Path> getFolders(bool recursive = true) =>
        //        Directory.GetDirectories (path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select (p => new Path (p));
        // public IEnumerable<Path> getEmptyFolders(bool recursive = true) =>
        //         getFolders (recursive).Where (p => Directory.GetFiles (p).Length == 0);

        // [MenuItem ("Assets/Create/Hawksbill/Folders/List Empty Folders (recursively)", true)]
        // private static bool ListEmptyFoldersValidation() => !IsSelectedPathEmpty;
        // [MenuItem ("Assets/Create/Hawksbill/Folders/List Empty Folders (recursively)", false, 0)]
        // private static void ListEmptyFolders() => SelectedPath.getEmptyFolders ().ForAll (path => Debug.Log (path));

        static Path SelectedPath => new Path (Application.dataPath + AssetDatabase.GetAssetPath (Selection.activeObject).Substring ("assets".Length));
        static bool IsSelectedPathEmpty => AssetDatabase.GetAssetPath (Selection.activeObject) == "";
    }
}
