// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created date by seancooper
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using Hawksbill.IO;

class ImportScript : AssetPostprocessor
{
    public const string DefaultHeader = "// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created #DATETIME# by #USERNAME#";
    static bool IncludeAllImported = false;
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        var assets = importedAssets.Select (p => new Path (p)).Where (p => p.extension == ".cs").ToArray ();
        if (assets.Length == 0) return;
        foreach (Path path in assets)
        {
            if (!IncludeAllImported && path.createdAge > new TimeSpan (0, 0, 30)) continue;

            var file = System.IO.File.ReadAllText (path);
            var lines = file.Split (new string[] { "\n" }, StringSplitOptions.None);
            if (lines.Length > 0)
            {
                var line = lines[0];

                if (IncludeAllImported && !line.StartsWith ("//"))
                {
                    line = DefaultHeader;
                    lines = new string[] { line }.Concat (lines).ToArray ();
                }

                line = line.Replace ("#USERNAME#", Environment.UserName);
                line = line.Replace ("#DATETIME#", DateTime.Now.ToString ());

                if (line != lines[0])
                {
                    lines[0] = line;
                    file = String.Join ("\n", lines);
                    Debug.Log ("Writing file: " + path);
                    System.IO.File.WriteAllText (path, file);
                }
            }
        }
    }
}