// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hawksbill.UI
{
    [System.Serializable]
    [CreateAssetMenu (menuName = "Hawksbill/UI/Resources")]
    public class Resources : ScriptableObject
    {
        [Header ("Textures")]
        public Texture2D[] textures;
        [System.Serializable]
        public class TextureItem
        {
            public string name;
            public Texture2D texture;
        }
        public Texture2D getTexture(string name) => validateTexture (textures.FirstOrDefault (t => t.name == name));
        Texture2D validateTexture(Texture2D texture) => texture ? texture : Texture2D.blackTexture;
    }
}
