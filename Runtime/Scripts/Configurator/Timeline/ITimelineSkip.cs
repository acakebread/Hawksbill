// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 07/09/2021 15:01:28 by seantcooper
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    public enum SkipType
    {
        None = 0,
        Full = 1,
    }

    public interface ITimelineSkip
    {
        SkipType skipType { get; }
    }
}