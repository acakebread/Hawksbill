// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 12/09/2021 14:18:00 by seantcooper
using UnityEngine;

namespace Hawksbill
{
    public class WaitForNextFrame : CustomYieldInstruction
    {
        int targetFrame;
        public override bool keepWaiting => targetFrame == Time.frameCount;
        public WaitForNextFrame() => targetFrame = Time.frameCount + 1;
    }
}