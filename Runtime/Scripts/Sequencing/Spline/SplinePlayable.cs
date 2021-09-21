// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System.Linq;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using static Hawksbill.Sequencing.SplineData;
using System;
using Hawksbill.Geometry;
using System.Collections.Generic;
using Hawksbill.Analytics;
using Hawksbill.Sequencing.SplineEdit;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill.Sequencing
{
    [ExecuteInEditMode]
    public class SplinePlayable : MonoBehaviour
    {
        public bool hide = false;
        public SplineData data;
        [Line]
        public float _position;
        public float position { get => _position; set => onPositionChanged (_position = value); }
        public ComponentFilter.Mask filter = ComponentFilter.Mask.All;

        void Start() { }
        void Update()
        {
            FPSTracker.Log ("Position", position);
#if UNITY_EDITOR
            _transforms = null;
#endif
        }

        public float time => GetComponent<SplineTimeProvider> ()?.time ?? Time.time;
        public event Action positionChanged;

        protected virtual void onPositionChanged(float position)
        {
            if (!data || !data.hasNodes) return;
            setTransformAt (_position, filter);
            positionChanged?.Invoke ();
        }

        public void validate() => OnValidate ();
        protected virtual void OnValidate()
        {
            if (data) data.validate ();
            position = _position;
        }

        // INTERFACE
        public TransformBase getTransformAt(float f)
        {
            f = data.scope (f);
            V3 p1 = getPositionAt (f - RotationDistance), p2 = getPositionAt (f + RotationDistance);
            return new TransformBase ((p1 + p2) * 0.5f, Quaternion.LookRotation (p2 - p1));
        }

        public V3 getPositionAt(float f) => transformPosition (data.getPositionAt (f));

        public void setTransformAt(float f) => setTransformAt (f, ComponentFilter.Mask.All);
        public void setTransformAt(float f, ComponentFilter.Mask filter)
        {
            if (!data) return;
            var transform = getTransformAt (f);
            this.transform.SetPositionAndRotation (ComponentFilter.FilterPosition (this.transform.position, transform.position, filter),
                Quaternion.Euler (ComponentFilter.FilterRotation (this.transform.eulerAngles, transform.eulerAngles, filter)));
        }

        ISplineExTransformable[] _transforms;
        IEnumerable<ISplineExTransformable> transforms => _transforms == null ? _transforms = GetComponents<ISplineExTransformable> ().OrderBy (c => c.priority).ToArray () : _transforms;
        public V3 transformPosition(V3 position)
        {
            foreach (ISplineExTransformable t in transforms) position = t.transformPosition (position);
            return position;
        }
        public V3[] transformPositions(V3[] positions) => transforms.Aggregate (positions, (r, t) => t.transformPositions (r));

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if (!data || !data.hasNodes || hide) return;
            if (Selection.activeGameObject != gameObject)
            {
                var drawer = new SplineDraw (this);
                drawer.draw (1, true, false, true);
                if (Event.current.type == EventType.MouseUp)
                    if (drawer.nearSpline (HandleUtility.GUIPointToWorldRay (Event.current.mousePosition), out V3 closest, out float distance))
                        SelectionX.ForceSelection (gameObject);
                drawer.drawPosition (_position);
            }
        }
#endif

    }
}