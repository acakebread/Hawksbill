// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 23/01/2021 10:53:11 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class ActivationGroup : MonoBehaviour
    {
        [Button] public bool deactivateInEditor;
        [Button] public bool activateInEditor;
        [Button] public bool deactivateOnAwake;

        void Awake() { if (deactivateOnAwake) deactivate (); }
        void OnValidate()
        {
            if (deactivateInEditor)
            {
                deactivateInEditor = false;
                deactivate ();
            }
            if (activateInEditor)
            {
                activateInEditor = false;
                activate ();
            }
        }

        public void activate() => deactivationObjects.ForAll (g => g.SetActive (true));
        public void deactivate() => deactivationObjects.ForAll (g => g.SetActive (false));

        public GameObject[] deactivationObjects;
    }
}