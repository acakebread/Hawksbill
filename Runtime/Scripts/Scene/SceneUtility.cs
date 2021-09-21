// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Hawksbill
{
    public static class SceneUtility
    {
#if UNITY_EDITOR
        public static string GetDataAssetFullPath() =>
            Application.dataPath + "/" + System.String.Join ("/", GetDataAssetPath ().Split ('/').Skip (1));

        public static string GetDataAssetPath()
        {
            var scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene ();
            return System.String.Join ("/", scene.path.Split ('/').TakeLessOne ()) + "/" + scene.name;
        }
#endif
    }
}