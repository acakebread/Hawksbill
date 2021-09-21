// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 16/06/2021 14:11:46 by seantcooper
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Hawksbill
{
    [System.Serializable, CreateAssetMenu (menuName = "Hawksbill/Objects/Object Collection")]
    public class ObjectCollection : ScriptableObject
    {
        public Collection[] collections;
        public UnityEngine.Object[] getObjects(string name) => collections.FirstOrDefault (c => c.name == name)?.objects;
        public Collection getCollection(string name) => collections.FirstOrDefault (c => c.name == name);
        public IEnumerable<string> getCollectionNames(string name) => collections.Select (c => c.name);

        [System.Serializable]
        public class Collection
        {
            public string name;
            public UnityEngine.Object[] objects;
            public int length => objects.Length;
            public static implicit operator bool(Collection empty) => empty != null;
        }
    }
}