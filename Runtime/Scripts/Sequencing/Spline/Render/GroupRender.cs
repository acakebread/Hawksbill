// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/05/2021 15:41:10 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Linq;
using Hawksbill.Sequencing;

namespace Hawksbill.Render
{
    [ExecuteInEditMode]
    public class GroupRender : MonoBehaviour
    {
        public SplineGroup splineGroup;
        [Range (0, 1000000000)] public uint seed = 1000;
        [Line]
        public FrameRenderer renderObject;
        void OnValidate() { if (splineGroup == null) splineGroup = GetComponent<SplineGroup> (); }
        void LateUpdate() => renderObject?.draw (splineGroup.time, splineGroup.transforms);
    }
}
