// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 03/05/2021 10:52:49 by seantcooper
using UnityEngine;
using UnityEngine.VFX;

namespace Hawksbill.VFX
{
    public static class VisualEffectX
    {
        public static void SetTransform(this VisualEffect vfx, string key, Transform transform)
        {
            vfx.SetVector3 (key + "_position", transform.position);
            vfx.SetVector3 (key + "_angles", transform.eulerAngles);
            vfx.SetVector3 (key + "_scale", transform.lossyScale);
        }
    }
}