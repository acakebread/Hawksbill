// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 03/01/2021 20:37:36 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEngine.Playables;
using Hawksbill.Sequencing;

namespace Hawksbill.Sequencing
{
    ///<summary>Put text here to describe the Class</summary>
    public class SectionBehaviour : PlayableBehaviour
    {
        public string name;
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SectionController controller = playerData as SectionController;
            // info.weight = overlap
        }
    }
}