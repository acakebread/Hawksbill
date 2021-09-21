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
//     public static class GUIDrawers
//     {
//         public class PaletteControl
//         {
//             static Rect rect;
//             public static int Draw(int index, Texture2D texture)
//             {
//                 GUIStyle style = new GUIStyle { normal = { background = texture } };
//                 if (GUILayout.Button ("", style, GUILayout.MinHeight (160)))
//                 {
//                     var uv = math.floor (math.clamp ((Event.current.mousePosition - rect.position) / rect.size, 0, 1) * 16);
//                     index = (int) (uv.y * 16 + uv.x);

//                     V2 getUV(int color)
//                     {
//                         const float scalar = 1f / 16f, step = scalar / 2;
//                         return new V2 (((color & 0xf)) * scalar + step, ((color >> 4)) * scalar + step);
//                     }
//                     var uvo = getUV (index);
//                     var i = (int) (uvo.x * 16) + (int) (uvo.y * 16) * 16;
//                     Debug.Log ("Start edit " + index + " " + i + " " + uvo.x + " " + uvo.y);

//                 }
//                 if (Event.current.type == EventType.Repaint)
//                 {
//                     rect = GUILayoutUtility.GetLastRect ();
//                     Rect position = new Rect (new Vector2 (index & 15, index >> 4) * rect.size / 16 + rect.position, rect.size / 16);
//                     GUIDrawers.DrawOutline (position, new Color (1, 1, 1, 0.8f), 2);
//                 }
//                 return index;
//             }
//         }

//         static Material _material;
//         static Material material => _material ? _material : _material = new Material (Shader.Find ("Hidden/Internal-Colored"));

//         public static void Space() => GUILayout.Label ("", GUILayout.MaxHeight (4));

//         public static void DrawOutline(Rect rect, Color color, float thickness = 1)
//         {
//             V2 t = V2.one * thickness;
//             rect.min -= t / 2; rect.max += t / 2;
//             V2[] r1 = new V2[] { V2.zero, rect.size }, r2 = new V2[] { r1[0] + t, r1[1] - t };
//             GUI.BeginClip (rect);
//             material.SetPass (0);
//             GL.Begin (GL.QUADS);
//             GL.Color (color);
//             GL.Vertex3 (r1[0].x, r1[0].y, 0); GL.Vertex3 (r1[1].x, r1[0].y, 0); GL.Vertex3 (r2[1].x, r2[0].y, 0); GL.Vertex3 (r2[0].x, r2[0].y, 0);
//             GL.Vertex3 (r1[1].x, r1[0].y, 0); GL.Vertex3 (r1[1].x, r1[1].y, 0); GL.Vertex3 (r2[1].x, r2[1].y, 0); GL.Vertex3 (r2[1].x, r2[0].y, 0);
//             GL.Vertex3 (r1[1].x, r1[1].y, 0); GL.Vertex3 (r1[0].x, r1[1].y, 0); GL.Vertex3 (r2[0].x, r2[1].y, 0); GL.Vertex3 (r2[1].x, r2[1].y, 0);
//             GL.Vertex3 (r1[0].x, r1[1].y, 0); GL.Vertex3 (r1[0].x, r1[0].y, 0); GL.Vertex3 (r2[0].x, r2[0].y, 0); GL.Vertex3 (r2[0].x, r2[1].y, 0);
//             GL.End ();
//             GUI.EndClip ();
//         }
//     }
// }
