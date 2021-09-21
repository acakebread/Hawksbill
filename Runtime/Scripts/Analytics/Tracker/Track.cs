// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 30/07/2021 08:48:36 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections.Generic;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class Track
    {
        // Counter
        static Dictionary<string, FrameCounter> FrameCounters = new Dictionary<string, FrameCounter> ();
        public static Counter Count(string name)
        {
            FrameCounter counter = FrameCounters.ContainsKey (name) ? FrameCounters[name] : FrameCounters[name] = new FrameCounter (name);
            counter.increment ();
            return counter;
        }

        static Dictionary<string, IDCounter> IDCounters = new Dictionary<string, IDCounter> ();
        public static IDCounter Count(string name, int id)
        {
            IDCounter counter = IDCounters.ContainsKey (name) ? IDCounters[name] : IDCounters[name] = new IDCounter (name);
            var current = counter.current;
            counter.increment (id);
            if (counter.current > current)
                Debug.Log (current);
            return counter;
        }

        public class Counter
        {
            public string name;
            public int current;
            public Counter(string name) => this.name = name;
        }

        public class FrameCounter : Counter
        {
            public int last;
            int frame;
            public FrameCounter(string name) : base (name) { }
            public void increment()
            {
                if (Time.frameCount != frame)
                {
                    frame = Time.frameCount;
                    last = current;
                    current = 0;
                }
                current++;
            }
        }

        public class IDCounter : Counter
        {
            HashSet<int> ids = new HashSet<int> ();
            public IDCounter(string name) : base (name) { }
            public void increment(int id)
            {
                if (!ids.Contains (id))
                {
                    ids.Add (id);
                    current = ids.Count;
                }
            }
        }
    }
}