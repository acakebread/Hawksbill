// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 31/08/2021 08:36:07 by seantcooper
using System;
using UnityEngine;

namespace Hawksbill
{
    ///<summary>Creates a pretty readme file</summary>
    [System.Serializable, CreateAssetMenu (menuName = "Hawksbill/Readme")]
    public class Readme : ScriptableObject
    {
        public int priority = 10;
        public Texture2D image; // Landscape
        public string title;
        public Section[] sections;

        [Serializable]
        public class Section
        {
            public string header;
            public string body;
            public string link;
        }
    }
}