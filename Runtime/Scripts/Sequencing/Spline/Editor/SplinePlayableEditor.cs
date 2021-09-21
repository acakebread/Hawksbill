// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System.Linq;
using UnityEditor;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using Hawksbill.Geometry;
using static Hawksbill.Sequencing.SplineData;
using System.Collections.Generic;
using Hawksbill.Reflection;
using System;
using Hawksbill.Analytics;

namespace Hawksbill.Sequencing.SplineEdit
{
    [CanEditMultipleObjects]
    [CustomEditor (typeof (SplinePlayable), true)]
    public partial class SplinePlayableEditor : Editor
    {
        static Plane FloorPlane = new Plane (V3.up, V3.zero);

        void OnEnable() => Tools.current = Tool.None;

        SplinePlayable playable => (target as SplinePlayable);
        SplineData data => playable.data;

        protected void OnSceneGUI()
        {
            if (!currentEditMode) startEditMode<EditModeNormal> ();

            if (Event.current.type == EventType.Repaint)
            {
                currentEditMode?.sceneGUI ();
                drawGUI ();
            }
            else
            {
                drawGUI ();
                currentEditMode?.sceneGUI ();
            }
        }

        void drawGUI()
        {
            Handles.BeginGUI ();
            GUILayout.BeginArea (new Rect (10, 10, 100, Screen.height - 20));
            GUILayout.BeginVertical ();

            if (EditMode.Button ("Freehand Draw", !(currentEditMode as EditModeDraw)))
                startEditMode<EditModeDraw> ();

            currentEditMode?.drawGUI ();

            GUILayout.EndVertical ();
            GUILayout.EndArea ();
            Handles.EndGUI ();
        }

        EditMode currentEditMode;
        void startEditMode<T>() where T : EditMode
        {
            if (currentEditMode != null && typeof (T) == currentEditMode.GetType ()) return;
            currentEditMode = Activator.CreateInstance<T> ();
            currentEditMode.editor = this;
            currentEditMode.onExit += () => currentEditMode = null;
            currentEditMode.start ();
        }

        // Extentions
        Type[] _extensions;
        Type[] extensions => _extensions == null ?
            _extensions = AssemblyX.GetAllTypesInheriting (typeof (SplineExtension)).Where (t => !t.IsAbstract).ToArray () : _extensions;

        // Inspector GUI
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector ();
            GUILayout.Space (10);
            GUILayout.Label ("Extensions", EditorStyles.boldLabel);
            var extensions = this.extensions.Where (e => playable.GetComponent (e) == null).ToArray ();
            var names = new string[] { "(select)" }.Concat (extensions.Select (e => e.Name)).ToArray ();
            int index = EditorGUILayout.Popup ("Add Extension", 0, names);
            if (index != 0) playable.gameObject.AddComponent (extensions[index - 1]);
        }
    }
}

