// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 30/07/2021 09:49:44 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class StateMachine : MonoBehaviour
    {
#if UNITY_EDITOR
        [ReadOnly] public string currentStateName;
#endif
        protected State currentState;
        Coroutine updateHandle;
        Dictionary<Type, State> states;

        protected virtual void OnEnable()
        {
            if (currentState) startCurrent ();
        }

        protected virtual void OnDisable() => updateHandle = null;

        protected bool setState<T>() where T : State
        {
            if (currentState)
            {
                if (typeof (T) == currentState.GetType ()) return false;
                if (updateHandle != null) StopCoroutine (updateHandle);
                currentState.exit ();
            }
            currentState = activateState<T> ();
#if UNITY_EDITOR
            currentStateName = typeof (T).Name;
#endif
            currentState.enter ();
            startCurrent (true);
            return true;
        }

        void startCurrent(bool delay = false)
        {
            if (!gameObject.activeInHierarchy) return;
            if (delay) this.delay (() => startCurrent ());
            else updateHandle = StartCoroutine (currentState.update ());
        }

        protected bool addState<T>() where T : State => activateState<T> ();

        T activateState<T>() where T : State
        {
            Type type = typeof (T);
            if (states == null || !states.ContainsKey (type))
            {
                if (states == null) states = new Dictionary<Type, State> ();
                states.Add (type, (State) Activator.CreateInstance (type));
                states[type].machine = this;
            }
            return (T) states[type];
        }

        [Serializable]
        public abstract class State
        {
            internal StateMachine machine;
            protected Transform transform => machine.transform;
            public T getStateMachine<T>() where T : StateMachine => (T) machine;
            internal virtual void enter() { }
            internal abstract IEnumerator update();
            internal virtual void exit() { }
            public static implicit operator bool(State empty) => empty != null;
            protected bool setState<T>() where T : State => machine.setState<T> ();
        }

        // Do nothing State
        public class StateNone : State
        { internal override IEnumerator update() { while (true) yield return new WaitForSeconds (1); } }

    }
}