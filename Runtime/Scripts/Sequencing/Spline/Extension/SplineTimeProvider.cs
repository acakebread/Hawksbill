// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 21/05/2021 13:37:30 by seantcooper
using UnityEngine;
using UnityEngine.Playables;

namespace Hawksbill.Sequencing
{
    ///<summary>Put text here to describe the Class</summary>
    [RequireComponent (typeof (SplinePlayable))]
    public class SplineTimeProvider : SplineExtension
    {
        public PlayableDirector playableDirector;
        public new float time => (float) (playableDirector?.time ?? Time.time);
    }
}