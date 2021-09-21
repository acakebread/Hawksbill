// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 01/07/2021 17:31:37 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using V4 = UnityEngine.Vector4;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class IsoSphere : MonoBehaviour
    {
        const int MaxPoints = 128;
        [Range (0.1f, 500), Delayed] public float radius = 1;
        [Range (2, MaxPoints), Delayed] public int points = 2;

        public MeshFilter meshFilter => GetComponent<MeshFilter> ();

        void OnValidate()
        {
            meshFilter.sharedMesh = new Create (radius, points).getMesh ();
        }

        class Create
        {
            float radius, points;
            List<Vector3> vts = new List<Vector3> ();
            List<Vector3> nrm = new List<Vector3> ();
            List<Vector2> uvs = new List<Vector2> ();
            List<int> idx = new List<int> ();
            Dictionary<uint, int> dvts = new Dictionary<uint, int> ();

            public Create(float radius, int points)
            {
                this.points = points;
                this.radius = radius;

                if (points < 2 || points > MaxPoints) return;

                int c = points - 1;

                // Top
                var pt0 = EnumerableUtility.Range (new V4 (0, 1, 0, 0), new V4 (-1, 0, 0, 0), c).ToArray ();
                var pt1 = EnumerableUtility.Range (new V4 (0, 1, 0, 0.25f), new V4 (0, 0, -1, 0.25f), c).ToArray ();
                var pt2 = EnumerableUtility.Range (new V4 (0, 1, 0, 0.5f), new V4 (1, 0, 0, 0.5f), c).ToArray ();
                var pt3 = EnumerableUtility.Range (new V4 (0, 1, 0, 0.75f), new V4 (0, 0, 1, 0.75f), c).ToArray ();
                var pt4 = EnumerableUtility.Range (new V4 (0, 1, 0, 1), new V4 (-1, 0, 0, 1), c).ToArray ();

                addSide (pt0, pt1);
                addSide (pt1, pt2);
                addSide (pt2, pt3);
                addSide (pt3, pt4);

                // Bottom
                var pt5 = pt0.Select (v => new V4 (v.x, -v.y, v.z, v.w)).ToArray ();
                var pt6 = pt1.Select (v => new V4 (v.x, -v.y, v.z, v.w)).ToArray ();
                var pt7 = pt2.Select (v => new V4 (v.x, -v.y, v.z, v.w)).ToArray ();
                var pt8 = pt3.Select (v => new V4 (v.x, -v.y, v.z, v.w)).ToArray ();
                var pt9 = pt4.Select (v => new V4 (v.x, -v.y, v.z, v.w)).ToArray ();

                addSide (pt6, pt5);
                addSide (pt7, pt6);
                addSide (pt8, pt7);
                addSide (pt9, pt8);
            }

            void addSide(V4[] pt1, V4[] pt2, bool wrap1 = false, bool wrap2 = false)
            {
                V4[] line = new V4[] { pt1[0] }, next;
                for (int i = 1, ni = pt1.Length, j, nj; i < ni; i++)
                {
                    next = EnumerableUtility.Range (pt1[i], pt2[i], i).ToArray ();
                    for (j = 0, nj = line.Length - 1; j < nj; j++)
                    {
                        addVertices (line[j], next[j + 1], next[j]);
                        addVertices (line[j], line[j + 1], next[j + 1]);
                    }
                    addVertices (line[j], next[j + 1], next[j]);
                    line = next;
                }
            }

            void addVertices(params V4[] pts) => pts.ForAll (v => idx.Add (addVertex (v)));

            int addVertex(V4 v)
            {
                var key = Unity.Mathematics.math.hash (v);
                if (!dvts.ContainsKey (key))
                {
                    dvts.Add (key, vts.Count);
                    V3 normal = ((V3) v).normalized;
                    nrm.Add (normal);
                    vts.Add (normal * radius);
                    uvs.Add (new V2 (v.w, Mathf.Asin (normal.y) / Mathf.PI + 0.5f));
                }
                return dvts[key];
            }

            public Mesh getMesh()
            {
                Mesh mesh = new Mesh () { name = "<IsoSphere>" }; //, indexFormat = IndexFormat.UInt32 
                mesh.SetVertices (vts);
                mesh.SetNormals (nrm);
                mesh.SetUVs (0, uvs);
                mesh.SetTriangles (idx, 0);
                mesh.bounds = new Bounds (V3.zero, V3.one * radius * 2);
                return mesh;
            }
        }
    }
}