using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Hawksbill.Analytics;

using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using V3I = UnityEngine.Vector3Int;

using static Hawksbill.Voxel.VoxelMesh;

namespace Hawksbill.Voxel
{
    public class VoxelEdit : MonoBehaviour
    {
        public VoxelContainer voxel => GetComponent<VoxelContainer> ();
        public VoxelMesh[] meshes => voxel.meshes;

        public int rndI(int range) => UnityEngine.Random.Range (0, range);
        public int rndNP(int range) => UnityEngine.Random.Range (0, range * 2) - range;
        public float rnd(float range) => UnityEngine.Random.value * range;

        void Start()
        {
            for (int i = 100; i > 0; --i)
            {
                var color = (byte) rndI (256);
                var brush = new Array3D (8, 8, 8, color);
                var position = new V3I (rndNP (12), rndI (24), rndNP (12));
                paste (brush, position).ForAll (m => m.invalidate ());
            }
        }

        public VoxelMesh[] paste(Array3D brush, V3I index, Set mode = Set.Or)
        {
            index -= brush.size / 2;
            var meshes = voxel.getMeshes (brush.bounds.offset (index), true).ToArray ();
            meshes.ForAll (m => m.data.paste (brush, index - m.index, mode));
            return meshes;
        }

        public Position getEditPosition(Ray ray)
        {
            Position result = null;
            var plane = new Plane (transform.up, transform.position);
            if (plane.Raycast (ray, out float enter) && plane.GetSide (ray.origin))
            {
                result = new Position (transform.InverseTransformPoint (ray.GetPoint (enter)),
                    transform.InverseTransformDirection (plane.normal), V3.Distance (ray.GetPoint (enter), ray.origin));
            }
            if (Physics.Raycast (ray, out RaycastHit hit) && (result == null || hit.distance < result.distance))
            {
                var mesh = hit.collider.GetComponent<VoxelMesh> ();
                result = mesh ? new Position (transform.InverseTransformPoint (hit.point),
                    transform.InverseTransformDirection (hit.normal), hit.distance, mesh) : null;
            }
            return result;
        }

        public enum Set { Copy, Or, Clear }

        public class Position
        {
            public float distance;
            public V3 position, normal;
            public VoxelMesh mesh;

            public Position(V3 position, V3 normal, float distance, VoxelMesh mesh = null)
            {
                this.position = position;
                this.normal = normal;
                this.distance = distance;
                this.mesh = mesh;
            }

            public Axis axis => math.abs (normal.x) > 0 ? Axis.X : (math.abs (normal.y) > 0 ? Axis.Y : Axis.Z);
            public V3 center => index + V3.one / 2;
            public V3I index => V3I.FloorToInt (position - normal * 0.5f);
            public V3I faceindex => V3I.FloorToInt (index + normal);

            public static implicit operator bool(Position empty) => empty != null;
            public enum Axis { X, Y, Z };
        }
    }

    static class Array3DEdit
    {
        public static void paste(this Array3D dest, Array3D src, V3I position, VoxelEdit.Set set = VoxelEdit.Set.Copy)
        {
            var positions = dest.bounds.getPositions (src.bounds.offset (position)).Select (p => new { p1 = p, p2 = p - position });
            switch (set)
            {
                case VoxelEdit.Set.Copy: positions.ForAll (p => dest[p.p1] = src[p.p2]); break;
                case VoxelEdit.Set.Or: foreach (var p in positions) if (src[p.p2] > 0) dest[p.p1] = src[p.p2]; break;
                case VoxelEdit.Set.Clear: foreach (var p in positions) if (src[p.p2] > 0) dest[p.p1] = 0; break;
            }
        }
    }
}