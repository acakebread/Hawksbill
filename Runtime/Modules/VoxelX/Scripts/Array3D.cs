using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;
using System.Runtime.CompilerServices;

namespace Hawksbill.Voxel
{
    [Serializable]
    public class Array3D : IEnumerable<byte>
    {
        public readonly Vector3Int size;
        public int length;

        public byte[] _bytes;
        public byte[] bytes
        {
            get => _bytes;
            set
            {
                if (_bytes == null && value.Length == length) _bytes = value;
                Array.Copy (value, _bytes, math.min (value.Length, length));
            }
        }

        public BoundsInt bounds => new BoundsInt (0, 0, 0, size.x, size.y, size.z);

        // Indexing
        public byte this[Vector3Int index] { get => bytes[getIndex (index)]; set => bytes[getIndex (index)] = value; }
        public byte this[int index] { get => bytes[index]; set => bytes[index] = value; }

        // Constructor
        public Array3D(int sizex, int sizey, int sizez) : this (sizex, sizey, sizez, new byte[sizex * sizey * sizez]) { }
        public Array3D(int sizex, int sizey, int sizez, byte color) : this (sizex, sizey, sizez) => clear (color);
        public Array3D(int sizex, int sizey, int sizez, byte[] data)
        {
            this.size = new Vector3Int (sizex, sizey, sizez);
            this.length = sizex * sizey * sizez;
            this.bytes = data;
        }

        public void clear() => Array.Clear (bytes, 0, bytes.Length);
        public void clear(byte value) => bytes = Enumerable.Repeat (value, length).ToArray ();

        //Enumerable
        public IEnumerator<byte> GetEnumerator() { foreach (byte b in bytes) yield return b; }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator ();
        public IEnumerable<int> indicesWithin => Enumerable.Range (0, bytes.Length);
        public IEnumerable<Vector3Int> positionsWithin => bounds.getPositions ();

        // Range & Indexing support
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        int getIndex(Vector3Int index) => getIndex (index.x, index.y, index.z);
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        int getIndex(int x, int y, int z) => (y * size.z + z) * size.x + x;
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        bool inRange(int u, int size) => u >= 0 && u < size;
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        bool isIndexValid(Vector3Int index) => inRange (index.x, size.x) && inRange (index.y, size.y) && inRange (index.z, size.z);
    }
}
