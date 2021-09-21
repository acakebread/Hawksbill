// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 11/06/2021 08:58:56 by seantcooper
using UnityEngine;
using Hawksbill;
using V3 = UnityEngine.Vector3;
using System.Linq;
using Hawksbill.Geometry;
using Hawksbill.Analytics;
using System.Collections.Generic;
using static Hawksbill.Sequencing.SplineData;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill.Sequencing.SplineEdit
{
    public class SplineDraw
    {
#if UNITY_EDITOR
        public const float ZScale = 0.1f;
        public static float GetScale(V3 position) => HandleUtility.GetHandleSize (position) * ZScale;
        public const float NodeScale = 0.6f;
#else
       public static float GetScale(V3 position) => 1;
#endif

        SplinePlayable playable;
        SplineData data => playable.data;
        IEnumerable<Node> nodeRange => data.nodes.Take ((int) data.nodeRange);

        public void clear() => cache = null;

        Dictionary<Node, V3[]> cache;
        V3[] getPositions(Node node)
        {
            if (cache == null) cache = new Dictionary<Node, V3[]> ();
            if (!cache.ContainsKey (node)) cache[node] = playable.transformPositions (data.getPositions (node).ToArray ());
            return cache[node];
        }

        public SplineDraw(SplinePlayable playable)
        {
            this.playable = playable;
        }

        public void draw(float alpha = 1, bool nodes = true, bool ground = false, bool markers = false)
        {
            drawSpline (alpha);
            if (nodes) drawNodes (alpha);
            if (markers) drawMarkers (alpha);
            if (ground) drawGround (alpha * 0.25f);
        }

        public void drawSpline(float alpha = 1)
        {
#if UNITY_EDITOR
            Handles.color = new Color (data.color.r, data.color.g, data.color.b, data.color.a * alpha);
            nodeRange.ForAll (n => Handles.DrawPolyLine (getPositions (n)));
#endif
        }

        public void drawNodes(float alpha = 1)
        {
#if UNITY_EDITOR
            Handles.color = new Color (data.color.r, data.color.g, data.color.b, data.color.a * alpha);
            nodeRange.ForAll (n => drawSphere (getPositions (n)[0], alpha));
#endif
        }

        public void drawMarkers(float alpha = 0.3f)
        {
#if UNITY_EDITOR
            Handles.color = new Color (data.color.r, data.color.g, data.color.b, data.color.a * alpha);
            nodeRange.ForAll (n => getPositions (n).Where ((p, i) => (i % 10) == 0 && i != 0).ForAll (p => drawSphere (p, 0.3f)));
#endif
        }

        public void drawGround(float alpha = 0.25f)
        {
#if UNITY_EDITOR
            var matrix = Handles.matrix;
            Handles.matrix = Matrix4x4.Scale (new V3 (1, 0, 1));
            Handles.color = new Color (data.color.r, data.color.g, data.color.b, data.color.a * alpha);
            nodeRange.ForAll (n => Handles.DrawPolyLine (getPositions (n)));
            Handles.matrix = matrix;
#endif
        }

        public bool nearSpline(Ray ray, out V3 position, out float distance)
        {
            bool ret = false;
            position = V3.zero;
            distance = float.MaxValue;
            if (data.nodes.Count < 2) return ret;
#if UNITY_EDITOR
            V3[] line = new V3[] { ray.origin, ray.origin + ray.direction * 1000 };
            float minDistance = float.MaxValue;

            foreach (var node in nodeRange)
            {
                V3[] positions = getPositions (node);
                var thickness = Mathf.Max (GetScale (positions[0]), GetScale (positions.Last ())) * NodeScale;
                var dist = positions.getShortestDistance (line, out V3 closest, data.loop);
                minDistance = Mathf.Min (dist, minDistance);
                if (dist < thickness)
                {
                    position = closest;
                    ret = true;
                }
            }
#endif
            return ret;
        }

#if UNITY_EDITOR
        static void drawCircle(V3 position, V3 normal, float scale = 0.5f) =>
           Handles.DrawSolidDisc (position, normal, GetScale (position) * scale);

        static void drawCube(Vector3 position, float scale = 0.5f) =>
           Handles.CubeHandleCap (0, position, Quaternion.identity, GetScale (position) * scale, EventType.Repaint);

        static void drawSphere(Vector3 position, float scale = 0.5f) =>
           Handles.SphereHandleCap (0, position, Quaternion.identity, GetScale (position) * scale, EventType.Repaint);
#endif

        public void drawPosition(float position) => drawPosition (playable.getPositionAt (position));

        public void drawPosition(Vector3 position, float scale = 0.5f) =>
            Gizmos.DrawCube (position, V3.one * GetScale (position) * scale);

        public static void DrawPosition(Vector3 position, Color color, float scale = 0.5f)
        {
            Gizmos.color = color;
            Gizmos.DrawCube (position, V3.one * GetScale (position) * scale);
        }

    }
}

