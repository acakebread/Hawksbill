// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/04/2021 08:33:38 by seantcooper
using UnityEngine;
using Hawksbill;
using System;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class PoolItem : MonoBehaviour
    {
        [HideInInspector] public PoolContainer container;
        [SerializeField, HideInInspector] protected bool instantiated = true;

        internal void activate()
        {
            gameObject.SetActive (true);
        }

        internal void deactivate()
        {
            instantiated = false;
            gameObject.SetActive (false);
        }
    }
}