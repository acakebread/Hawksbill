// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/05/2021 18:20:51 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class LayerID : MonoBehaviour
    {
        public int layerID;
        void OnValidate()
        {
            layerID = gameObject.layer;
        }
    }
}