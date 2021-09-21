// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 19/06/2021 09:10:38 by seantcooper
using UnityEngine;
using V3 = UnityEngine.Vector3;
using V2 = UnityEngine.Vector2;
using System;
using System.Linq;
using Hawksbill.Analytics;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace Hawksbill
{
    public class Blitter : IDisposable
    {
        RenderTexture renderTexture;
        RenderTexture prevRenderTexture;

        public Blitter(RenderTexture renderTexture)
        {
            // GL.InvalidateState ();
            prevRenderTexture = RenderTexture.active;
            RenderTexture.active = this.renderTexture = renderTexture;
            Graphics.SetRenderTarget (this.renderTexture);
            GL.PushMatrix ();
            GL.LoadOrtho ();
        }

        void IDisposable.Dispose()
        {
            GL.PopMatrix ();
            RenderTexture.active = prevRenderTexture;
        }

        public void clear() => clear (Color.black);
        public void clear(Color color) => GL.Clear (true, true, color);
        public void clear(Texture2D texture) { clear (Color.black); drawTexture (texture); }

        public void applyBlits(Blit[] blits) => blits.ForAll (blit => blit.draw (this));

        // DRAW TEXTURE
        public void drawTexture(Texture2D texture) => drawTexture (texture, V2.one / 2);

        public void drawTexture(Texture2D texture, float opacity) =>
            drawTexture (texture, Texture2D.whiteTexture, Color.white, opacity, V2.one / 2);

        public void drawTexture(Texture2D texture, V2 position, float rotation = 0, float scale = 1, BlendMode blendMode = BlendMode.Normal) =>
            drawTexture (texture, Texture2D.whiteTexture, Color.white, 1, position, rotation, scale, blendMode);

        public void drawTexture(Texture2D texture, Texture2D mask, Color tint, float opacity, V2 position, float rotation = 0, float scale = 1, BlendMode blendMode = BlendMode.Normal)
        {
            // Debug.Log ("Draw Texture");
            Material material = getMaterial (blendMode);
            material.SetTexture ("_MainTex", texture);
            material.SetTexture ("_Back", renderTexture);
            material.SetTexture ("_Mask", mask);
            material.SetColor ("_Color", tint);
            material.SetFloat ("_Alpha", opacity);
            material.SetPass (0);
            Graphics.DrawMeshNow (quad, Matrix4x4.TRS (position, Quaternion.Euler (0, 0, rotation), V3.one * scale), 0);
            GL.Flush ();
        }

        // DRAW MATERIAL
        // public void drawMaterial(Material material) => drawMaterial (material, V2.one / 2, 0, 1);
        // public void drawMaterial(Material material, V2 position, float rotation = 0, float scale = 1)
        // {
        //     material.SetPass (0);
        //     Graphics.DrawMeshNow (quad, Matrix4x4.TRS (position, Quaternion.Euler (0, 0, rotation), V3.one * scale), 0);
        //     GL.Flush ();
        // }


        public enum BlendMode
        {
            Normal = 0,
            Add = 1,
            Subtract = 2,
            Multiply = 3,
            Divide = 3,
        }

        static Dictionary<BlendMode, Material> materials;
        Material getMaterial(BlendMode blendMode)
        {
            if (materials == null) materials = new Dictionary<BlendMode, Material> ();
            return materials[blendMode] = new Material (Shader.Find ("Blitter/Blend" + blendMode));
        }

        static Mesh _quad;
        static Mesh quad
        {
            get
            {
                if (!_quad)
                {
                    _quad = new Mesh
                    {
                        vertices = new V3[] { new V3 (-.5f, -.5f), new V3 (-.5f, .5f), new V3 (.5f, .5f), new V3 (.5f, -.5f) },
                        uv = new V2[] { new V2 (0, 0), new V2 (0, 1), new V2 (1, 1), new V2 (1, 0) },
                        triangles = new int[] { 0, 2, 1, 0, 3, 2 },
                        normals = new V3[] { new V3 (0, 0, 1), new V3 (0, 0, 1), new V3 (0, 0, 1), new V3 (0, 0, 1) },
                    };
                }
                return _quad;
            }
        }

        [Serializable]
        public class Blit
        {
            public bool enabled = true;
            public Blitter.BlendMode blendMode = Blitter.BlendMode.Normal;
            public Texture2D texture, mask;
            public Color tint = Color.white;
            [Range (0, 1)] public float opacity = 1;
            public V2 position = new V2 (0.5f, 0.5f);
            [Range (0, 10)] public float scale = 1;
            [Range (-360, 360)] public float angle = 0;

            //public Blit() { Debug.Log ("Blit::Constructor"); }
            internal void draw(Blitter blitter)
            {
                if (enabled)
                    blitter.drawTexture (texture, mask, tint, opacity, position, angle, scale, blendMode);
            }
        }
    }
}
