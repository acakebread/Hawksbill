// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 06/08/2021 09:09:16 by seantcooper
using UnityEngine;
using Hawksbill;
using V3 = UnityEngine.Vector3;
using System.Collections.Generic;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class MeshFaceSeparation
    {

        public static bool IsCompactable(GameObject gameObject)
        {
            return gameObject && gameObject.GetComponent<MeshFilter> ();
        }

        public static GameObject Separate(GameObject gameObject, bool writeAsAsset, bool inPlace = false)
        {
            var filter = gameObject.GetComponent<MeshFilter> ();
            var mesh = filter.mesh;
            var vertexFaces = new Dictionary<V3, List<Face>> ();

            var verts = mesh.vertices;
            var tris = mesh.triangles;

            for (int i = 0; i < tris.Length; i += 3)
            {
                V3 p1 = verts[tris[i + 0]], p2 = verts[tris[i + 1]], p3 = verts[tris[i + 2]];
                V3 normal = getNormal (p1, p2, p3);
                Face face = new Face { vertices = new[] { p1, p2, p3 }, indices = new[] { tris[i + 0], tris[i + 1], tris[i + 2] } };

                vertexFaces[p1].Add (face);
                vertexFaces[p2].Add (face);
                vertexFaces[p3].Add (face);
            }

            return gameObject;
        }

        struct Face
        {
            public V3 normal;
            public V3[] vertices;
            public int[] indices;
        }

        static V3 getNormal(V3 p1, V3 p2, V3 p3) => (Vector3.Cross (p2 - p1, p3 - p1)).normalized;
    }
}