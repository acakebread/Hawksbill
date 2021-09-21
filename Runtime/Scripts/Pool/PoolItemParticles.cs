// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/04/2021 10:36:32 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Linq;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class PoolItemParticles : PoolItem
    {
        public ParticleSystem[] particleSystems;
        void Awake()
        {
            // particleSystems = GetComponentsInChildren<ParticleSystem> ().Where (p => p.transform.parent = transform).ToArray ();
            // if (particleSystems.Length == 0)
            //     Debug.LogError ("GameObject must have at least one particle system!");
        }

        public void OnParticleSystemStopped()
        {
            container?.destroy (this);
        }

        void OnEnable()
        {
            if (instantiated) return;
            foreach (var particleSystem in particleSystems)
            {
                particleSystem.Clear (true);
                particleSystem.Play (true);
            }
        }
    }
}