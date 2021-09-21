// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:02:36 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawksbill.Actions
{
    public class Rotater : MonoBehaviour
    {
        public float speed = 1;
        void Update() => transform.eulerAngles += new Vector3 (0, speed * Time.deltaTime, 0);
    }
}
