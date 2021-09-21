// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 20/01/2021 22:18:00 by acakebread

namespace Hawksbill
{
    using UnityEngine;

    [ExecuteAlways]
    public class CameraMatcher : MonoBehaviour
    {
        public Camera sourceCamera;
        public Camera destinationCamera;

        void OnValidate()
        {
            sourceCamera = Camera.main;
            destinationCamera = GetComponent<Camera> ();
        }

        void OnPreRender()
        {
            if (destinationCamera && sourceCamera)
            {
                destinationCamera.fieldOfView = sourceCamera.fieldOfView;
                destinationCamera.transform.position = sourceCamera.transform.position;
                destinationCamera.transform.rotation = sourceCamera.transform.rotation;
            }
        }
    }
}