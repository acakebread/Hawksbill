// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 18/05/2021 17:38:45 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEditor;

namespace Hawksbill.Sequencing
{
    ///<summary>Extension MonoBahavior Reliant on Playable</summary>
    [RequireComponent (typeof (SplinePlayable))]
    public class SplineExtension : MonoBehaviour
    {
        public SplinePlayable playable => GetComponent<SplinePlayable> ();
        public SplineData data => playable.data;
        public float time => playable.time;
        internal virtual void OnValidate() => playable.validate ();
    }

    public interface ISplineExTransformable
    {
        int priority { get; }
        Vector3 transformPosition(Vector3 v);
        Vector3[] transformPositions(Vector3[] vs);
    }
}