// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/05/2021 15:41:10 by seantcooper

using UnityEngine;
using Hawksbill;
using System.Linq;
using Hawksbill.Sequencing;

namespace Hawksbill.Render
{
    [ExecuteInEditMode]
    public class SingleRender : MonoBehaviour
    {
        public float time;
        [Line]
        public FrameRenderer frameRenderer;

        //void OnValidate() => _frames = null;

        void Update() => frameRenderer?.draw (time, transform);

        // int getFrameIndex(float time) => Mathf.RoundToInt (Mathf.Max (0, time) * frameRenderer.clip.frameRate) % _frames.Length;

        // void draw()
        // {
        //     var frames = getFrames ();
        //     if (frames == null || frames.Length == 0) return;
        //     int index = getFrameIndex (time);
        //     Graphics.DrawMesh (frames[index], transform.position, transform.rotation, this.frameRenderer.material, 0);
        // }

        // [ReadOnly] public int frameCount;
        // [ReadOnly] public Mesh[] _frames;

        // // Mesh[] _frames;
        // Mesh[] getFrames()
        // {
        //     if (_frames == null || _frames.Length == 0) _frames = frameRenderer.clip.getAnimationFrames (frameRenderer.prefab, 1f);
        //     frameCount = _frames.Length;
        //     return _frames;
        // }


    }
}