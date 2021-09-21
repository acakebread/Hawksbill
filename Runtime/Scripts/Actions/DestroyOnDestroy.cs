// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:02:36 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawksbill.Actions
{
    public class DestroyOnDestroy : MonoBehaviour
    {
        public UnityEngine.Object target;

        IEnumerator Start()
        {
            while (enabled)
            {
                if (target == null)
                    Destroy (gameObject);
                yield return null;
            }
        }
    }
}

