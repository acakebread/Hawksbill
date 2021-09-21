using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using Hawksbill.Analytics;

namespace Hawksbill.Voxel
{
    public static class MeshUtility
    {
        public struct Neighbors
        {
            public static Neighbors zero = new Neighbors ();
            public Array3D XN, YN, ZN, XP, YP, ZP;
        }

        public static Mesh CreateMesh(Array3D data) => CreateMesh (data, Neighbors.zero);
        public static Mesh CreateMesh(Array3D data, Neighbors neighbors)
        {
            Geometry geometry = new Geometry (data);
            geometry.create ();
            return geometry.getMesh ();
        }

        public static Mesh CreateMesh(Array3D data, float expand)
        {
            Geometry geometry = new Geometry (data);
            geometry.create ();
            geometry.expand (expand);
            return geometry.getMesh ();
        }

        public static GameObject CreateObject(Array3D data, string name, Material material, Transform parent, float expand = 0)
        {
            GameObject o = new GameObject (name);
            o.transform.parent = parent;
            o.transform.localPosition = Vector3.zero;
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = V3.one;
            o.AddComponent<MeshRenderer> ().sharedMaterial = material;
            o.AddComponent<MeshFilter> ().sharedMesh = CreateMesh (data, expand);
            return o;
        }

        class Geometry
        {
            Vertex[] VTX;

            const float uvscalar = 1f / 16f, uvstep = uvscalar / 2;
            static readonly V2[] UV = Enumerable.Range (0, 256).Select (i => new V2 (((i & 15)) * uvscalar + uvstep, (15 - (i >> 4)) * uvscalar + uvstep)).ToArray ();

            int VTI = 0;
            Array3D data;

            public static readonly V3 NormalNX = new V3 (-1, 0, 0);
            public static readonly V3 NormalPX = new V3 (1, 0, 0);
            public static readonly V3 NormalNY = new V3 (0, -1, 0);
            public static readonly V3 NormalPY = new V3 (0, 1, 0);
            public static readonly V3 NormalNZ = new V3 (0, 0, -1);
            public static readonly V3 NormalPZ = new V3 (0, 0, 1);

            public Geometry(Array3D data)
            {
                this.data = data;
                VTX = new Vertex[data.length * 4 * 6];
            }

            internal void create() => create (Neighbors.zero);
            internal void create(Neighbors neighbors)
            {
                int minx = 0, miny = 0, minz = 0, maxx = data.size.x - 1, maxy = data.size.y - 1, maxz = data.size.z - 1;
                int xoff = 1, yoff = data.size.x * data.size.z, zoff = data.size.x;
                int xwoff = data.size.z - xoff, ywoff = data.length - yoff, zwoff = data.size.x * data.size.z - zoff;
                int b = 0, color;

                byte[] xn = neighbors.XN == null ? null : neighbors.XN.bytes, xp = neighbors.XP == null ? null : neighbors.XP.bytes;
                byte[] yn = neighbors.YN == null ? null : neighbors.YN.bytes, yp = neighbors.YP == null ? null : neighbors.YP.bytes;
                byte[] zn = neighbors.ZN == null ? null : neighbors.ZN.bytes, zp = neighbors.ZP == null ? null : neighbors.ZP.bytes;
                byte[] dt = data.bytes;

                for (V3 v = V3.zero; v.y < data.size.y; v.y++)
                {
                    for (v.z = 0; v.z < data.size.z; v.z++)
                    {
                        for (v.x = 0; v.x < data.size.x; v.x++, b++)
                        {
                            if ((color = dt[b]) != 0)
                            {
                                if (v.x == minx ? (xn == null ? true : xn[b + xwoff] == 0) : dt[b - xoff] == 0) addFaceXN (v, color);
                                if (v.x == maxx ? (xp == null ? true : xp[b - xwoff] == 0) : dt[b + xoff] == 0) addFaceXP (v, color);
                                if (v.z == minz ? (zn == null ? true : zn[b + zwoff] == 0) : dt[b - zoff] == 0) addFaceZN (v, color);
                                if (v.z == maxz ? (zp == null ? true : zp[b - zwoff] == 0) : dt[b + zoff] == 0) addFaceZP (v, color);
                                if (v.y == miny ? (yn == null ? true : yn[b + ywoff] == 0) : dt[b - yoff] == 0) addFaceYN (v, color);
                                if (v.y == maxy ? (yp == null ? true : yp[b - ywoff] == 0) : dt[b + yoff] == 0) addFaceYP (v, color);
                            }
                        }
                    }
                }
            }

