// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 04/05/2021 11:49:45 by seantcooper
using UnityEngine;
using Hawksbill;
using UnityEngine.VFX;

namespace Hawksbill.VFX
{
    [ExecuteInEditMode]
    ///<summary>Bind value to VisualEffect Property</summary>
    public class VFXBindTransform : MonoBehaviour
    {
        public VisualEffect effect;
        public TransformType type;
        public string bindingName = "";

        void Update()
        {
            if (!effect) return;
            switch (type)
            {
                case TransformType.Transform:
                    effect.SetTransform (bindingName, transform);
                    break;

                case TransformType.WorldToLocalMatrix:
                    effect.SetMatrix4x4 (bindingName, transform.worldToLocalMatrix);
                    break;

                case TransformType.LocalToWorldMatrix:
                    effect.SetMatrix4x4 (bindingName, transform.worldToLocalMatrix);
                    break;

                case TransformType.Position:
                    effect.SetVector3 (bindingName, transform.position);
                    break;

                case TransformType.Rotation:
                    effect.SetVector3 (bindingName, transform.eulerAngles);
                    break;

                case TransformType.Scale:
                    effect.SetVector3 (bindingName, transform.lossyScale);
                    break;

                case TransformType.LocalPosition:
                    effect.SetVector3 (bindingName, transform.localPosition);
                    break;

                case TransformType.LocalRotation:
                    effect.SetVector3 (bindingName, transform.localEulerAngles);
                    break;

                case TransformType.LocalScale:
                    effect.SetVector3 (bindingName, transform.localScale);
                    break;
            }
        }

        public enum TransformType
        {
            Transform,
            WorldToLocalMatrix,
            LocalToWorldMatrix,
            Position,
            Rotation,
            Scale,
            LocalPosition,
            LocalRotation,
            LocalScale,
        }
    }
}