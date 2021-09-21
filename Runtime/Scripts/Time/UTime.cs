// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 25/02/2021 08:54:47 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    public class UTime
    {
        float startTime, leadTime;
        public UTime()
        {
            this.startTime = Time.time;
        }
        public UTime(float leadTime) : base ()
        {
            this.leadTime = leadTime;
        }
        public float time => (Time.time - startTime) - leadTime;
        //public float unit
        public float unit(float duration) => time < 0 ? 0 : time / duration;
        public float pingpong(float duration)
        {
            float u = unit (duration * 2);
            return Mathf.Floor (u) + ((u % 1) * 2 - 1) * duration;
        }
    }
}