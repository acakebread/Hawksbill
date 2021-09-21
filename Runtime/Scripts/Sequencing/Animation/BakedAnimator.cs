// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 24/07/2021 11:54:32 by seantcooper
using UnityEngine;
using Hawksbill;
using Hawksbill.Sequencing;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class BakedAnimator : MonoBehaviour
    {
        public Clip[] animations;
        public string defaultClip = "Idle";
        Dictionary<string, Clip> anims;
        Clip current;
        float startTime;

        MeshFilter meshFilter;
        bool visible;

        void OnValidate()
        {
            animations = getClips (animations).ToArray ();
        }

        void Start()
        {
            anims = getAnimations (animations).Where (c => c.frames != null).ToSafeDictionary (k => k.name, v => v);

            var material = GetComponentInChildren<MeshRenderer> ().sharedMaterial;

            Destroy (GetComponentInChildren<Animator> ());
            transform.destroyChildren ();

            meshFilter = gameObject.AddComponent<MeshFilter> ();
            gameObject.AddComponent<MeshRenderer> ().sharedMaterial = material;
            setAnimation (defaultClip);
        }

        void Update()
        {
            if (visible && current && current.update ())
                meshFilter.sharedMesh = current.frames[current.frameIndex];
        }

        public bool setAnimation(string name)
        {
            if (anims != null && anims.ContainsKey (name) && current != anims[name])
            {
                (current = anims[name]).reset ();
                //  print ("Set Animation " + name);
                return true;
            }
            return false;
        }

        void OnBecameVisible() => visible = true;
        void OnBecameInvisible() => visible = false;

        static Dictionary<string, Clip[]> cache;
        Clip[] getAnimations(Clip[] clips)
        {
            if (cache == null) cache = new Dictionary<string, Clip[]> ();
            if (!cache.ContainsKey (name))
            {
                clips.Where (c => !c.exclude).ForAll (a => a.build (gameObject));
                cache.Add (name, clips);
            }
            else
            {
                for (int i = 0; i < clips.Length; i++)
                    clips[i].frames = cache[name][i].frames;
            }
            return clips;
        }

        IEnumerable<AnimationClip> getAnimationClips() => GetComponentInChildren<Animator> ().runtimeAnimatorController.animationClips;
        IEnumerable<Clip> getClips(IList<Clip> currentClips)
        {
            var clips = currentClips == null || currentClips.Count == 0 ? new List<AnimationClip> () : currentClips.Select (c => c.clip).ToList ();
            foreach (var clip in getAnimationClips ())
            {
                var index = clips.IndexOf (clip);
                if (index > -1) yield return currentClips[index];
                else yield return new Clip (clip);
            }
        }

        [Serializable]
        public class Clip
        {
            public string name;
            public bool exclude;
            public AnimationClip clip;
            public float frameRate => clip.frameRate;

            [NonSerialized] public Mesh[] frames;
            [NonSerialized] public int frameIndex;
            [NonSerialized] public float time;

            public bool update()
            {
                int index = Mathf.FloorToInt ((Time.time - time) * frameRate) % frames.Length;
                if (index != frameIndex)
                {
                    frameIndex = index;
                    // Debug.Log (name + " " + frameIndex + "/" + frames.Length);
                    return true;
                }
                return false;
            }

            public void reset()
            {
                time = Time.time;
                frameIndex = -1;
            }

            public Clip(AnimationClip clip)
            {
                this.name = clip.name;
                this.clip = clip;
            }

            internal void build(GameObject gameObject) => frames = AnimationClipX.getAnimationFrames (clip, gameObject, true, 1);

            public static implicit operator bool(Clip empty) => empty != null;
        }
    }
}