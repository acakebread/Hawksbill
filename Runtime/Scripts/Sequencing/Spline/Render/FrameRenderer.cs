// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/05/2021 15:41:10 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Linq;
using Hawksbill.Sequencing;
using Hawksbill.Geometry;
using System.Collections.Generic;
using System;

namespace Hawksbill.Render
{
    [System.Serializable, CreateAssetMenu (menuName = "Hawksbill/Spline/Render Group Object description")]
    public class FrameRenderer : ScriptableObject
    {
        public GameObject prefab;
        public AnimationClip clip;
        public Material[] materials;
        [Line]
        [Button] public bool invalidate;
        [ReadOnly] public int frameCount;
        Mesh[] _frames;

        // Mesh[] _frames;
        Mesh[] getFrames()
        {
            if (_frames == null || _frames.Length == 0) _frames = clip.getAnimationFrames (prefab, 1f);
            frameCount = _frames.Length;
            return _frames;
        }

        void OnValidate()
        {
            _frames = null;
            if (invalidate) invalidate = false;
        }

        int getFrameIndex(float time) => Mathf.RoundToInt (Mathf.Max (0, time) * clip.frameRate) % _frames.Length;

        public void draw(float time, Transform transform, Camera camera = null) => draw (time, transform.localToWorldMatrix, materials[0], camera);
        public void draw(float time, TransformBase transform, Camera camera = null) => draw (time, transform.matrix, materials[0], camera);
        public void draw(float time, Matrix4x4 matrix, Material material, Camera camera = null)
        {
            var frames = this.getFrames ();
            if (frames == null || frames.Length == 0) return;
            int index = getFrameIndex (time);
            Graphics.DrawMesh (frames[index], matrix, material, 0, camera);
        }

        public void draw(float time, IEnumerable<TransformBase> transforms, Camera camera = null)
        {
            var rnd = new Rnd ();
            transforms.ForAll (t => draw (time + rnd.value, t.matrix, materials[rnd.range (materials.Length)], camera));
        }
    }
}

// [Line]
// public AssetSearch search;

//         public class AssetSearch
//         {
//             public string path = "Assets/Prototype/Animals/Generic";
//             [Button] public bool search;
//             [Button] public bool invalidate;

//                         if (search)
//             {
//                 search = false;
//                 var materials = UnityEditor.AssetDatabase.FindAssets (name + " t:Material", new[] { path + "/Materials" });
//                 if (materials.Length > 0)
//                 {
//                     string p = UnityEditor.AssetDatabase.GUIDToAssetPath (materials.First ());
//                     this.materials = new Material[] { UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(p)
//         };
//     }
//     var prefabs = UnityEditor.AssetDatabase.FindAssets (name + " t:GameObject", new[] { path + "/Prefabs" });
//                 if (prefabs.Length > 0)
//                 {
//                     string p = UnityEditor.AssetDatabase.GUIDToAssetPath (prefabs.First ());
//     prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(p);
//                 }
// var clips = UnityEditor.AssetDatabase.FindAssets (name + "_walk t:AnimationClip", new[] { path + "/Animation" });
// if (clips.Length > 0)
// {
//     string p = UnityEditor.AssetDatabase.GUIDToAssetPath (clips.First ());
//     clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AnimationClip> (p);
// }
//             }

//         }
