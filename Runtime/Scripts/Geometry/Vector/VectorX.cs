// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 19/07/2021 17:00:31 by seantcooper
using UnityEngine;
using Hawksbill;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using V2I = UnityEngine.Vector2Int;
using V3I = UnityEngine.Vector3Int;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class VectorX
    {
        public static V2 xz(this V3 v) => new V2 (v.x, v.z);
        public static V2I xz(this V3I v) => new V2I (v.x, v.z);
        public static V3 xz(this V2 v) => new V3 (v.x, 0, v.y);
        public static V3I xz(this V2I v) => new V3I (v.x, 0, v.y);

        public static float distanceSqr(this V3 v1, V3 v2) { float x = v2.x - v1.x, y = v2.y - v1.y, z = v2.z - v1.z; return x * x + y * y + z * z; }
        public static float distance(this V3 v1, V3 v2) => Mathf.Sqrt (v1.distanceSqr (v2));

    }
}