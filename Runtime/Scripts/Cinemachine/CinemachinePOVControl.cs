// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 26/06/2021 14:25:05 by seantcooper
using UnityEngine;
using Hawksbill;
using Cinemachine;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class CinemachinePOVControl : Cinemachine.CinemachineExtension
    {
        public CinemachineVirtualCamera vcamera => GetComponent<CinemachineVirtualCamera> ();
        public int mouseButton = 0;
        public float zoomSpeed = 1;
        public string horizontalAxis = "Mouse X";
        public string verticalAxis = "Mouse Y";

        void Start() => CinemachineCore.GetInputAxis = GetAxisCustom;

        void Update()
        {
            var deltay = -Input.mouseScrollDelta.y;
            if (deltay != 0) vcamera.GetCinemachineComponent<CinemachineFramingTransposer> ().m_CameraDistance += deltay * zoomSpeed;
        }

        public float GetAxisCustom(string axisName)
        {
            if (axisName == horizontalAxis) return Input.GetMouseButton (mouseButton) ? UnityEngine.Input.GetAxis (horizontalAxis) : 0;
            else if (axisName == verticalAxis) return Input.GetMouseButton (mouseButton) ? UnityEngine.Input.GetAxis (verticalAxis) : 0;
            return UnityEngine.Input.GetAxis (axisName);
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
        }
    }
}