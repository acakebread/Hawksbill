using System;
using UnityEngine;

namespace Hawksbill.Voxel
{
    [ExecuteInEditMode]
    public class VoxelMesh : MonoBehaviour
    {
        DateTime dateTime;
        public const int Size = 32, Total = Size * Size * Size;

        [HideInInspector] public Array3D data = new Array3D (Size, Size, Size);
        public Vector3Int index;
        public Neighbours neighbours;
        public VoxelContainer voxel => transform.parent.GetComponent<VoxelContainer> ();
        public bool forceInvalidation;
        Validation validated = Validation.All;

        public bool hasMesh => GetComponent<MeshFilter> ().sharedMesh != null;
        public MeshFilter meshFilter => GetComponent<MeshFilter> ();
        public MeshRenderer meshRenderer => GetComponent<MeshRenderer> ();
        public MeshCollider meshCollider => GetComponent<MeshCollider> ();


        public void invalidate(Validation validation = Validation.All)
        {
            validated &= ~validation;
            validate ();
        }

        void validate()
        {
            if (validated == Validation.All) return;
            var mesh = MeshUtility.CreateMesh (data);
            if ((validated & Validation.Render) == 0) meshFilter.sharedMesh = mesh;
            if ((validated & Validation.Collide) == 0) meshCollider.sharedMesh = mesh;
            meshCollider.enabled = meshRenderer.enabled = mesh;
            validated = Validation.All;
        }

        public enum Validation { Render = 1 << 0, Collide = 1 << 1, All = Render | Collide }

        [Serializable]
        public class Neighbours
        {
            public VoxelMesh XN, XP, YN, YP, ZN, ZP;
            // public static Neighbours GetNeighbours(VoxelMesh mesh)
            // {
            //     V3I i = mesh.index;
            //     VoxelX voxel = mesh.voxel;
            //     return new Neighbours
            //     {
            //         XN = voxel.getMesh (i-new V3I),
            //         XP = voxel.getMesh (i.x + 1, i.y, i.z),
            //         YN = voxel.getMesh (i.x, i.y - 1, i.z),
            //         YP = voxel.getMesh (i.x, i.y + 1, i.z),
            //         ZN = voxel.getMesh (i.x, i.y, i.z - 1),
            //         ZP = voxel.getMesh (i.x, i.y, i.z + 1),
            //     };
            // }
        }
    }
}

// void OnDrawGizmos()
// {
//     Gizmos.matrix = transform.localToWorldMatrix;
//     Gizmos.color = new Color (0, 0, 1, 0.5f);
//     if (hasMesh) Gizmos.DrawWireCube (data.bounds.center, data.bounds.size);
//     else Gizmos.DrawCube (data.bounds.center, data.bounds.size);
// }
