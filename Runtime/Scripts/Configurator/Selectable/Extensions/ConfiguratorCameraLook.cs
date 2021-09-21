// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/08/2021 18:01:44 by seantcooper
using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Hawksbill.Geometry;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using QT = UnityEngine.Quaternion;

namespace Hawksbill.Configurator
{
    ///<summary>Controls and manages the Virtual Camera</summary>
    [ExecuteInEditMode]
    [AddComponentMenu ("")]
    //[RequireComponent (typeof (ConfiguratorSelectable))]
    public sealed class ConfiguratorCameraLook : ConfiguratorCamera
    {
        const float MinDistance = 1, MaxDistance = 20, MinAngle = -180, MaxAngle = 180;
        [Line]
        [SerializeField] Vector3 offset;
        [SerializeField] Mouse.Button mouseButton = Mouse.Button.Right;
        [SerializeField] Axis horizontalAxis = new Axis ("Mouse X", true);
        [SerializeField] Axis verticalAxis = new Axis ("Mouse Y", false);
        [SerializeField] Zoom zoom;

        Axis[] axes => new Axis[] { horizontalAxis, verticalAxis };

        V3 camPosition => camera.transform.localPosition;
        V3 camTarget => offset;
        V3 camDirection => camRotation * V3.forward;
        QT camRotation => Quaternion.Euler (verticalAxis.localAngle, horizontalAxis.localAngle, 0);
        float camDistance => Vector3.Distance (camPosition, camTarget);

        protected override void setView(ConfiguratorCameraView.View view)
        {
            if (!view) return;
            zoom.distance = view.ZoomValue;
            horizontalAxis.setAngle (view.horizontalValue);
            verticalAxis.setAngle (view.verticalValue);
        }

        void Update()
        {
            camera.LookAt = camera.Follow = null;
            zoom.update ();
            if (Application.isPlaying) axes.ForAll (a => a.update (mouseButton));
#if UNITY_EDITOR
            else axes.ForAll (a => a.setAngle (a.angle));
#endif
            camera.transform.localPosition = camTarget + camRotation * Vector3.back * zoom.distance;
            camera.transform.localRotation = Quaternion.LookRotation (camTarget - camera.transform.localPosition);
        }

        [Serializable]
        public class Zoom
        {
            [SerializeField] [Range (0.01f, 10)] float _speed = 0.5f;
            [SerializeField] float _distance = 10;
            public float distance { get => _distance; set => _distance = range.clamp (value); }
            [MinMax (MinDistance, MaxDistance)] public MinMaxFloat range = new MinMaxFloat (MinDistance, MaxDistance);
            public void update() => distance = distance - Mouse.WheelDeltaY * _speed;
        }

        [Serializable]
        public class Axis
        {
            public Axis(string inputAxisName, bool inverted)
            {
                this.inputAxisName = inputAxisName;
                this.inverted = inverted;
            }

            public string inputAxisName;
            public float inputAxisValue => -Input.GetAxis (inputAxisName);

            [HideInInspector, SerializeField] bool inverted;

            [Tooltip ("The default direction!")]
            public float origin = 0;
            public float angle = 0;
            internal float delta = 0;
            public bool infinateRotation => range.min == MinAngle && range.max == MaxAngle;

            public void setAngle(float angle)
            {
                this.angle = constrain (angle);
            }

            [MinMax (MinAngle, MaxAngle)] public MinMaxFloat range = new MinMaxFloat (-180, 180);
            [Range (0.1f, 20)] public float speed = 10f;

            public float localAngle => inverted ? origin - angle : origin + angle;
            public float localMin => inverted ? origin - range.min : origin + range.min;
            public float localMax => inverted ? origin - range.max : origin + range.max;

            public void update(Mouse.Button button)
            {
                if (button == Mouse.Button.None || Mouse.IsMousePressed (button))
                {
                    if (!Mouse.InputUsable) return;
                    float newValue = constrain (angle + speed * inputAxisValue);
                    delta = (delta + ((newValue == angle) ? 0 : (newValue - angle) / Time.deltaTime)) / 2;
                    angle = newValue;
                }
                else if (delta != 0)
                {
                    delta *= 0.9f;
                    if (Mathf.Abs (delta) < 0.0001f) delta = 0;
                    angle = constrain (angle + delta * Time.deltaTime);
                }
            }

            public float constrain(float value) =>
                infinateRotation ? value : range.clamp (value);
        }

        void OnDrawGizmosSelected()
        {
            if (!gameObject.isSelectedInEditor ()) return;
            Gizmos.matrix = transform.localToWorldMatrix;
            const float sphereR = 0.1f;
            Color cLight = new Color (1, 1, 1, 0.33f), cBold = new Color (1, 1, 1, 1);

            // DISTANCE
            Gizmos.color = cLight;
            Gizmos.DrawLine (camTarget, camTarget - camDirection * zoom.distance);
            Gizmos.color = cBold;
            V3 min = camTarget - camDirection * zoom.range.min, max = camTarget - camDirection * zoom.range.max;
            Gizmos.DrawLine (min, max);
            Gizmos.DrawSphere (min, sphereR);
            Gizmos.DrawSphere (max, sphereR);

            void drawArc(V3 e1, V3 e2, float d, bool bold = false, bool drawLines = false)
            {
                Gizmos.color = bold ? cBold : cLight;
                var step = (1 / Math.Max (Mathf.Abs ((e2 - e1).x), Mathf.Abs ((e2 - e1).y))) * 5;
                var points = EnumerableUtility.Range (0, 1, step).Select (f => QT.Euler (V3.Lerp (e1, e2, f)) * V3.back * d).ToArray ();
                points.Skip (1).ForAll ((p, i) => Gizmos.DrawLine (p, points[i]));

                if (drawLines)
                {
                    Gizmos.color = cLight;
                    Gizmos.DrawLine (points.First (), camTarget);
                    Gizmos.DrawSphere (points.First (), sphereR);
                    Gizmos.DrawLine (points.Last (), camTarget);
                    Gizmos.DrawSphere (points.Last (), sphereR);
                }
            }


            float vMin = verticalAxis.localMin, vMax = verticalAxis.localMax, vAng = verticalAxis.localAngle;
            float hMin = horizontalAxis.localMin, hMax = horizontalAxis.localMax, hAng = horizontalAxis.localAngle;

            cLight = new Color (0, 1, 0, cLight.a);
            cBold = new Color (0, 1, 0, cBold.a);
            drawArc (new V3 (vAng, hMin, 0), new V3 (vAng, hMax, 0), zoom.range.min);
            drawArc (new V3 (vAng, hMin, 0), new V3 (vAng, hMax, 0), zoom.range.max, false, true);
            drawArc (new V3 (vAng, hMin, 0), new V3 (vAng, hMax, 0), zoom.distance, true);

            cLight = new Color (1, 0, 0, cLight.a);
            cBold = new Color (1, 0, 0, cBold.a);
            drawArc (new V3 (vMin, hAng, 0), new V3 (vMax, hAng, 0), zoom.distance, true, true);
        }
    }
}