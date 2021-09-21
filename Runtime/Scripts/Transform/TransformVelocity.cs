// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 16/04/2021 17:23:42 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections.Generic;
using System.Linq;
using System;
using V3 = UnityEngine.Vector3;
using Hawksbill.Geometry;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class TransformVelocity : MonoBehaviour
    {
        [Range (1, 120)] public int maxEntries = 16;
        [Line]

        float positionTime;

        void Start()
        {
            trackingStart ();
        }

        void Update()
        {
            trackingUpdate ();
        }

        TransformBase lastTransform;
        List<Record> records;

        [Line]
        [ReadOnly] public Vector3 currentVelocity;
        [ReadOnly] public Vector3 averageVelocity;
        [Line]
        [ReadOnly] public Vector3 currentAngularVelocity;
        [ReadOnly] public Vector3 averageAngularVelocity;
        [Line]
        [ReadOnly] public float currentSpeed;
        [ReadOnly] public float averageSpeed;
        [Line]
        [ReadOnly] public float currentAngularSpeed;
        [ReadOnly] public float averageAngularSpeed;

        public void trackingStart()
        {
            records = new List<Record> () { new Record () };
            lastTransform = new TransformBase (transform);
        }

        public void trackingUpdate()
        {
            V3 getVelocity() => (transform.position - lastTransform.position) / Time.deltaTime;
            V3 getAngularVelocity()
            {
                V3 difference = (transform.eulerAngles - lastTransform.eulerAngles);
                for (int i = 0; i < 3; i++)
                    if (difference[i] < -180) difference[i] += 360; else if (difference[i] > 180) difference[i] -= 360;
                return difference / Time.deltaTime;
            }

            //ADD
            records.Add (new Record (getVelocity (), getAngularVelocity ()));
            lastTransform = new TransformBase (transform);

            //TOO MANY
            while (records.Count > maxEntries) records.RemoveAt (0);

            updateValues ();
        }

        void updateValues()
        {
            currentVelocity = records.Last ().velocity;
            currentSpeed = currentVelocity.magnitude;

            currentAngularVelocity = records.Last ().angular;
            currentAngularSpeed = currentAngularVelocity.magnitude;

            averageVelocity = records.Select (r => r.velocity).Aggregate (new Vector3 (0, 0, 0), (s, v) => s + v) / records.Count;
            averageSpeed = averageVelocity.magnitude;

            //var change = 
            averageAngularVelocity = records.Select (r => r.angular).Aggregate (new Vector3 (0, 0, 0), (s, v) => s + v) / records.Count;
            averageAngularSpeed = averageAngularVelocity.magnitude;
        }

        class Record
        {
            public V3 velocity, angular;
            public Record() { }
            public Record(V3 velocity, V3 angular)
            {
                this.velocity = velocity;
                this.angular = angular;
            }
        }
    }
}