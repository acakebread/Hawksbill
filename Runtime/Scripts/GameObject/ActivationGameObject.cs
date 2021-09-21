// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 23/01/2021 10:53:11 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class ActivationGameObject : MonoBehaviour
    {
        [Range (0, 60)] public float delayActivation;
        public GameObject[] activation;

        [Range (0, 60)] public float delayDeactivation;
        public GameObject[] deactivation;

        void Start()
        {
            IEnumerator setActive(bool state, float delay, GameObject[] objects)
            {
                if (delay > 0) yield return new WaitForSeconds (delay);
                objects?.ForAll (g => g.SetActive (state));
            }
            StartCoroutine (setActive (true, delayActivation, activation));
            StartCoroutine (setActive (false, delayDeactivation, deactivation));
        }
    }
}