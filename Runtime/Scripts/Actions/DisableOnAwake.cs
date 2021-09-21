// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:02:36 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawksbill.Actions
{
    public class DisableOnAwake : MonoBehaviour
    {
        public UnityEngine.Object target;
        void Awake()
        {
            if (target is Renderer) (target as Renderer).enabled = false;
            else if (target is Collider) (target as Collider).enabled = false;
            else if (target is Behaviour) (target as Behaviour).enabled = false;
            else if (target) Debug.Log ("DisableOnPlay Target type is unknown: " + target.GetType ());
            else gameObject.SetActive (false);
        }
    }
}
