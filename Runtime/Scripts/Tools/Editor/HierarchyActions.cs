// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/07/2021 15:35:17 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEditor;
using System.Linq;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class HierarchyActions
    {
        [MenuItem ("GameObject/Hawksbill/Remove inactive GameObjects")]
        static void RemoveDisabled()
        {
            Debug.Log ("GameObjects selected: " + Selection.gameObjects.Length);
            var gameObjects = Selection.gameObjects.SelectMany (g => g.GetComponentsInChildren<Transform> (true).
                Where (t => !t.gameObject.activeSelf).Select (t => t.gameObject)).ToArray ();
            if (gameObjects.Length == 0) return;

            if (EditorUtility.DisplayDialog ("Asset Folders", "Deleting " + gameObjects.Length + " GameObjects. Are you sure?", "Yes", "No"))
            {
                foreach (var gameObject in gameObjects)
                {
                    if (gameObject)
                    {
                        Debug.Log ("Destroy GameObject " + gameObject.name);
                        GameObject.DestroyImmediate (gameObject);
                    }
                }
            }
        }

        // struct UnityTransform
        // {
        //     public Transform transform;
        //     public void unpackPrefab()
        //     {
        //         PrefabUtility.UnpackPrefabInstance (transform.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        //     }
        // }


    }
}