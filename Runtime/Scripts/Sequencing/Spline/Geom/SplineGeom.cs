// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 19/05/2021 10:24:47 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using V3 = UnityEngine.Vector3;

namespace Hawksbill.Sequencing
{
    public class ComponentFilter
    {
        [Flags]
        public enum Mask
        {
            All = Position | Rotation,

            Position = PositionX | PositionY | PositionZ,
            PositionX = 1 << 0,
            PositionY = 1 << 1,
            PositionZ = 1 << 2,
            Rotation = RotationX | RotationY | RotationZ,
            RotationX = 1 << 3,
            RotationY = 1 << 4,
            RotationZ = 1 << 5,
        }

        public static V3 FilterPosition(V3 p1, V3 p2, Mask f) =>
            new V3 ((f & Mask.PositionX) == 0 ? p1.x : p2.x, (f & Mask.PositionY) == 0 ? p1.y : p2.y, (f & Mask.PositionZ) == 0 ? p1.z : p2.z);

        public static V3 FilterRotation(V3 p1, V3 p2, Mask f) =>
            new V3 ((f & Mask.RotationX) == 0 ? p1.x : p2.x, (f & Mask.RotationY) == 0 ? p1.y : p2.y, (f & Mask.RotationZ) == 0 ? p1.z : p2.z);
    }

}