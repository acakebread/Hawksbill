// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 01/07/2021 17:31:37 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using V2 = UnityEngine.Vector2;
using V2I = UnityEngine.Vector2Int;
using V3 = UnityEngine.Vector3;
using V4 = UnityEngine.Vector4;
using System;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class CubeSphere : MonoBehaviour
    {
        const int MaxPoints = 128;
        [Range (2, MaxPoints), Delayed] public int points = 2;

        public MeshFilter meshFilter => GetComponent<MeshFilter> ();

        void OnValidate()
        {
            meshFilter.sharedMesh = new Create (0.5f, points).getMesh ();
        }

        class Create
        {
            float radius, points;
            List<Vector3> vts = new List<Vector3> ();
            List<Vector3> nrm = new List<Vector3> ();
            List<Vector2> uvs = new List<Vector2> ();
            List<List<int>> idx = new List<List<int>> ();
            Dictionary<uint, int> dvts = new Dictionary<uint, int> ();

            static V3[] CubeVertex = new V3[]
            {
                new V3 (-1, -1, -1), new V3 (1, -1, -1), new V3 (1, -1, 1), new V3 (-1, -1, 1),
                new V3 (-1, 1, -1), new V3 (1, 1, -1), new V3 (1, 1, 1), new V3 (-1, 1, 1),
            };

            static V2[] CubeUV = new V2[] { new V2 (0, 0), new V2 (1, 0), new V2 (1, 1), new V2 (0, 1) };
            //static V2[] CubeUV = new V2[] { new V2 (1, 0), new V2 (0, 0), new V2 (0, 1), new V2 (1, 1) };
            //static V2[] CubeUV = new V2[] { new V2 (1, 0), new V2 (0, 0), new V2 (0, 1), new V2 (1, 1) };
            //   static V2[] CubeUV = new V2[] { new V2 (0, 0), new V2 (0, 1), new V2 (1, 1), new V2 (1, 0) };

            static int[][] CubeSide = new int[][]
            {
                new int[] {5,4,7,6}, //top
                new int[] {0,1,2,3}, //bottom
                new int[] {6,7,3,2}, //front
                new int[] {4,5,1,0}, //back
                new int[] {5,6,2,1}, //left
                new int[] {7,4,0,3}, //right
            };

            public Create(float radius, int points)
            {
                this.points = points;
                this.radius = radius;

                if (points < 2 || points > MaxPoints) return;

                foreach (int[] side in CubeSide)
                    addSide (new V3[] { CubeVertex[side[0]], CubeVertex[side[1]], CubeVertex[side[2]], CubeVertex[side[3]] }, CubeUV.Reverse ().ToArray ());
            }

            void addSide(V3[] ps, V2[] uv)
            {
                this.idx.Add (new List<int> ());
                var idx = this.idx.Last ();

                print (String.Join (",", ps));
                int c = (int) points - 1;

                int w = c + 1;
                for (int y = 0, o = vts.Count; y < c; y++, o += w)
                    for (int x = 0; x < c; x++)
                    {
                        int i0 = o + x, i1 = i0 + 1, i2 = i1 + w, i3 = i0 + w;
                        if (((x + y) & 1) == 1)
                        {
                            idx.Add (i0); idx.Add (i1); idx.Add (i2);
                            idx.Add (i0); idx.Add (i2); idx.Add (i3);
                        }
                        else
                        {
                            idx.Add (i0); idx.Add (i1); idx.Add (i3);
                            idx.Add (i1); idx.Add (i2); idx.Add (i3);
                        }
                    }

                for (int y = 0; y <= c; y++)
                {
                    float fy = (float) y / c;
                    V3 v1 = V3.Lerp (ps[0], ps[3], fy), v2 = V3.Lerp (ps[1], ps[2], fy);
                    V3 uv1 = V2.Lerp (uv[0], uv[3], fy), uv2 = V2.Lerp (uv[1], uv[2], fy);
                    for (int x = 0; x <= c; x++)
                    {
                        float fx = (float) x / c;
                        V3 normal = V3.Lerp (v1, v2, fx).normalized;
                        uvs.Add (V2.Lerp (uv1, uv2, fx));
                        nrm.Add (normal);
                        vts.Add (normal * radius);
                    }
                }
            }

            public Mesh getMesh()
            {
                Mesh mesh = new Mesh () { name = "<CubeSphere>", subMeshCount = this.idx.Count };
                mesh.SetVertices (vts);
                mesh.SetNormals (nrm);
                mesh.SetUVs (0, uvs);
                foreach (var idx in this.idx)
                    this.idx.ForAll ((indices, i) => mesh.SetTriangles (indices, i));
                mesh.bounds = new Bounds (V3.zero, V3.one * radius * 2);
                return mesh;
            }
        }
    }
}