// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 09/02/2021 09:00:27 by seantcooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hawksbill
{
    public static class AssetCache
    {
        static Dictionary<string, Info> Prefabs = new Dictionary<string, Info> ();

        public static bool LoadPrefab(string guid, out Info info)
        {
            if (!Prefabs.ContainsKey (guid))
            {
                string path = AssetDatabase.GUIDToAssetPath (guid);
                if (path.Length >= 8 && path.Substring (path.Length - 7).ToLower () == ".prefab")
                {
                    Prefabs[guid] = new Info
                    {
                        guid = guid,
                        path = path,
                        obj = AssetDatabase.LoadAssetAtPath (path, typeof (GameObject)) as GameObject
                    };
                }
                else Prefabs[guid] = null;
            }
            info = Prefabs[guid];
            return info && info.isValid;
        }

        public class Info
        {
            public string guid;
            public string path;
            public UnityEngine.Object obj;
            public bool isValid => obj;
            public static implicit operator bool(Info empty) => empty != null;
        }
    }
}