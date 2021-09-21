// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 16/07/2021 11:41:49 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    [System.Serializable, CreateAssetMenu (menuName = "Hawksbill/Objects/Texture2D List")]
    public class Texture2DList : ScriptableObject
    {
        public UnityEngine.Texture2D[] textures;
        public UnityEngine.Texture2D this[int i] => textures[i];
        public int count => textures.Length;
    }
}