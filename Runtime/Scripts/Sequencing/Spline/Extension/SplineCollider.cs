// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using Hawksbill.Geometry;
using V3 = UnityEngine.Vector3;
using System;
using System.Linq;

namespace Hawksbill.Sequencing
{
    [RequireComponent (typeof (SplinePlayable))]
    public abstract class SplineCollider : SplineExtension //, ISplineExTransformable
    {
        public abstract void collide(TransformBase transform);
    }
}