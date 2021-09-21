// // Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 19/05/2021 12:12:03 by seantcooper
// using UnityEngine;
// using Hawksbill;
// using V3 = UnityEngine.Vector3;
// using System.Linq;
// using Hawksbill.Geometry;
// using Hawksbill.Analytics;
// using System.Collections.Generic;
// using static Hawksbill.Sequencing.SplineData;

// #if UNITY_EDITOR
// using UnityEditor;
// #endif

// namespace Hawksbill.Sequencing
// {
//     public static class SplineDrawers
//     {
// #if UNITY_EDITOR
//         public const float ZScale = 0.1f;
//         public static float GetScale(V3 position) => HandleUtility.GetHandleSize (position) * ZScale;
//         const float NodeScale = 0.6f;
// #endif

//         public static void drawSpline(this SplineData data, SplinePlayable playable, float alpha = 1,
//             bool showNodes = true, bool showGround = false, bool showMarkers = false)
//         {
// #if UNITY_EDITOR
//             var matrix = Handles.matrix;
//             var normal = Camera.current.transform.forward;

//             foreach (var node in data.nodes.Take ((int) data.nodeRange))
//             {
//                 Handles.color = new Color (data.color.r, data.color.g, data.color.b, data.color.a * alpha);
//                 Handles.matrix = Matrix4x4.identity;
//                 V3[] positions = playable.transformPositions (data.getPositions (node).ToArray ());
//                 int count = positions.Length;

//                 Handles.DrawPolyLine (positions.Take (count).ToArray ());

//                 if (showMarkers) EnumerableUtility.Range (count / 10, count, count / 10).ForAll (i => drawSphere (positions[i], 0.3f));

//                 if (showNodes) drawSphere (positions[0], NodeScale);

//                 if (showGround)
//                 {
//                     Handles.color = new Color (Handles.color.r, Handles.color.g, Handles.color.b, Handles.color.a * 0.25f);
//                     Handles.matrix = Matrix4x4.Scale (new V3 (1, 0, 1));
//                     Handles.DrawPolyLine (positions);
//                 }
//             }
//             Handles.matrix = matrix;
// #endif
//         }

//         public static bool nearSpline(this SplineData data, SplinePlayable playable, Ray ray, out V3 position, out float distance)
//         {
//             bool ret = false;
//             position = V3.zero;
//             distance = float.MaxValue;
//             if (data.nodes.Count < 2) return ret;
// #if UNITY_EDITOR
//             V3[] line = new V3[] { ray.origin, ray.origin + ray.direction * 1000 };
//             float minDistance = float.MaxValue;

//             foreach (var node in data.nodes.Take ((int) data.nodeRange))
//             {
//                 V3[] positions = playable.transformPositions (data.getPositions (node).ToArray ());
//                 int count = positions.Length;

//                 var thickness = Mathf.Max (GetScale (positions[0]), GetScale (positions.Last ())) * NodeScale;
//                 var dist = positions.getShortestDistance (line, out V3 closest, data.loop);
//                 minDistance = Mathf.Min (dist, minDistance);
//                 if (dist < thickness)
//                 {
//                     position = closest;
//                     ret = true;
//                 }
//             }
// #endif
//             return ret;
//         }

// #if UNITY_EDITOR
//         static void drawCircle(V3 worldPosition, V3 normal, float scale = 0.5f) =>
//            Handles.DrawSolidDisc (worldPosition, normal, GetScale (worldPosition) * scale);

//         static void drawCube(Vector3 worldPosition, float scale = 0.5f) =>
//            Handles.CubeHandleCap (0, worldPosition, Quaternion.identity, GetScale (worldPosition) * scale, EventType.Repaint);

//         static void drawSphere(Vector3 worldPosition, float scale = 0.5f) =>
//            Handles.SphereHandleCap (0, worldPosition, Quaternion.identity, GetScale (worldPosition) * scale, EventType.Repaint);
// #endif

//         public static void drawPosition(this SplineData data, SplinePlayable playable, float position) =>
//            DrawPosition (playable.getPositionAt (position));

//         public static void DrawPosition(Vector3 worldPosition, float scale = 0.5f) =>
//             Gizmos.DrawCube (worldPosition, V3.one * GetScale (worldPosition) * scale);

//         public static void DrawPosition(Vector3 worldPosition, Color color, float scale = 0.5f)
//         {
//             Gizmos.color = color;
//             Gizmos.DrawCube (worldPosition, V3.one * GetScale (worldPosition) * scale);
//         }
//     }
// }