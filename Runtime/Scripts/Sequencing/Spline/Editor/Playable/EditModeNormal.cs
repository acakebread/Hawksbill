// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 09/06/2021 18:44:43 by seantcooper
using System.Linq;
using UnityEditor;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using Hawksbill.Geometry;
using static Hawksbill.Sequencing.SplineData;
using System.Collections.Generic;

namespace Hawksbill.Sequencing.SplineEdit
{
    ///<summary>Put text here to describe the Class</summary>
    public class EditModeNormal : EditMode
    {
        public override void sceneGUI()
        {
            base.sceneGUI ();

            if (!data || !data.hasNodes) return;
            var noise = SplineNoise.Enabled;
            SplineNoise.Enabled = false;

            drawSplines ();
            drawNodes ();
            drawScaleControls ();
            drawNodeControls ();
            SplineNoise.Enabled = noise;
            eventKeys ();
        }

        void eventKeys()
        {
            switch (Event.current.type)
            {
                case EventType.KeyDown:
                    switch (Event.current.keyCode)
                    {
                        case KeyCode.Delete:
                        case KeyCode.Backspace:
                            deleteNode ();
                            Event.current.Use ();
                            break;
                    }
                    break;
            }
        }

        public override void drawGUI()
        {
            base.drawGUI ();
            if (GUILayout.Button ("Delete Node"))
            {
                deleteNode ();
            }
        }

        void drawScaleControls()
        {
            if (!data.selectedNode) return;
            V3 position = playable.getPositionAt (data.selectedNode.index);
            Quaternion rotation = transform.multiply (data.selectedNode.rotation);

            var d = data.selectedNode.rotation * V3.forward;
            V3 cpIn = position - matrixRS.MultiplyVector (d * data.selectedNode.scale.In);
            V3 cpOut = position + matrixRS.MultiplyVector (d * data.selectedNode.scale.Out);

            Handles.color = Color.white;
            void draw(V3 cp, V3 pos, ref float scale)
            {
                Handles.DrawDottedLine (cp, pos, 4);
                EditorGUI.BeginChangeCheck ();
                var np = Handles.Slider2D (cp, rotation * V3.up, rotation * V3.right, rotation * V3.forward, SplineDraw.GetScale (cp), Handles.SphereHandleCap, 0);
                if (EditorGUI.EndChangeCheck ())
                {
                    RecordObject (data, "Change Spline Curve");
                    scale = matrixRS.inverse.MultiplyPoint (np - position).magnitude;
                    SetAllDirty ();
                }
            }

            if ((data.loop || data.selectedNode.index != 0) && cpIn != position)
                draw (cpIn, position, ref data.selectedNode.scale.In);
            if ((data.loop || data.selectedNode.index != data.nodes.Count - 1) && cpOut != position)
                draw (cpOut, position, ref data.selectedNode.scale.Out);
        }

        void drawNodeControls()
        {
            if (!data.selectedNode) return;
            V3 startPosition = playable.getPositionAt (data.selectedNode.index), position = startPosition;
            Quaternion startRotation = transform.multiply (data.selectedNode.rotation), rotation = startRotation;

            // position = Handles.PositionHandle (position, rotation);
            // if (startPosition != position)
            // {
            //     RecordObject (data, "Move Spline Node");
            //     selectedNode.position += matrixRS.inverse.MultiplyPoint (position - startPosition);
            //     SetAllDirty ();
            // }
            // rotation = Handles.RotationHandle (rotation, position);
            // if (startRotation != rotation)
            // {
            //     RecordObject (data, "Rotate Spline Node");
            //     selectedNode.rotation = transform.inverseMultiply (rotation);
            //     SetAllDirty ();
            // }

            EditorGUI.BeginChangeCheck ();
            Handles.TransformHandle (ref position, ref rotation);
            if (EditorGUI.EndChangeCheck ())
            {
                RecordObject (data, "Move Spline Node");
                data.selectedNode.position += matrixRS.inverse.MultiplyPoint (position - startPosition);
                data.selectedNode.rotation = transform.inverseMultiply (rotation);
                SetAllDirty ();
            }
        }

        void drawNodes()
        {
            Handles.color = Color.red;
            foreach (var n in data.nodes)
            {
                var position = transform.multiply (n.position);
                var size = SplineDraw.GetScale (position);
                if (n != data.selectedNode)
                {
                    if (Handles.Button (position, n.rotation, size, size, Handles.SphereHandleCap))
                        data.select (n);
                }
                else Handles.SphereHandleCap (0, position, n.rotation, size, EventType.Repaint);
            }
        }

        static GUIStyle _nodeNumber;
        static GUIStyle nodeNumber => _nodeNumber == null ?
            _nodeNumber = new GUIStyle (GUI.skin.label) { fontSize = 7, alignment = TextAnchor.LowerCenter } : _nodeNumber;

        void drawSplines()
        {
            var noise = SplineNoise.Enabled;
            SplineNoise.Enabled = false;
            SplineDraw drawer = new SplineDraw (playable);
            drawer.draw (1, false, false, true);
            SplineNoise.Enabled = true;
            drawer.clear ();
            drawer.draw (0.33f, false, true, false);
            SplineNoise.Enabled = noise;

            Handles.color = Color.green;
            for (float f = 0.5f; f < data.nodeRange; f++)
            {
                var position = transform.multiply (data.getPositionAt (f));
                var size = SplineDraw.GetScale (position);
                if (Handles.Button (position, Quaternion.identity, size * 0.6f, size, Handles.SphereHandleCap))
                {
                    addNode (data.getNodeAt (f));
                    break;
                }
            }

            foreach (Node node in data.nodes)
                Handles.Label (transform.multiply (node.position), node.index.ToString (), nodeNumber);
        }

        void deleteNode()
        {
            if (data.nodes.Count > 2 && data.selectedNode)
            {
                RecordObject (data, "Delete Spline Node");
                data.nodes.Remove (data.selectedNode);
                data.validate ();
                SetAllDirty ();
            }
        }

        void addNode(Node node)
        {
            RecordObject (data, "Add Spline Node");
            data.nodes.Insert ((int) node.index + 1, data.getNodeAt (node.index));
            data.select (node);
            data.validate ();
            SetAllDirty ();
        }
    }
}