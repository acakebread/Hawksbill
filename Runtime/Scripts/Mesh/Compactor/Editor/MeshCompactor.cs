// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 20/07/2021 09:14:57 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class MeshCompactor_Menu
    {
        static int FrameCount = -1;

        [MenuItem ("GameObject/Hawksbill/Mesh/Compact mesh filters", true)]
        static bool Check() { return SelectedGameObjects.Count () > 0; }

        [MenuItem ("GameObject/Hawksbill/Mesh/Compact mesh filters")]
        static void Compact()
        {
            if (Time.frameCount == FrameCount) return;
            switch (EditorUtility.DisplayDialogComplex ("Mesh Compactor", "Compacts all Meshes to a single mesh. Note: only supports single materials that are the same!",
                "Compact in Scene", "Compact to Prefab", "Do nothing"))
            {
                case 0: // scene
                    Selection.objects = run (false).ToArray ();
                    break;
                case 1: // prefab
                    Selection.objects = run (true).ToArray ();
                    break;
                case 2: // cancel
                    break;
            }
            FrameCount = Time.frameCount;
        }

        static IEnumerable<GameObject> run(bool write)
        {
            foreach (var g in SelectedGameObjects)
                yield return MeshCompactor.Compact (g.transform, write);
        }

        static IEnumerable<GameObject> SelectedGameObjects => Selection.gameObjects.Where (g => MeshCompactor.IsCompactable (g));
    }
}