            internal void expand(float d)
            {
                if (d == 0) return;
                float d2 = d * 2;
                for (int i = 0; i < VTI; i += 4)
                {
                    V3 n = VTX[i].n * d;
                    V3 c = (VTX[i + 0].p + VTX[i + 1].p + VTX[i + 2].p + VTX[i + 3].p) * 0.25f;
                    VTX[i + 0].p += (VTX[i + 0].p - c) * d2 + n;
                    VTX[i + 1].p += (VTX[i + 1].p - c) * d2 + n;
                    VTX[i + 2].p += (VTX[i + 2].p - c) * d2 + n;
                    VTX[i + 3].p += (VTX[i + 3].p - c) * d2 + n;
                }
            }

            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            void addFaceXN(V3 v, int color)
            {
                Vertex vx = new Vertex (v, NormalNX, UV[color]);
                VTX[VTI++] = vx; vx.p.z++; VTX[VTI++] = vx; vx.p.y++; VTX[VTI++] = vx; vx.p.z--; VTX[VTI++] = vx;
            }
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            void addFaceXP(V3 v, int color)
            {
                Vertex vx = new Vertex (v, NormalPX, UV[color]);
                vx.p.x++; VTX[VTI++] = vx; vx.p.y++; VTX[VTI++] = vx; vx.p.z++; VTX[VTI++] = vx; vx.p.y--; VTX[VTI++] = vx;
            }
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            void addFaceYN(V3 v, int color)
            {
                Vertex vx = new Vertex (v, NormalNY, UV[color]);
                VTX[VTI++] = vx; vx.p.x++; VTX[VTI++] = vx; vx.p.z++; VTX[VTI++] = vx; vx.p.x--; VTX[VTI++] = vx;
            }
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            void addFaceYP(V3 v, int color)
            {
                Vertex vx = new Vertex (v, NormalPY, UV[color]);
                vx.p.y++; VTX[VTI++] = vx; vx.p.z++; VTX[VTI++] = vx; vx.p.x++; VTX[VTI++] = vx; vx.p.z--; VTX[VTI++] = vx;
            }
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            void addFaceZN(V3 v, int color)
            {
                Vertex vx = new Vertex (v, NormalNZ, UV[color]);
                VTX[VTI++] = vx; vx.p.y++; VTX[VTI++] = vx; vx.p.x++; VTX[VTI++] = vx; vx.p.y--; VTX[VTI++] = vx;
            }
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
            void addFaceZP(V3 v, int color)
            {
                Vertex vx = new Vertex (v, NormalPZ, UV[color]);
                vx.p.z++; VTX[VTI++] = vx; vx.p.x++; VTX[VTI++] = vx; vx.p.y++; VTX[VTI++] = vx; vx.p.x--; VTX[VTI++] = vx;
            }

            internal Mesh getMesh()
            {
                var mesh = new Mesh ();
                mesh.indexFormat = IndexFormat.UInt32;

                var idx = new int[VTI / 4 * 6];
                for (int i = 0, u = 0; i < idx.Length; u += 4)
                {
                    idx[i++] = u + 0; idx[i++] = u + 1; idx[i++] = u + 2;
                    idx[i++] = u + 0; idx[i++] = u + 2; idx[i++] = u + 3;
                }

                mesh.SetVertexBufferParams (VTI, Vertex.layout);
                mesh.SetVertexBufferData (VTX, 0, 0, VTI);
                mesh.SetIndexBufferParams (idx.Length, IndexFormat.UInt32);
                mesh.SetIndexBufferData (idx, 0, 0, idx.Length);
                mesh.subMeshCount = 1;
                mesh.SetSubMesh (0, new SubMeshDescriptor (0, idx.Length));

                mesh.bounds = new Bounds (data.bounds.center, data.bounds.size);
                return mesh;
            }

            [StructLayout (LayoutKind.Sequential)]
            struct Vertex
            {
                public V3 p, n;
                public V2 uv;

                public Vertex(float3 position, float3 normal, float2 uv)
                {
                    this.p = position;
                    this.n = normal;
                    this.uv = uv;
                }
                static public VertexAttributeDescriptor[] layout => new[]
                {
                    new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                    new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
                    new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2),
                };
            }
        }
    }
}
