// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 19/02/2021 09:21:59 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class AudioCameraHook : MonoBehaviour
    {
        [Disable] public new Camera camera;
        void Update()
        {
            camera = Camera.main;
            if (camera) transform.SetPositionAndRotation (camera.transform.position, camera.transform.rotation);
        }
    }
}