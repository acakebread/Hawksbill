// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 06/05/2021 17:19:58 by seantcooper
// using UnityEngine;
// using Hawksbill;
// using Cinemachine;
// using System;
// using UnityEngine.EventSystems;

// namespace Hawksbill
// {
//     public class CinemachineFreelookControl : Cinemachine.CinemachineExtension
//     {
//         //public CinemachineCore.Stage applyAfter;

//         public MouseButton mouseButton = MouseButton.None;
//         [Range (10, 90)] public float defaultFov = 40;
//         [ObjectColumns (28)] public MinMax fovZoomRange = new MinMax (15, 50);
//         public bool invertZoom = false;

//         CinemachineFreeLook freeLook => GetComponent<CinemachineFreeLook> ();
//         float scrollZoomDelta => -Input.mouseScrollDelta.y * (invertZoom ? -1 : 1);

//         protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
//         {
//             // if (Input.mouseScrollDelta.y != 0)
//             // {

//             // }
//         }

//         void OnValidate()
//         {
//             defaultFov = fovZoomRange.clamp (defaultFov);
//         }

//         void Start()
//         {
//             if (freeLook) freeLook.m_Lens.FieldOfView = defaultFov;
//             CinemachineCore.GetInputAxis = GetAxisCustom;
//         }

//         void Update()
//         {
//             if (freeLook)
//             {
//                 if (EventSystem.current.IsPointerOverGameObject ()) return;
//                 freeLook.m_Lens.FieldOfView = fovZoomRange.clamp (freeLook.m_Lens.FieldOfView + scrollZoomDelta);

//             }
//         }

//         public float GetAxisCustom(string axisName)
//         {
//             if (axisName == "Mouse X") return isMouseDown ? UnityEngine.Input.GetAxis ("Mouse X") : 0;
//             else if (axisName == "Mouse Y") return isMouseDown ? UnityEngine.Input.GetAxis ("Mouse Y") : 0;
//             return UnityEngine.Input.GetAxis (axisName);
//         }

//         //bool isMouseDown => mouseButton == MouseButton.None ? true :
//         //    (!EventSystem.current.IsPointerOverGameObject () && Input.GetMouseButton ((int) mouseButton));

//         System.Collections.Generic.Dictionary<int, bool> _isFocussed = new System.Collections.Generic.Dictionary<int, bool> ();
//         bool isFocussed(int nButton) => Input.GetMouseButtonDown (nButton) ? _isFocussed[nButton] = !EventSystem.current.IsPointerOverGameObject () : _isFocussed.ContainsKey (nButton) ? _isFocussed[nButton] : false;

//         bool isMouseDown => mouseButton == MouseButton.None ? true :
//            isFocussed ((int) mouseButton) && Input.GetMouseButton ((int) mouseButton);

//         public enum MouseButton
//         {
//             None = -1,
//             Left = 0,
//             Right = 1
//         }

//         [Serializable]
//         public struct MinMax
//         {
//             public float min, max;
//             public MinMax(float min, float max) { this.min = min; this.max = max; }
//             public float clamp(float v) => Mathf.Clamp (v, min, max);
//         }
//     }
// }