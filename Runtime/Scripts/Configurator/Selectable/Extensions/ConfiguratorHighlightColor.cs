// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 25/08/2021 08:52:48 by seantcooper
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace Hawksbill.Configurator
{
    ///<summary>Highlights MeshRenderers on messages from Selectable</summary>
    [AddComponentMenu ("")]
    [ExecuteInEditMode]
    public sealed class ConfiguratorHighlightColor : ConfiguratorHighlight, IObjectSelectedHandler, IObjectDeselectedHandler,
        IControlPressedHandler, IControlOverHandler, IControlOutHandler, IUpdateRenderHandler
    {
        [Range (0, 2)] public float fadeDuration = 0.25f;
        [Line]
        public States states = new States ();

        Material _materialInstance;
        Material materialInstance => _materialInstance ? _materialInstance : _materialInstance = Instantiate (states.material);

        protected override void OnValidate()
        {
            base.OnValidate ();
            if (states != null && !states.material)
            {
                states.material = ProjectSettings.Editor.defaultSelectionMaterial;
                if (!states.material) states.material = new Material (Shader.Find ("Serialized Standard Material"));
            }
        }

        Color currentColor, targetColor;
        float startTime;

        void Update()
        {
            // Pretty.Track ("Update");
        }

        void IUpdateRenderHandler.OnUpdateRender()
        {
            // Pretty.Track ("OnUpdateRender");
            render ();
        }

        void render()
        {
            if (targetColor.a == 0) return;
            float t = Mathf.Clamp01 ((Time.unscaledTime - startTime) / fadeDuration);
            // if (t == 1) return;
            materialInstance.color = Color.Lerp (currentColor, targetColor, t);
            foreach (var filter in getChildrenComponents<MeshFilter> ())
            {
                Matrix4x4 matrix = filter.transform.localToWorldMatrix;
                Mesh mesh = filter.sharedMesh;
                if (!mesh) continue;
                for (int i = 0; i < mesh.subMeshCount; i++)
                    Graphics.DrawMesh (mesh, matrix, materialInstance, filter.gameObject.layer, null, i, null, false, false);
            }
        }

        void IObjectSelectedHandler.OnObjectSelected() => setTargetColor (states.selected, true);
        void IObjectDeselectedHandler.OnObjectDeselected() => setTargetColor (Color.clear);
        void IControlPressedHandler.OnControlPressed() => setTargetColor (states.pressed);
        void IControlOverHandler.OnControlOver() => setTargetColor (states.highlighted);
        void IControlOutHandler.OnControlOut() => setTargetColor (Color.clear);

        void setTargetColor(Color color, bool force = false)
        {
            if (!force && selectable.isSelected) return;
            currentColor = materialInstance.color;
            targetColor = color;
            startTime = Time.unscaledTime;
        }

        [Serializable]
        public class States
        {
            public Material material;
            public Color highlighted = new Color (1, 1, 1, 0.5f);
            public Color pressed = new Color (1, 1, 1, 0.75f);
            public Color selected = new Color (1, 1, 1, 1f);
            public Color disabled = new Color (0.5f, 0.5f, 0.5f, 0.5f);
        }
    }
}