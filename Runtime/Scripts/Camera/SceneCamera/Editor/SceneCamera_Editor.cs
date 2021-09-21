// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:01 by seancooper
using UnityEditor;
using UnityEngine;

namespace Hawksbill.Cameras
{
    [CustomEditor (typeof (SceneCamera))]
    public class SceneCamera_Editor : Editor
    {
        SceneCamera sceneCamera => (SceneCamera) target;
        protected virtual void OnSceneGUI()
        {
            if (!sceneCamera.enabled) return;
            sceneCamera.copySceneToGameCamera ();
        }
    }
}

// UnityEngine.Camera gameView = UnityEngine.Camera.main;
// UnityEngine.Camera sceneView = UnityEditor.SceneView.lastActiveSceneView.camera;

// gameView.orthographicSize = sceneView.orthographicSize;
// gameView.orthographic = sceneView.orthographic;

// gameView.nearClipPlane = sceneView.nearClipPlane;
// gameView.farClipPlane = sceneView.farClipPlane;

// gameView.fieldOfView = sceneView.fieldOfView;
// gameView.transform.localPosition = sceneView.transform.localPosition;
// gameView.transform.localRotation = sceneView.transform.localRotation;
// gameView.transform.localScale = sceneView.transform.localScale;
