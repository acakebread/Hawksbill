// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 03/01/2021 20:32:42 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEngine.Timeline;
using Hawksbill.Sequencing;
using UnityEngine.Playables;
using System.Linq;

namespace Hawksbill.Sequencing
{
    ///<summary>Put text here to describe the Class</summary>
    [TrackBindingType (typeof (SplinePlayable))]
    [TrackColor (1, 0.5f, 0)]
    [TrackClipType (typeof (SplineClip))]
    public class SplineTrack : TrackAsset { }
}