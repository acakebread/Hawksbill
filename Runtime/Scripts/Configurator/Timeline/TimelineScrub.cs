// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 23/08/2021 20:47:42 by seantcooper
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hawksbill.Analytics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using static Hawksbill.Configurator.TimelineQuery;

namespace Hawksbill.Configurator
{
    [ExecuteInEditMode]
    public class TimelineScrub : MonoBehaviour
    {
        const double FastSpeed = 10;
        public bool skipWarnings;
        public bool autoPlay;
        public int initialTime = 0;

        [Line]
        [SerializeField, HideInInspector] double lastTime;
        [SerializeField, HideInInspector] double speed;
        [HideInInspector] public bool showChildren;
        [HideInInspector] public State currentState;

        public PlayableDirector director => GetComponent<PlayableDirector> ();
        TimelineAsset asset => director.playableAsset as TimelineAsset;
        public double fps => asset.editorSettings.fps;
        public double frameDuration => 1 / fps;
        public double duration => director.duration;

        void OnValidate()
        {
            setState (State.Stop);
            time = 0;
        }

        void Start()
        {
            if (Application.isPlaying)
            {
                if (autoPlay)
                {
                    time = initialTime;
                    setState (State.PlayForward);
                }
                else setState (State.Stop);
            }
        }

        void Update()
        {
            scrub (lastTime, director.time);
        }

        void gotoStart()
        {
            scrub (director.time, 0);
            setState (State.Stop);
        }

        public double time
        {
            get => director.time;
            set => scrub (director.time, value);
        }

        double clampTime(double time) => math.clamp (time, 0, duration);

        public void setState(State state)
        {
            //print ("Set State " + state);
            switch (currentState = state)
            {
                case State.None:
                    setState (State.Stop);
                    break;
                case State.GotoStart:
                    time = 0;
                    setState (State.Stop);
                    break;
                case State.StepBack:
                    time = clampTime (time - 1 / fps);
                    setState (State.Stop);
                    break;
                case State.FastReverse:
                    speed = -FastSpeed;
                    break;
                case State.PlayReverse:
                    speed = -1;
                    break;
                case State.Stop:
                    speed = 0;
                    break;
                case State.PlayForward:
                    speed = 1;
                    break;
                case State.FastForward:
                    speed = FastSpeed;
                    break;
                case State.StepForward:
                    time = clampTime (time + 1 / fps);
                    setState (State.Stop);
                    break;
                case State.GotoEnd:
                    time = director.duration;
                    setState (State.Stop);
                    break;
            }

            if (speed == 0) stopPlay ();
            else startPlay ();
        }

        void play()
        {
            float deltaTime = Time.realtimeSinceStartup - currentTime;
            currentTime = Time.realtimeSinceStartup;
            // print ("Play " + Time.deltaTime + " " + Time.unscaledDeltaTime);
            // if (Time.deltaTime == 0) return;
            var lastTime = time;
            var newTime = time + speed * deltaTime;
            var setTime = clampTime (newTime);
            time = setTime;

            // print ("setTime:" + deltaTime);

            if (newTime != setTime) setState (State.Stop);
        }

        Coroutine playback;
        float currentTime;
        double startPosition;
        void startPlay()
        {
            currentTime = Time.realtimeSinceStartup;
            startPosition = time;
            if (Application.isPlaying)
            {
                stopPlay ();
                // StopAllCoroutines ();
                playback = StartCoroutine (CoroutineX.Simple (play));
            }
#if UNITY_EDITOR
            else
            {
                UnityEditor.EditorApplication.update -= play;
                UnityEditor.EditorApplication.update += play;
            }
#endif
        }
        void stopPlay()
        {
            if (Application.isPlaying) { if (playback != null) StopCoroutine (playback); playback = null; }
#if UNITY_EDITOR
            else UnityEditor.EditorApplication.update -= play;
#endif
        }

