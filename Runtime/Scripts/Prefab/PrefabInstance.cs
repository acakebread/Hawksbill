// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 05/01/2021 19:46:01 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    [System.Serializable]
    public class PrefabInstance<T> where T : UnityEngine.Object
    {
        public T prefab, instance;
        public void instantiate(Transform parent) => (instance = UnityEngine.Object.Instantiate (prefab, parent)).name = prefab.name;
    }
}