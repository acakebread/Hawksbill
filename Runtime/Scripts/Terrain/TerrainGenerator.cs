// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 28/04/2021 15:54:23 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using Hawksbill.Geometry;
using System.Linq;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class TerrainGenerator : MonoBehaviour
    {
        [ReadOnly] public int resolution;
        [Line]
        [Button] public bool generate = false;
        [Range (1, 16)] public int octaves = 8;
        [Range (0.000001f, 0.1f)] public float scale = 0.5f;
        [Range (0, 1)] public float persistance = 0.5f;
        [Range (1, 10)] public float lacunarity = 2;
        [Range (0, 100000)] public uint seed = 1000;
        public PerlinValues perlin;

        Terrain terrain => GetComponent<Terrain> ();

        void OnValidate()
        {
            if (generate)
            {
                generate = false;
                generateTerrain ();
            }
            resolution = terrain.terrainData.heightmapResolution;
        }

        void generateTerrain()
        {
            TerrainData data = terrain.terrainData;
            resolution = terrain.terrainData.heightmapResolution;
            data.SetHeights (0, 0, generateHeights ());
            terrain.terrainData = data;
        }

        float[,] generateHeights()
        {
            float[] heights = Enumerable.Range (0, resolution * resolution).Select (i => 0.5f).ToArray ();

            Rnd rnd = new Rnd (seed);
            float amplitude = 1, frequency = scale;

            float[] noise = new float[heights.Length];
            for (float octaves = 0; octaves < this.octaves; octaves++)
            {
                Vector2 offset = ((rnd.value2 * 2 - 1) * 10000f);
                PerlinNoise (resolution, frequency, amplitude, offset, noise);
                heights = heights.Select ((h, i) => h + noise[i]).ToArray ();
                amplitude *= persistance;
                frequency *= lacunarity;
            }

            float min = heights.Min (), max = heights.Max (), r = max - min;
            var h2d = new float[resolution, resolution];
            for (int x = 0, i = 0; x < resolution; x++)
                for (int y = 0; y < resolution; y++, i++)
                    h2d[x, y] = (heights[i] - min) / r;
            zeroBorders (h2d);
            return h2d;
        }

        void zeroBorders(float[,] heights)
        {
            for (int u = 0, r = resolution - 1; u < resolution; u++)
                heights[0, u] = heights[r, u] = heights[u, 0] = heights[u, r] = 0;
        }

        static float[] PerlinNoise(int size, float frequency, float amplitude, Vector2 offset, float[] values = null)
        {
            print ("Frequency = " + frequency);
            values = values == null ? new float[size * size] : values;
            for (int x = 0, i = 0; x < size; x++)
                for (int y = 0; y < size; y++, i++)
                    // {
                    //     values[i] = (noise.pnoise (new float2 (x * frequency + offset.x, y * frequency + offset.y), new float2 (10000, 10000)) - 0.5f) * 2 * amplitude;
                    // }
                    values[i] = (Mathf.PerlinNoise (x * frequency + offset.x, y * frequency + offset.y) - 0.5f) * 2 * amplitude;
            return values;
        }

        [Serializable]
        public class PerlinValues
        {
            [Range (0, 1)] public float[] values = new float[16];
        }
    }

    // #if UNITY_EDITOR
    //     [CustomPropertyDrawer (typeof (TerrainGenerator.PerlinValues))]
    //     public class PerlinValuesPropertyDrawer : PropertyDrawer
    //     {
    //         const float Spacing = 2;
    //         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //         {
    //             var array = property.FindPropertyRelative ("values");
    //             for (int i = 0; i < array.arraySize; i++)
    //             {
    //                 EditorGUILayout.PropertyField (array.GetArrayElementAtIndex (i), new GUIContent ((i + 1) + ":"));
    //             }
    //         }

    //         // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //         // {
    //         //     var array = property.FindPropertyRelative ("values");
    //         //     return base.GetPropertyHeight (property, label) * array.arraySize;
    //         // }

    //     }
    // #endif
}