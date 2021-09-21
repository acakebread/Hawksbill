// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 09/06/2021 18:44:01 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEditor;
using V3 = UnityEngine.Vector3;
using System;
using Hawksbill.Geometry;

namespace Hawksbill.Sequencing.SplineEdit
{
    ///<summary>Base Edit mode class</summary>
    public class EditMode
    {
        public SplinePlayableEditor editor;
        public SplinePlayable playable => editor.target as SplinePlayable;
        public SplineData data => playable.data;
        public event Action onExit;

        protected SplineTransform splineTransform;
        protected TransformBase transform;
        protected Matrix4x4 matrixRS;

        protected bool getMouseWorldPosition(V3 mousePosition, out V3 point)
        {
            var ray = HandleUtility.GUIPointToWorldRay (mousePosition);
            if (Physics.Raycast (ray, out RaycastHit hit)) { point = hit.point; return true; }
            else if (new Plane (V3.up, V3.zero).Raycast (ray, out float enter)) { point = ray.GetPoint (enter); return true; }
            point = V3.zero;
            return false;
        }

        public virtual void start() { }
        public virtual void sceneGUI()
        {
            this.splineTransform = playable.GetComponent<SplineTransform> ();
            this.transform = splineTransform?.splineTransform ?? new TransformBase ();
            this.matrixRS = transform.matrixRS;
        }

        public virtual void drawGUI() { GUILayout.Space (4); }
        protected virtual void exit() => onExit?.Invoke ();

        protected void RecordObject(UnityEngine.Object undoObject, string message) => Undo.RecordObject (undoObject, message);
        protected void SetAllDirty()
        {
            EditorUtility.SetDirty (data);
            EditorUtility.SetDirty (playable);
            playable.validate ();
        }

        public static implicit operator bool(EditMode empty) => empty != null;

        public static bool Button(string name, bool enabled = true)
        {
            using (new GUIHelpers.Enable (enabled))
            {
                bool state = GUILayout.Button (name);
                if (state) Event.current.Use ();
                return state;
            }
        }
    }
}