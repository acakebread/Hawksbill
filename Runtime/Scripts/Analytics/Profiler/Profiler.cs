// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:02:36 by seancooper
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Hawksbill.Analytics
{
    public static class Profiler
    {
        public static Dictionary<string, Profile> profiles = new Dictionary<string, Profile> ();

        static Stopwatch _stopwatch;
        static Stopwatch stopWatch => _stopwatch == null ? _stopwatch = Stopwatch.StartNew () : _stopwatch;

        public static long ticks => stopWatch.ElapsedTicks;

        public static float TickToMilliseconds(long ticks) => (float) ticks * 1000 / Stopwatch.Frequency;
        public static float TickToSeconds(long ticks) => (float) ticks / Stopwatch.Frequency;

        // Profiling
        public class Profile
        {
            Sample currentSample, slowestSample;
            public string name;
            public string note;
            public string description => name + (note == null ? "" : " " + note);
            public void start() => currentSample.begin ();
            public void stop()
            {
                currentSample.finalise ();
                if (currentSample.ticks > slowestSample.ticks || slowestSample.expired) slowestSample = currentSample;
            }
            public long slowest => slowestSample.ticks;
            public long ticks => currentSample.ticks;
            public override string ToString() => "[" + description + " " + TickToMilliseconds (ticks).ToString ("N3") + "ms" + "]";

            struct Sample
            {
                public void begin() => this.stamp = this.ticks = Profiler.ticks;
                public void finalise() { this.ticks = (this.stamp = Profiler.ticks) - this.ticks; }
                public bool expired => TickToSeconds (Profiler.ticks - this.stamp) < 5;
                internal long ticks, stamp;
            }
        }

        public static void Start(string name)
        {
            if (!profiles.ContainsKey (name)) profiles.Add (name, new Profile { name = name });
            profiles[name].start ();
        }

        public static Profile Stop(string name, string note = null)
        {
            if (profiles.ContainsKey (name))
            {
                profiles[name].stop ();
                profiles[name].note = note;
                return profiles[name];
            }
            return new Profile { name = "Cannot find " + name };
        }
    }
}
