// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 12/06/2021 12:37:32 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Hawksbill
{
    public class RangeList<T>
    {
        protected readonly float start, step, range;
        public readonly T[] items;
        Func<float, T> get;
        Func<T, T, float, T> lerp;

        public RangeList(float start, float end, float step, Func<float, T> get, Func<T, T, float, T> lerp)
        {
            if (start >= end) throw new Exception ("RangeList: End cannot be greater or equal to start!");
            if (step == 0) throw new Exception ("RangeList: Step cannot be 0!");
            this.start = start;
            this.range = end - start;
            float count = Mathf.Ceil (range / step);
            this.step = range / count;
            this.get = get;
            this.lerp = lerp;
            IEnumerable<float> getRange()
            {
                for (float i = 0, v = start; i <= count; i++, v += step) yield return v;
                yield return start + range;
            }
            this.items = getRange ().Select (f => get (f)).ToArray ();
        }

        public T this[float f]
        {
            get
            {
                f = (f - start) / step;
                if (f <= 0) return items[0];
                else if (f >= items.Length - 1) return items.Last ();
                return lerp (items[(int) f], items[(int) f + 1], f % 1);
            }
        }
    }
}