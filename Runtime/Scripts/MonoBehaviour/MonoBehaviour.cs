// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 26/02/2021 12:04:32 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Collections;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public static class MonoBehaviourX
    {
        public static Coroutine delay(this MonoBehaviour monoBehaviour, Action action, float delay = 0)
        {
            IEnumerator _delay()
            {
                if (delay == 0) yield return null;
                else yield return new WaitForSeconds (delay);
                action?.Invoke ();
            }
            return monoBehaviour.StartCoroutine (_delay ());
        }
    }
}