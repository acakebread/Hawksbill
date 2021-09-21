// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 07/09/2021 13:41:37 by seantcooper
using System;
using System.Collections;
using UnityEngine;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class CoroutineX
    {
        public static IEnumerator Simple(Action action, YieldInstruction yieldInstruction = null)
        {
            while (true)
            {
                action ();
                yield return yieldInstruction;
            }
        }

    }
}