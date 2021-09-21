// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 16/06/2021 14:11:46 by seantcooper
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Hawksbill
{
    [System.Serializable, CreateAssetMenu (menuName = "Hawksbill/Objects/Object List")]
    public class ObjectList : ScriptableObject
    {
        public UnityEngine.Object[] objects;
        public UnityEngine.Object this[int i] => objects[i];
        public int count => objects.Length;
    }
}