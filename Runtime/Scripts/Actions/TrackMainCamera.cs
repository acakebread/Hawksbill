// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:02:36 by seancooper
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawksbill.Actions
{
    public class TrackMainCamera : MonoBehaviour
    {
        public float offset = 90;
        [Range (0.0001f, 5)] public float trackSpeed = 1;

        void Update()
        {
            if (Camera.main)
            {
                var p = Camera.main.transform.position;
                var e = transform.eulerAngles;
                var q = Quaternion.LookRotation (transform.position - p);
                var ne = Quaternion.Euler (e.x, q.eulerAngles.y + offset, e.z);
                transform.rotation = Quaternion.Lerp (transform.rotation, ne, Time.deltaTime * trackSpeed);
            }
        }

    }
}
