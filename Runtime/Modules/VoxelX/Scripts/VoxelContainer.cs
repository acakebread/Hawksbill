using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using V3I = UnityEngine.Vector3Int;

namespace Hawksbill.Voxel
{
    public class VoxelContainer : MonoBehaviour
    {
        public V3I size = new V3I (1000, 500, 1000);
        public Material material;
        public bool hideInHierarchy;

        public BoundsInt bounds => new BoundsInt (-size.x / 2, 0, -size.z / 2, size.x, size.y, size.z);
        public VoxelMesh[] meshes => GetComponentsInChildren<VoxelMesh> ();

        static int Snap(float x) => Mathf.FloorToInt (x / VoxelMesh.Size) * VoxelMesh.Size;
        internal static V3I GetMeshIndex(V3I index) => new V3I (Snap (index.x), Snap (index.y), Snap (index.z));

        [MethodImpl (MethodImplOptions.AggressiveInlining)] static string MNSIGN(int u) => u < 0 ? u.ToString () : "+" + u;
        static string GetMeshName(V3I index) => MNSIGN (index.x) + MNSIGN (index.y) + MNSIGN (index.z);

        VoxelMesh createMesh(V3I index)
        {
            index = GetMeshIndex (index);
            VoxelMesh mesh = new GameObject (GetMeshName (index)).AddComponent<VoxelMesh> ();
            mesh.transform.parent = transform;
            mesh.transform.localPosition = index;
            mesh.transform.localScale = V3.one;
            mesh.transform.localRotation = Quaternion.identity;
            mesh.index = index;
            mesh.gameObject.AddComponent<MeshRenderer> ().sharedMaterial = material;
            mesh.gameObject.AddComponent<MeshFilter> ();
            mesh.gameObject.AddComponent<MeshCollider> ();
            if (hideInHierarchy) mesh.gameObject.hideFlags |= HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            return mesh;
        }

        internal VoxelMesh getMesh(V3I index, bool create = false)
        {
            var t = transform.Find (GetMeshName (GetMeshIndex (index)));
            return t == null ? (create ? createMesh (index) : null) : t.GetComponent<VoxelMesh> ();
        }

        internal IEnumerable<V3I> getMeshIndicies(BoundsInt bounds)
        {
            var b = this.bounds.intersection (bounds);
            b.min = GetMeshIndex (b.min);
            for (V3I p = b.min; p.z < b.zMax; p.z += VoxelMesh.Size)
                for (p.y = b.yMin; p.y < b.yMax; p.y += VoxelMesh.Size)
                    for (p.x = b.xMin; p.x < b.xMax; p.x += VoxelMesh.Size)
                        yield return p;
        }

        internal IEnumerable<VoxelMesh> getMeshes(BoundsInt bounds, bool create = false)
        {
            VoxelMesh mesh;
            foreach (var p in getMeshIndicies (bounds))
                if ((mesh = getMesh (p, create))) yield return mesh;
        }
    }
}