        void scrub(double from, double to)
        {
            if (to == from) return;
            Profiler.Start ("Scrub");
            if (to > from)
            {
                if (skipWarnings) to = skip (from, to, true);
                evaluateAt (scrubForward (from, to));
            }
            else if (to < from)
            {
                if (skipWarnings) to = skip (to, from, false);
                evaluateAt (scrubBackwards (to, from));
            }
            evaluateAt (lastTime = to);
            Profiler.Stop ("Scrub");
        }

        const double MS = 0.001f;

        class Range { public double min, max; }

        double skip(double min, double max, bool forwards = true)
        {
            var clips = director.getClipInfos ();
            double startValue = forwards ? max : min;

            var warningSkips = clips.Where (c => c.skipType == SkipType.Full).
                OrderBy (c => c.start).Select (c => new Range { min = c.start, max = c.end });

            IEnumerable<Range> getSkips()
            {
                var warningSkips = clips.Where (c => c.skipType == SkipType.Full).
                    OrderBy (c => c.start).Select (c => new Range { min = c.start - MS, max = c.end + MS });

                var current = warningSkips.FirstOrDefault ();
                if (current == null) yield break;

                foreach (var r in warningSkips.Skip (1))
                {
                    if (current == null) current = r;
                    if (r.min <= current.max) current.max = r.max;
                    else
                    {
                        yield return current;
                        current = null;
                    }
                }
                if (current == null) yield return current;
            }

            var skips = getSkips ().ToArray ();
            double value = forwards ? max : min;
            var skip = skips.FirstOrDefault (r => value >= r.min && value <= r.max);
            if (skip != null) return forwards ? skip.max : skip.min;
            return value;

            // // for (int i = 100; i > 0; --i)
            // // {
            // //     double value = forwards ? max : min;
            // //     var range = clips.Where (c => value >= c.start && value < c.end);
            // //     var skips = range.Where (c => c.skipType == SkipType.Full);
            // //     var values = skips.Select (c => forwards ? c.end : c.start).Distinct ().OrderBy (s => s);
            // //     double p = forwards ? values.LastOrDefault () : values.FirstOrDefault ();
            // //     if (p == 0 || p == value) break;
            // //     if (forwards) max = p; else min = p - frameDuration;
            // // }
            // // var skipTo = forwards ? max : min;
            // // print ("Skip " + skipTo + " " + startValue + " delta:" + (skipTo - startValue));
            // return skipTo;
        }

        TimelineQuery.Collection getScrubbable()
        {
            var collection = director.getClipInfos ();
            var skippable = skipWarnings ? collection.getSkippable () : new Collection ();
            return skipWarnings ? new Collection (collection.Except (skippable)) : collection;
        }

        IEnumerable<double> scrubForward(double min, double max) =>
            getScrubbable ().Select (c => c.end - MS).Where (s => s >= min && s < max).Distinct ().OrderBy (s => s);

        IEnumerable<double> scrubBackwards(double min, double max) =>
            getScrubbable ().Select (c => c.start).Where (s => s >= min && s < max).Distinct ().OrderBy (s => -s);

        void evaluateAt(IEnumerable<double> times) => times.ForAll (time => evaluateAt (time));
        void evaluateAt(double time)
        {
            director.time = time;
            director.Evaluate ();
        }

        void OnGUI()
        {
            Texture2D[] icons = ProjectSettings.Instance.media.playControls.textures;
            void drawControls()
            {
                float scalar = 1;
                float Width = 400 * scalar, Height = 36 * scalar, Padding = 4 * scalar;
                float x = (Screen.width - Width) / 2;
                Rect rect = new Rect (x, Screen.height - (Height + Padding), Width, Height);
                State selected = (State) GUI.Toolbar (rect, (int) currentState, icons);
                if (selected != currentState)
                {
                    currentState = selected;
                    setState (currentState);
                }
            }
            drawControls ();
        }

        public enum State
        {
            None = -1,
            GotoStart = 0,
            StepBack = 1,
            FastReverse = 2,
            PlayReverse = 3,
            Stop = 4,
            PlayForward = 5,
            FastForward = 6,
            StepForward = 7,
            GotoEnd = 8,
        }
    }
}