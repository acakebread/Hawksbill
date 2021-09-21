// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 03/02/2021 09:23:43 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Wonder14.Audio
{
    ///<summary>Put text here to describe the Class</summary>
    public class AudioAction : MonoBehaviour
    {
        public Action action;
        [Tooltip ("Value associated with action, i.e. DestoryAfterTime - 'value' = the amount of time.")]
        public float value;

        public enum Action
        {
            DestoryOnComplete = 0,
            DestoryAfterTime = 1,
        }
    }
}