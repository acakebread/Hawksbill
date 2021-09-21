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
//     public static class VoxelEdit_GUI
//     {
//         static EditMode _CurrentEditMode;
//         static EditMode CurrentEditMode => EditModes[EditModeIndex];
//         static EditMode[] EditModes = new EditMode[] {
//             new EditModePaste (), new EditModePicker (), new EditModeFill(), new EditModeExtrude(), new EditModeSelect(),
//             new EditModeAxis(), new EditModeSquare(), new EditModeCircle(), new EditModeLine(), new EditModeWand()
//  };
//         static int EditModeIndex;
//         static int ColorIndex;

//         public static void OnGUI(VoxelEdit voxelEdit)
//         {
//             GUILayout.Label ("This is a Label in a Custom Editor");
//             //var rect = GUILayoutUtility.GetLastRect ();
//             ColorIndex = GUIDrawers.PaletteControl.Draw (ColorIndex, (Texture2D) voxelEdit.voxel.material.mainTexture);
//             GUIDrawers.Space ();
//             EditModeIndex = GUILayout.SelectionGrid (EditModeIndex, Resources.GetIcons (EditModes.Select (e => e.icon)), 5, GUILayout.MaxHeight (80));
//             CurrentEditMode.OnGUI ();
//         }

//         public static void StartEdit(VoxelEdit voxelEdit)
//         {
//             EditMode.voxelEdit = voxelEdit;
//             CurrentEditMode.startEdit ();
//         }

//         public static void UpdateEdit(Vector3Int index) => CurrentEditMode.updateEdit (index);
//         public static void StopEdit() => CurrentEditMode.stopEdit ();

//         public class EditMode
//         {
//             internal static VoxelEdit voxelEdit;
//             public virtual int icon => 0;
//             public virtual void OnGUI() { }
//             public virtual void startEdit() { }
//             public virtual void updateEdit(Vector3Int index) { }
//             public virtual void stopEdit() { }
//         }

//         public class EditModePaste : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Brush;
//             UndoPaste undoPaste;
//             Array3D brush;
//             HashSet<VoxelMesh> meshesChanged;

//             public override void OnGUI()
//             {
//                 GUILayout.Label ("EditModePaste");
//             }

//             public override void startEdit()
//             {
//                 brush = Primitives.Sphere (2, 2, 2, (byte) ColorIndex);
//                 meshesChanged = new HashSet<VoxelMesh> ();
//                 undoPaste = new UndoPaste (voxelEdit);
//             }
//             public override void updateEdit(Vector3Int index)
//             {
//                 void invalidate(VoxelMesh mesh)
//                 {
//                     mesh.invalidate (VoxelMesh.Validation.Render);
//                     meshesChanged.Add (mesh);
//                 }
//                 voxelEdit.paste (brush, index).ForAll (m => invalidate (m));
//             }
//             public override void stopEdit()
//             {
//                 meshesChanged.ForAll (mesh => mesh.invalidate ());
//                 undoPaste.finish ();
//                 undoPaste = null;
//             }
//         }
//         public class EditModePicker : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Picker;
//             public override void OnGUI() { GUILayout.Label ("EditModePicker"); }
//         }
//         public class EditModeFill : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Fill;
//             public override void OnGUI() => GUILayout.Label ("EditModeFill");
//         }
//         public class EditModeExtrude : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Extrude;
//             public override void OnGUI() => GUILayout.Label ("EditModeExtrude");
//         }
//         public class EditModeSelect : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Select;
//             public override void OnGUI() => GUILayout.Label ("EditModeSelect");
//         }
//         public class EditModeAxis : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Axis;
//             public override void OnGUI() => GUILayout.Label ("EditModeAxis");
//         }
//         public class EditModeSquare : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Square;
//             public override void OnGUI() => GUILayout.Label ("EditModeSquare");
//         }
//         public class EditModeCircle : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Circle;
//             public override void OnGUI() => GUILayout.Label ("EditModeCircle");
//         }
//         public class EditModeLine : EditMode
//         {
//             public override int icon => (int) Resources.Icon.Line;
//             public override void OnGUI() => GUILayout.Label ("EditModeLine");
//         }
//         public class EditModeWand : EditMode
//         {
//             public override int icon => (int) Resources.Icon.MagicWand;
//             public override void OnGUI() => GUILayout.Label ("EditModeWand");
//         }

//         //UndoPaste undoPaste;
//         class UndoPaste
//         {
//             public int group;
//             public VoxelMesh[] meshes;
//             VoxelEdit edit;

//             public UndoPaste(VoxelEdit edit)
//             {
//                 this.edit = edit;
//                 Undo.SetCurrentGroupName ("Voxel Paste");
//                 group = Undo.GetCurrentGroup ();
//                 meshes = edit.meshes;
//                 Undo.RegisterFullObjectHierarchyUndo (edit.gameObject, "Voxel Paste");
//             }

//             public void finish()
//             {
//                 edit.meshes.Except (meshes).ForAll (m => Undo.RegisterCreatedObjectUndo (m.gameObject, "Remove mesh"));
//                 Undo.CollapseUndoOperations (group);
//             }
//         }
//     }
// }