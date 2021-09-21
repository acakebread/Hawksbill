// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 18/04/2021 09:30:31 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class ApplyVelocity : MonoBehaviour
    {
        public Vector3 velocity;
        public float decay = 1;

        IEnumerator Start()
        {
            float time = Time.time;
            while (enabled)
            {
                yield return null;
                transform.position += Vector3.Lerp (velocity, Vector3.zero, (Time.time - time) / decay) * Time.deltaTime;
            }
        }
    }
}