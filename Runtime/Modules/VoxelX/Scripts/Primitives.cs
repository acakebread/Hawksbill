using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using V3I = UnityEngine.Vector3Int;

namespace Hawksbill.Voxel
{
    public static class Primitives
    {
        public static Array3D Sphere(int sizex, int sizey, int sizez, byte color)
        {
            var array = new Array3D (sizex, sizey, sizez);
            var center = array.bounds.center;
            foreach (var p in array.positionsWithin)
                if (math.lengthsq ((float3) ((Vector3) p - center) / center) < 1)
                    array[p] = color;
            return array;
        }

        public static Array3D Cube(int sizex, int sizey, int sizez, byte color)
        {
            var array = new Array3D (sizex, sizey, sizez);
            array.clear (color);
            return array;
        }
    }
}
