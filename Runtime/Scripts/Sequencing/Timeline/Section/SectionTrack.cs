// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 03/01/2021 20:32:42 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEngine.Timeline;
using Hawksbill.Sequencing;
using UnityEngine.Playables;
using System.Linq;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    [TrackBindingType (typeof (SectionController))]
    [TrackColor (1, 0, 1)]
    [TrackClipType (typeof (SectionClip))]
    public class SectionTrack : TrackAsset
    {
        // public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        // {
        //     foreach (var c in GetClips ())
        //         c.displayName = (c.asset as SectionClip).name;

        //     var mixer = ScriptPlayable<SectionTrackMixer>.Create (graph);
        //     mixer.SetInputCount (inputCount);
        //     return mixer;
        // }

    }
}