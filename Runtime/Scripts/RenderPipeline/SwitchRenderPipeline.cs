// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 09/09/2021 15:53:48 by seantcooper
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    public static class SwitchRenderPipeline
    {
        const string URPAssetAssemblyType = "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset, Unity.RenderPipelines.Universal.Runtime";
        const string URPAssetType = "UniversalRenderPipelineAsset";

#if UNITY_EDITOR
        [MenuItem ("Hawksbill/Render Pipeline/Switch to Standard Pipeline", true)]
        public static bool switchToSRP_Validate() => GraphicsSettings.renderPipelineAsset;

        [MenuItem ("Hawksbill/Render Pipeline/Switch to Standard Pipeline")]
        public static void switchToSRP() => GraphicsSettings.renderPipelineAsset = null;

        [MenuItem ("Hawksbill/Render Pipeline/Switch to Universal Pipeline", true)]
        public static bool switchToURP_Validate() =>
            Type.GetType (URPAssetAssemblyType) != null && HasAssetOfType (URPAssetType) &&
            (GraphicsSettings.renderPipelineAsset?.GetType () != Type.GetType (URPAssetAssemblyType));

        [MenuItem ("Hawksbill/Render Pipeline/Switch to Universal Pipeline")]
        public static void switchToURP()
        {
            GraphicsSettings.renderPipelineAsset = (RenderPipelineAsset) AssetDatabase.LoadAssetAtPath
                (AssetDatabase.GUIDToAssetPath (FindFirstAssetOfType (URPAssetType)), Type.GetType (URPAssetAssemblyType));
        }

        [MenuItem ("Hawksbill/Render Pipeline/Switch to High Definition Pipeline", true)]
        public static bool switchToHDRP_Validate()
        {
            return false;
        }
        [MenuItem ("Hawksbill/Render Pipeline/Switch to High Definition Pipeline")]
        public static void switchToHDRP()
        {
        }

        // support
        static bool HasAssetOfType(string type) => FindFirstAssetOfType (type) != null;
        static string FindFirstAssetOfType(string type) => AssetDatabase.FindAssets ("t:" + type).FirstOrDefault ();


#endif
    }
}