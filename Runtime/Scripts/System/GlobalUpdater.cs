// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    public class GlobalUpdater : SingletonMonoBehaviour<GlobalUpdater>
    {
        internal Action update;
        internal static void StartInstance(Action update) =>
            GlobalManager.GetOrAddComponent<GlobalUpdater> ().update = update;

        void Update() => update ();
    }

    //[InitializeOnLoad]
    public static class Updater
    {
        static HashSet<Action> Actions = new HashSet<Action> ();

#if UNITY_EDITOR
        static Updater()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) EditorApplication.playModeStateChanged += OnPlayStateChanged;
            else EditorApplication.update += Update;
        }
        static void OnPlayStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
                GlobalUpdater.StartInstance (Update);
        }
#else
        static Updater()
        {
            GlobalUpdater.StartInstance (Update);
        }
#endif

        public static void Add(Action update) => Actions.Add (update);
        public static void Remove(Action update) => Actions.Remove (update);
        public static void Update()
        {
            try
            {
                Actions.ToArray ().ForAll (action => action ());
            }
            catch (Exception e)
            {
                Debug.Log ("Exception in Updater:\n" + e);
                if (GlobalUpdater.GetInstance ())
                    GlobalUpdater.GetInstance ().enabled = false;
            }
        }
    }
}
