// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.IO;
// using UnityEditor;
// using UnityEngine;
// using Unity.Mathematics;
// using V3 = UnityEngine.Vector3;
// using V2 = UnityEngine.Vector2;
// using static Hawksbill.SplineData;

// namespace Hawksbill.Voxel
// {
//     [CustomEditor (typeof (VoxelEdit))]
//     public class VoxelEdit_Editor : Editor
//     {
//         [MenuItem ("CONTEXT/VoxelEdit/", false, 150)]
//         static void Space() { }
//         [MenuItem ("CONTEXT/VoxelEdit/Load", false, 151)]
//         static void Load() { }
//         [MenuItem ("CONTEXT/VoxelEdit/Save", false, 152)]
//         static void Save() { }
//         [MenuItem ("CONTEXT/VoxelEdit/Clear All", false, 153)]
//         static void ClearAll()
//         {
//             Undo.RegisterFullObjectHierarchyUndo (Selection.activeGameObject, "Voxel Clear All");

//             var meshes = Selection.activeGameObject.GetComponentsInChildren<VoxelMesh> (true);
//             meshes.ForAll (m => DestroyImmediate (m.gameObject));

//             Debug.Log (">>Clear " + Selection.activeGameObject.name + " " + meshes.Length);
//         }

//         Tool lastTool;
//         Event e => Event.current;
//         static Camera camera => UnityEditor.SceneView.lastActiveSceneView.camera;
//         VoxelEdit voxelEdit => (VoxelEdit) target;
//         VoxelX voxel => voxelEdit.GetComponent<VoxelX> ();

//         void OnEnable() { lastTool = Tools.current; Tools.current = Tool.None; }
//         void OnDisable() { Tools.current = lastTool; }

//         protected virtual void OnSceneGUI()
//         {
//             Handles.BeginGUI ();
//             GUILayout.Label ("Voxel Editing");
//             Handles.EndGUI ();

//             updateCursor ();

//             if (e.type == EventType.Layout) HandleUtility.AddDefaultControl (GUIUtility.GetControlID (GetHashCode (), FocusType.Passive));
//             processEvents ();


//         }

//         bool editing = false;
//         void processEvents()
//         {
//             int controlID = GUIUtility.GetControlID (FocusType.Passive);
//             switch (e.GetTypeForControl (controlID))
//             {
//                 case EventType.MouseDown:
//                     if (e.button == 0 && e.modifiers == EventModifiers.None)
//                     {
//                         editing = true;
//                         VoxelEdit_GUI.StartEdit (voxelEdit);
//                         e.Use ();
//                     }
//                     break;
//                 case EventType.MouseDrag:
//                     if (editing && voxelEdit.getEditIndex (HandleUtility.GUIPointToWorldRay (Event.current.mousePosition), out Vector3Int index))
//                     {
//                         VoxelEdit_GUI.UpdateEdit (index);
//                         e.Use ();
//                     }
//                     break;
//                 case EventType.MouseUp:
//                     if (editing) // && e.button == 0 && e.modifiers == EventModifiers.None)
//                     {
//                         VoxelEdit_GUI.StopEdit ();
//                         e.Use ();
//                         editing = false;
//                     }
//                     break;
//             }
//         }

//         // 3D
//         void updateCursor()
//         {
//             const string name = "cursor";
//             var cursor = voxel.transform.Find (name);
//             if (voxelEdit.getEditIndex (HandleUtility.GUIPointToWorldRay (Event.current.mousePosition), out Vector3Int index))
//             {
//                 if (cursor == null) cursor = MeshUtility.CreateObject (new Array3D (1, 1, 1, 1), name, voxel.material, voxel.transform, 0.01f).transform;
//             }
//             if (cursor)
//                 cursor.localPosition = index;
//         }

//         // GUI
//         //public override void OnInspectorGUI() => VoxelEdit_GUI.OnGUI (voxelEdit);
//     }
// }