// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Hawksbill
{
    public static class TextureUtility
    {
        public static Texture2D CreateReadable(this Texture2D texture)
        {
            int width = texture.width, height = texture.height;
            RenderTexture.active = RenderTexture.GetTemporary (width, height);
            RenderTexture.active.filterMode = texture.filterMode;
            Graphics.Blit (texture, RenderTexture.active);

            var output = new Texture2D (width, height);
            output.ReadPixels (new Rect (0, 0, width, height), 0, 0);
            output.Apply ();

            RenderTexture.active = null;
            return output;
        }

        public static Texture2D CreateTexture(this Texture2D texture, int width = 0, int height = 0, RenderTextureFormat format = RenderTextureFormat.Default, FilterMode mode = FilterMode.Trilinear, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Linear)
        {
            RenderTexture rt = RenderTexture.GetTemporary (width > 0 ? width : texture.width, height > 0 ? height : texture.height);
            rt.filterMode = mode;
            RenderTexture.active = rt;
            Graphics.Blit (texture, rt);
            var nTex = new Texture2D (rt.width, rt.height);
            nTex.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
            nTex.Apply ();
            RenderTexture.active = null;
            return nTex;
        }

        public static RenderTexture RenderWithMaterial(this Texture2D texture, Material material)
        {
            RenderTexture rt = new RenderTexture (texture.width, texture.height, 32);
            RenderTexture.active = rt;
            Graphics.Blit (texture, rt, material);
            RenderTexture.active = null;
            return rt;
        }

        static Material _blendMaterial;
        static Material blendMaterial = _blendMaterial ? _blendMaterial : _blendMaterial = new Material (Shader.Find ("Blitter/BlitAlpha"));

        public static void Blit(this RenderTexture destination, Texture2D source, float alpha = 1)
        {
            RenderTexture.active = destination;
            blendMaterial.SetColor ("_Color", new Color (1, 1, 1, alpha));
            Graphics.Blit (source, destination, blendMaterial);
            RenderTexture.active = null;
        }

        public static Texture2D ToTexture2D(this RenderTexture source)
        {
            RenderTexture.active = source;
            var output = new Texture2D (source.width, source.height);
            output.ReadPixels (new Rect (0, 0, source.width, source.height), 0, 0);
            output.Apply ();
            RenderTexture.active = null;
            return output;
        }

        static Dictionary<Color, Texture2D> TColors = new Dictionary<Color, Texture2D> ();
        public static Texture2D GetColor(float r, float g, float b, float a) => GetColor (new Color (r, g, b, a));
        public static Texture2D GetColor(Color color)
        {
            if (!TColors.ContainsKey (color))
            {
                TColors[color] = new Texture2D (1, 1, TextureFormat.RGBA32, false);
                TColors[color].SetPixel (0, 0, color);
                TColors[color].Apply ();
            }
            return TColors[color];
        }
    }
}
