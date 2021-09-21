// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 21/04/2021 17:55:17 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEngine.Playables;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class TimelineEvaluate : MonoBehaviour
    {
        public State state = State.Start;
        PlayableDirector playableDirector => GetComponent<PlayableDirector> ();

        void Start() => evaluate (State.Start);
        void Awake() => evaluate (State.Awake);
        void Update() => evaluate (State.Update);

        void evaluate(State state)
        {
            if (state == this.state)
            {
                playableDirector?.Evaluate ();
            }
        }

        public enum State
        {
            None, Start, Awake, Update
        }
    }
}