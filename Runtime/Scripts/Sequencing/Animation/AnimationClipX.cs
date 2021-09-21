// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/05/2021 14:27:34 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Linq;

namespace Hawksbill.Sequencing
{
    public static class AnimationClipX
    {
        ///<summary>Bake animation frames into an array of meshes</summary>
        public static Mesh[] getAnimationFrames(this AnimationClip clip, GameObject fbx, float speed = 1f) =>
            clip.getAnimationFrames (fbx, false, speed);

        public static Mesh[] getAnimationFrames(this AnimationClip clip, GameObject fbx, bool resetTransform, float speed = 1f)
        {
            GameObject instance = GameObject.Instantiate (fbx);

            SkinnedMeshRenderer renderer = instance.GetComponentInChildren<SkinnedMeshRenderer> ();
            if (resetTransform) renderer.transform.reset ();

            MeshFilter[] filters = instance.GetComponentsInChildren<MeshFilter> ();

            Mesh[] meshes = new Mesh[Mathf.FloorToInt (clip.length / speed * clip.frameRate)];
            Matrix4x4 matrix = instance.transform.localToWorldMatrix;

            for (int i = 0; i < meshes.Length; i++)
            {
                var frame = GetFrame (instance, renderer, clip, (float) i * clip.length / meshes.Length);
                meshes[i] = filters.Length == 0 ? frame : CombineMeshes (frame, matrix, filters);
            }

            GameObject.DestroyImmediate (instance);
            return meshes;
        }

        static Mesh GetFrame(GameObject instance, SkinnedMeshRenderer renderer, AnimationClip clip, float time)
        {
            Mesh mesh = new Mesh () { name = clip.name + time };
            clip.SampleAnimation (instance, time);
            renderer.BakeMesh (mesh, false);
            return mesh;
        }

        static Mesh CombineMeshes(Mesh mesh, Matrix4x4 matrix, MeshFilter[] filters)
        {
            var combine = new CombineInstance[] { new CombineInstance { mesh = mesh, transform = matrix } }.
                Concat (filters.Select (f => new CombineInstance { mesh = f.sharedMesh, transform = f.transform.localToWorldMatrix }));

            mesh = new Mesh { name = mesh.name };
            mesh.CombineMeshes (combine.ToArray ());
            return mesh;
        }
    }

    public static class MeshArray
    {
        ///<summary>Take array of meshes and make them submeshes in a single mesh</summary>
        public static Mesh packAsSubMeshes(this Mesh[] meshes)
        {
            if (meshes.Length > 32) Debug.LogError ("Maximum of 32 submeshes per mesh!");
            var mesh = new Mesh ()
            {
                subMeshCount = meshes.Length,
                vertices = meshes.SelectMany (f => f.vertices).ToArray (),
                uv = meshes.SelectMany (f => f.uv).ToArray (),
            };
            for (int index = 0, total = 0; index < meshes.Length; index++)
            {
                mesh.SetTriangles (meshes[index].triangles.Select (i => i + total).ToArray (), index);
                total += meshes[index].vertices.Count ();
            }
            mesh.RecalculateNormals ();
            mesh.RecalculateBounds ();
            return mesh;
        }
    }

}