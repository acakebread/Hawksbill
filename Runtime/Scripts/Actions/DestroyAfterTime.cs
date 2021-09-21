// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:02:36 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawksbill.Actions
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public float time = 1;

        IEnumerator Start()
        {
            yield return new WaitForSeconds (time);
            Destroy (gameObject);
        }
    }
}
