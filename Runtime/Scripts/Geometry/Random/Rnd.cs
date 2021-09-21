// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 27/03/2021 15:44:39 by seantcooper
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using V3 = UnityEngine.Vector3;

namespace Hawksbill.Geometry
{
    public class Rnd
    {
        Unity.Mathematics.Random random;
        public Rnd(int seed) : this ((uint) seed) { }
        public Rnd(uint seed = 10001) => random = new Random (seed);

        public static Rnd Random() => new Rnd ((uint) UnityEngine.Random.Range (1, int.MaxValue));

        public uint currentSeed => random.state;
        public uint nextSeed() { random.NextUInt (); return random.state; }

        public float[] values(int count) => Enumerable.Range (0, count).Select (i => value).ToArray ();
        public float value => random.NextFloat ();
        public float3 value3 => random.NextFloat3 ();
        public float2 value2 => random.NextFloat2 ();

        // range
        public int range(int max) => random.NextInt (0, max);
        public int range(int min, int max) => random.NextInt (min, max);
        public float range(float max) => random.NextFloat (0, max);
        public float range(float min, float max) => random.NextFloat (min, max);
        // public T range<T>(T[] array)
        // {
        //     if (array.Length == 0) throw new System.Exception ("Array is empty!");
        //     return array[range (array.Length)];
        // }
        public T range<T>(IList<T> list)
        {
            if (list.Count == 0) throw new System.Exception ("List is empty!");
            return list.Count == 1 ? list[0] : list[range (list.Count)];
        }
        public int weightedRange(int[] weights)
        {
            int total = weights.Sum ();
            int rnd = range (total);
            for (int i = 0; i < weights.Length; rnd -= weights[i], i++)
                if (rnd < weights[i]) return i;
            throw new System.Exception ("Execution should not reach this line!");
        }

        // order
        public IEnumerable<T> order<T>(IEnumerable<T> list) => list.OrderBy (a => value);

        // Volume
        public V3 onUnitSphere => new V3 (value * 2 - 1, value * 2 - 1, value * 2 - 1).normalized;
        public V3 onUnitCircle => new V3 (value * 2 - 1, 0, value * 2 - 1).normalized;
        public V3 inUnitSphere => new V3 (value * 2 - 1, value * 2 - 1, value * 2 - 1).normalized;
        public V3 inUnitCircle => new V3 (value * 2 - 1, 0, value * 2 - 1).normalized;

        public static implicit operator bool(Rnd empty) => empty != null;
    }
}