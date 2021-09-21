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
    ///<summary>Draw Spline Edit Mode</summary>
    public class EditModeDraw : EditMode
    {
        // Node prevNode => data.selectedNode == null ? data.nodes.Last () : (data.selectedNode == data.nodes.First () ? null : data.nodes[(int) selectedNode.index - 1]);
        // Node nextNode => data.selectedNode == null ? data.nodes.First () : (data.selectedNode == data.nodes.Last () ? null : data.nodes[(int) selectedNode.index + 1]);

        public override void start()
        {
            base.start ();
            //if (data.nodes.Count () == 0) 
            addNode ();
        }

        public override void sceneGUI()
        {
            var noise = SplineNoise.Enabled;
            SplineNoise.Enabled = false;
            base.sceneGUI ();

            int controlID = GUIUtility.GetControlID (FocusType.Passive);

            if (Event.current.type == EventType.Layout)
                HandleUtility.AddDefaultControl (controlID);

            switch (Event.current.type)
            {
                case EventType.MouseMove: mouseMove (); break;
                case EventType.MouseDown: mouseDown (); break;
                case EventType.KeyDown: keyDowns (); break;
            }

            if (Event.current.type == EventType.Repaint)
            {
                // data.drawSpline (playable, 1, true, false, false);
                var drawer = new SplineDraw (playable);
                drawer.draw (1, true, false, true);
            }
            SplineNoise.Enabled = noise;
        }

        void keyDowns()
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.Escape:
                case KeyCode.Return:
                    exit ();
                    break;
                case KeyCode.Delete:
                case KeyCode.Backspace:
                    removeLast ();
                    break;
            }
        }

        public override void drawGUI()
        {
            base.drawGUI ();
            if (GUILayout.Button ("Clear All")) clear ();
            if (GUILayout.Button ("Exit (Esc)")) exit ();
        }

        protected override void exit()
        {
            removeLast ();
            SetAllDirty ();
            base.exit ();
        }

        List<Node> queue;
        void clear() { data.nodes.Clear (); playable.validate (); }
        void addNode() => addNode (data.nodes.Count);
        void addNode(int index)
        {
            Node node;
            if (index >= data.nodes.Count - 1) data.nodes.Add (node = new Node ());
            else data.nodes.Insert (index, node = new Node ());
            if (queue == null) queue = new List<Node> ();
            queue.Add (node);
            playable.validate ();
        }

        void removeLast()
        {
            if (data.nodes.Count > 0)
            {
                data.nodes.RemoveAt (data.nodes.Count - 1);
                playable.validate ();
            }
        }

        void mouseMove()
        {
            if (data.nodes.Count >= 2 && getMouseWorldPosition (Event.current.mousePosition, out V3 position))
            {
                var rnodes = ((IEnumerable<Node>) data.nodes).Reverse ().ToArray (); //reverse nodes
                if (data.nodes.Count == 2)
                {
                    Direction d = new Direction (rnodes[0].position - rnodes[1].position);
                    rnodes[0].position = position;
                    rnodes[0].rotation = rnodes[1].rotation = d.rotation;
                    rnodes[0].scale.In = rnodes[1].scale.Out = d.length / 2;
                }
                else
                {
                    rnodes[0].position = position;
                    rnodes[0].rotation = Quaternion.LookRotation (rnodes[0].position - rnodes[1].cpOut);
                    rnodes[0].scale.In = rnodes[1].scale.Out = (rnodes[0].position - rnodes[1].position).magnitude / 2;
                }
                data.validate ();
            }
        }

        void mouseDown()
        {
            if (getMouseWorldPosition (Event.current.mousePosition, out V3 position))
            {
                if (data.nodes.Count == 0) addNode ();
                data.nodes.Last ().position = position;
                addNode ();
                data.nodes.Last ().position = position;
                SetAllDirty ();
                //Event.current.Use ();
            }
        }
    }
}