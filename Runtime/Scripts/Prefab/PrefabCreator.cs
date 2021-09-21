// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 13/02/2021 09:35:57 by seantcooper
using UnityEngine;
using Hawksbill;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class PrefabCreator : MonoBehaviour
    {
        public GameObject prefab;
        [TextArea] public string names;
        [Button] public bool create;

        void OnValidate()
        {
            if (create)
            {
                create = false;
#if UNITY_EDITOR
                EditorApplication.update += run;
#endif
            }
        }

        void duplicate(string name)
        {
#if UNITY_EDITOR
            var gameObject = Instantiate (prefab);
            gameObject.name = name;
            // DestroyImmediate (gameObject.GetComponent<PrefabCreator> ());

            // // Keep track of the currently selected GameObject(s)
            // GameObject[] objectArray = Selection.gameObjects;

            // Set the path as within the Assets folder,
            // and name it as the GameObject's name with the .Prefab format
            string localPath = "Assets/" + gameObject.name + ".prefab";

            // Make sure the file name is unique, in case an existing Prefab has the same name.
            localPath = AssetDatabase.GenerateUniqueAssetPath (localPath);

            // Create the new Prefab.
            PrefabUtility.SaveAsPrefabAsset (gameObject, localPath);
#endif
        }

        void run()
        {
#if UNITY_EDITOR
            EditorApplication.update -= run;
            names.Split (',').ForAll (name => duplicate (name));
#endif
        }


    }
}