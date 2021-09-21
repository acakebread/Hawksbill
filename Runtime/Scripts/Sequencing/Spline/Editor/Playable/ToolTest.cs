// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 10/06/2021 21:36:26 by seantcooper
// using UnityEngine;
// using Hawksbill;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEditor.EditorTools;
// using UnityEngine.Rendering;
// using Hawksbill.Geometry;
// using static Hawksbill.Sequencing.SplineData;

// using V3 = UnityEngine.Vector3;

// // By passing `typeof(MeshFilter)` as the second argument, we register VertexTool as a CustomEditor tool to be presented
// // when the current selection contains a MeshFilter component.
// namespace Hawksbill.Sequencing.SplineEdit
// {
//     [EditorTool ("Test for Playable", typeof (SplinePlayable))]
//     class SplineEditTest : EditorTool
//     {
//         GUIContent m_ToolbarIcon;

//         public SplinePlayable playable => target as SplinePlayable;
//         public SplineData data => playable.data;

//         public override GUIContent toolbarIcon
//         {
//             get
//             {
//                 if (m_ToolbarIcon == null)
//                     m_ToolbarIcon = new GUIContent (
//                         AssetDatabase.LoadAssetAtPath<Texture2D> ("Assets/Hawksbill/Scripts/Sequencing/Spline/Gizmos/X Icon.png"),
//                         "Test Tool");
//                 return m_ToolbarIcon;
//             }
//         }

//         void OnEnable()
//         {
//             ToolManager.activeToolChanged += ActiveToolDidChange;
//         }

//         void OnDisable()
//         {
//             ToolManager.activeToolChanged -= ActiveToolDidChange;
//         }

//         void ActiveToolDidChange()
//         {
//             if (!ToolManager.IsActiveTool (this))
//                 return;

//             // m_Vertices = targets.Select (x => {
//             //     return new TransformAndPositions ()
//             //     {
//             //         transform = ((MeshFilter) x).transform,
//             //         positions = ((MeshFilter) x).sharedMesh.vertices
//             //     };
//             // }).ToArray ();
//         }

//         public override void OnToolGUI(EditorWindow window)
//         {
//             test ();
//             Debug.Log ("Tool " + Tools.current.ToString ());
//             // var evt = Event.current;

//             // if (evt.type == EventType.Repaint)
//             // {
//             //     // var zTest = Handles.zTest;
//             //     // Handles.zTest = CompareFunction.LessEqual;

//             //     // foreach (var entry in m_Vertices)
//             //     // {
//             //     //     foreach (var vertex in entry.positions)
//             //     //     {
//             //     //         var world = entry.transform.TransformPoint (vertex);
//             //     //         Handles.DotHandleCap (0, world, Quaternion.identity, HandleUtility.GetHandleSize (world) * .05f, evt.type);
//             //     //     }
//             //     // }

//             //     // Handles.zTest = zTest;
//             // }
//         }

//         protected SplineTransform splineTransform;
//         protected TransformBase transform;
//         protected Matrix4x4 matrixRS;

//         protected bool getMouseWorldPosition(V3 mousePosition, out V3 point)
//         {
//             var ray = HandleUtility.GUIPointToWorldRay (mousePosition);
//             if (Physics.Raycast (ray, out RaycastHit hit)) { point = hit.point; return true; }
//             else if (new Plane (V3.up, V3.zero).Raycast (ray, out float enter)) { point = ray.GetPoint (enter); return true; }
//             point = V3.zero;
//             return false;
//         }


//         void test()
//         {
//             this.splineTransform = playable.GetComponent<SplineTransform> ();
//             this.transform = splineTransform?.splineTransform ?? new TransformBase ();
//             this.matrixRS = transform.matrixRS;
//             //  base.sceneGUI ();

//             int controlID = GUIUtility.GetControlID (FocusType.Passive);

//             if (Event.current.type == EventType.Layout)
//                 HandleUtility.AddDefaultControl (controlID);

//             switch (Event.current.type)
//             {
//                 case EventType.MouseMove: mouseMove (); break;
//                 case EventType.MouseDown: mouseDown (); break;
//                 case EventType.KeyDown: keyDowns (); break;
//             }

//             if (Event.current.type == EventType.Repaint)
//                 data.drawSpline (playable, 1, true, false, false);
//         }

//         // public override void drawGUI()
//         // {
//         //     if (GUILayout.Button ("Clear All")) clear ();
//         //     if (GUILayout.Button ("Exit")) exit ();
//         // }

//         // protected override void exit()
//         // {
//         //     removeLast ();
//         //     SetAllDirty ();
//         //     base.exit ();
//         // }

//         void clear() => data.nodes.Clear ();
//         void addNode() => data.nodes.Add (new Node { index = data.nodes.Count });
//         void removeLast() { if (data.nodes.Count > 0) data.nodes.RemoveAt (data.nodes.Count - 1); }

//         void keyDowns()
//         {
//             switch (Event.current.keyCode)
//             {
//                 case KeyCode.Escape:
//                 case KeyCode.Return:
//                     //exit ();
//                     break;
//                 case KeyCode.Delete:
//                 case KeyCode.Backspace:
//                     removeLast ();
//                     break;
//             }
//         }

//         void mouseMove()
//         {
//             if (data.nodes.Count >= 2 && getMouseWorldPosition (Event.current.mousePosition, out V3 position))
//             {
//                 var rnodes = ((IEnumerable<Node>) data.nodes).Reverse ().ToArray (); //reverse nodes
//                 if (data.nodes.Count == 2)
//                 {
//                     Direction d = new Direction (rnodes[0].position - rnodes[1].position);
//                     rnodes[0].position = position;
//                     rnodes[0].rotation = rnodes[1].rotation = d.rotation;
//                     rnodes[0].scale.In = rnodes[1].scale.Out = d.length / 2;
//                 }
//                 else
//                 {
//                     rnodes[0].position = position;
//                     rnodes[0].rotation = Quaternion.LookRotation (rnodes[0].position - rnodes[1].cpOut);
//                     rnodes[0].scale.In = rnodes[1].scale.Out = (rnodes[0].position - rnodes[1].position).magnitude / 2;
//                 }
//                 data.validate ();
//             }
//         }

//         void mouseDown()
//         {
//             if (getMouseWorldPosition (Event.current.mousePosition, out V3 position))
//             {
//                 if (data.nodes.Count == 0) addNode ();
//                 data.nodes.Last ().position = position;
//                 addNode ();
//                 data.nodes.Last ().position = position;
//                 //SetAllDirty ();
//                 //Event.current.Use ();
//             }

//         }
//     }
// }