// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 21/05/2021 13:37:30 by seantcooper
using UnityEngine;
using Hawksbill;
using V3 = UnityEngine.Vector3;
using Hawksbill.Geometry;

namespace Hawksbill.AI
{
    ///<summary>Put text here to describe the Class</summary>
    public class Group : MonoBehaviour
    {
        [Range (0, 100)] public int count = 16;
        [Header ("Noise")]
        [Range (0.0001f, 0.2f)] public float frequency = 0.01f;
        [Range (0, 50)] public float length = 2f;
        [Range (0, 100000)] public uint seed = 1000;
        public V3 scale = V3.one;

        Unity.Mathematics.Random getRandom() => new Unity.Mathematics.Random (seed);

        void Start()
        {

        }

        void Update()
        {

        }

        void OnDrawGizmos()
        {
            var rnd = getRandom ();
            var rotation = transform.rotation;
            var position = transform.position;
            var forward = transform.forward;

            Gizmos.color = Color.green;
            for (int i = 0; i < count; i++)
            {
                var offset = rnd.NextFloat (1000);
                var delta = noise (position, offset);
                var direction = noise (position + forward, offset) - delta;
                var p = position + delta;
                var r = rotation * Quaternion.LookRotation (direction);
                Gizmos.DrawCube (p, V3.one * 0.1f);
                Gizmos.DrawLine (p, p + r * V3.forward);


                //var r = new V3()
            }
        }

        public V3 noise(V3 v, float offset) => Vector3.Scale (Noise.Perlin (v * frequency + V3.one * offset), scale * length);
    }
}