// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 06/08/2021 17:48:25 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Linq;
using System.Collections.Generic;
using static Hawksbill.DynamicDescription;
using static Hawksbill.DynamicDescription.Field;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class Dynamic : MonoBehaviour
    {
        [SerializeField] internal DynamicDescription description;
        [SerializeField] internal FieldValueCollection values;

        public Value this[string name] => values[name]?.value ?? "";
        public bool hasField(string name) => description.hasField (name);

        public Dictionary<string, object> ToObject() => values.ToObject ();

        void OnValidate()
        {
            if (values == null) values = new FieldValueCollection ();
            values.connect (description);
            values.validate (description);
        }
    }
}