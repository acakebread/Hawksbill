// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 21/06/2021 08:27:02 by seantcooper
using UnityEngine;
using Hawksbill;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

namespace Hawksbill
{
    [System.Serializable, CreateAssetMenu (menuName = "Hawksbill/Shaders/Photoshop Shader Builder")]
    public class PSShaderBuilder : ScriptableObject
    {
        public Shader template;
        public string key = "_Template_";
        public string[] modes;
        [Button] public bool build = false;
#if UNITY_EDITOR
        void OnValidate()
        {
            if (build)
            {
                build = false;
                var templatePath = AssetDatabase.GetAssetPath (template.GetInstanceID ());
                var templateText = File.ReadAllText (templatePath);
                Debug.Log ("Shader path = " + templatePath);
                foreach (var mode in modes)
                {
                    var path = templatePath.Replace (key, mode);
                    var text = templateText.Replace (key, mode);
                    File.WriteAllText (path, text);
                    Debug.Log ("-Shader written " + path);
                }
            }
        }
#endif
    }
}
#endif