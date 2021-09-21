// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 01/07/2021 16:17:49 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class RotateAbout : MonoBehaviour
    {
        public float speed = 1;
        public Transform target;
        public Vector3 axis = Vector3.up;
        Vector3 position => target ? target.position : Vector3.zero;
        void Update() => transform.RotateAround (position, axis, speed * Time.deltaTime);
    }
}