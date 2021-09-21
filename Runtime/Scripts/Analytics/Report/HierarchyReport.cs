// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 11/01/2021 08:07:09 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class HierarchyReport : MonoBehaviour
    {
        public string[] types;
        public float time = 0;
        IEnumerator Start()
        {
            while (enabled)
            {
                yield return null;
                time = Time.time;
            }
        }
    }
}