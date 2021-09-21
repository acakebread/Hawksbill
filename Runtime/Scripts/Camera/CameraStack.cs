// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 15/09/2021 09:24:04 by seantcooper
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Hawksbill
{
    [ExecuteInEditMode]
    public class CameraStack : MonoBehaviour
    {
        void LateUpdate()
        {
            var camera = GetComponent<Camera> ();
            if (camera)
            {
                var cameraData = camera.GetUniversalAdditionalCameraData ();
                foreach (var stackCamera in cameraData.cameraStack)
                {
                    stackCamera.fieldOfView = camera.fieldOfView;
                    stackCamera.nearClipPlane = camera.nearClipPlane;
                    stackCamera.farClipPlane = camera.farClipPlane;
                }
            }
        }
    }
}