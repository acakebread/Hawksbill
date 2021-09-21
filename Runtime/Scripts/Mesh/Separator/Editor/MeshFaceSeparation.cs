// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 06/08/2021 09:09:16 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class MeshFaceSeparation_Menu
    {
        static int FrameCount = -1;

        [MenuItem ("GameObject/Hawksbill/Mesh/Face separation", true)]
        static bool Check() { return SelectedGameObjects.Count () > 0; }

        [MenuItem ("GameObject/Hawksbill/Mesh/Face separation")]
        static void SeperateFaces()
        {
            if (Time.frameCount == FrameCount) return;
            FrameCount = Time.frameCount;

            switch (EditorUtility.DisplayDialogComplex ("Mesh Face Separation", "Separate Mesh into coplanar faces!",
                "Seperate in Scene", "Write Mesh assets", "Do nothing"))
            {
                case 0: // scene
                    Selection.objects = run (false).ToArray ();
                    break;
                case 1: // asset
                    Selection.objects = run (true).ToArray ();
                    break;
                case 2: // cancel
                    break;
            }
        }

        static IEnumerable<GameObject> run(bool write) => SelectedGameObjects.Select (g => MeshFaceSeparation.Separate (g, write));
        static IEnumerable<GameObject> SelectedGameObjects => Selection.gameObjects.Where (g => MeshFaceSeparation.IsCompactable (g));
    }
}