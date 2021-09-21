// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 20/07/2021 09:36:09 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Linq;
using System.Collections.Generic;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using V2I = UnityEngine.Vector2Int;
using V3I = UnityEngine.Vector3Int;
using Hawksbill.IO;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class MeshCompactor
    {
        public static GameObject Compact(GameObject container, bool writeAsAsset, bool inPlace = false) =>
            Compact (container.transform, writeAsAsset, inPlace);

        public static GameObject Compact(Transform container, bool writeAsAsset, bool inPlace = false)
        {
            Dictionary<Material, Geometry> geometry = new Dictionary<Material, Geometry> ();
            var containerMatrix = container.worldToLocalMatrix;

            foreach (var filter in container.GetComponentsInChildren<MeshFilter> ())
            {
                var renderer = filter.GetComponent<MeshRenderer> ();
                var material = renderer.sharedMaterial;
                if (!geometry.ContainsKey (material)) geometry[material] = new Geometry ();
                geometry[material].add (filter, containerMatrix * filter.transform.localToWorldMatrix);
            }

            GameObject compacted = null;
            if (inPlace)
            {
                compacted = container.gameObject;
                compacted.transform.destroyChildren ();
            }
            else
            {
                compacted = new GameObject (container.name)
                {
                    transform = { localRotation = container.localRotation, localPosition = container.localPosition, localScale = container.localScale, }
                };
            }

            compacted.layer = container.gameObject.layer;
            compacted.tag = container.gameObject.tag;

            IEnumerable<Mesh> createMeshes()
            {
                foreach (var pair in geometry)
                {
                    var child = new GameObject (pair.Key.name)
                    {
                        transform = { parent = compacted.transform, localPosition = V3.zero, localRotation = Quaternion.identity, localScale = V3.one }
                    };
                    var mesh = pair.Value.getMesh (container.name + " (" + pair.Key.name + ")");
                    child.AddComponent<MeshFilter> ().sharedMesh = mesh;
                    child.AddComponent<MeshRenderer> ().sharedMaterial = pair.Key;
                    yield return mesh;
                }
            }
            var meshes = createMeshes ().ToArray ();

#if UNITY_EDITOR
            if (writeAsAsset)
            {
                string path = ((Path) container.gameObject.scene.path).folderPath.assetPath;
                meshes.ForAll (m => UnityEditor.AssetDatabase.CreateAsset (m, path + "/" + m.name + ".mesh"));
                var prefab = UnityEditor.PrefabUtility.SaveAsPrefabAsset (compacted, path + "/" + compacted.name + ".prefab");
                compacted.destroy ();
                compacted = UnityEditor.PrefabUtility.InstantiatePrefab (prefab) as GameObject;
            }
#endif

            return compacted;
        }

        public static bool IsCompactable(GameObject gameObject)
        {
            return gameObject && gameObject.GetComponentInChildren<MeshFilter> ();
        }

        public class Geometry
        {
            public List<V3> vertices = new List<V3> ();
            public List<V3> normals = new List<V3> ();
            public List<V2> uvs = new List<V2> ();
            public List<int> indices = new List<int> ();

            public void add(MeshFilter filter, Matrix4x4 matrix)
            {

                var mesh = filter.sharedMesh;
                int c = vertices.Count;
                indices.AddRange (mesh.triangles.Select (i => i + c));
                vertices.AddRange (mesh.vertices.Select (v => matrix.MultiplyPoint (v)));
                normals.AddRange (mesh.normals.Select (v => matrix.MultiplyVector (v)));
                uvs.AddRange (mesh.uv);
            }

            public Mesh getMesh(string name)
            {
                var mesh = new Mesh
                {
                    name = name,
                    vertices = vertices.ToArray (),
                    normals = normals.ToArray (),
                    uv = uvs.ToArray (),
                    triangles = indices.ToArray (),
                };
                mesh.RecalculateBounds ();
                mesh.Optimize ();
                //Debug.Log ("Mesh created " + name + " vertices: " + vertices.Count + " triangles: " + indices.Count / 3);
                return mesh;
            }
        }
    }
}