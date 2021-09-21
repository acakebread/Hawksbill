// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 17/07/2021 07:20:26 by seantcooper
using UnityEngine;
using Hawksbill;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    [ExecuteInEditMode]
    public class PrefabIsolationRenderMesh : MonoBehaviour
    {
        public bool includeChildren = true;
        MeshRenderer[] meshRenderers => includeChildren ? GetComponentsInChildren<MeshRenderer> () : new MeshRenderer[] { GetComponent<MeshRenderer> () };

#if UNITY_EDITOR
        void Update()
        {
            var stage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage ();
            meshRenderers.ForAll (r => r.forceRenderingOff = stage == null);
        }
#endif
    }
}