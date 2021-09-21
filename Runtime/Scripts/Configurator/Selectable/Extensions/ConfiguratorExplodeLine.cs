// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 05/09/2021 13:08:00 by seantcooper
using System;
using System.Collections;
using Hawksbill.Geometry;
using UnityEngine;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [AddComponentMenu ("")]
    public class ConfiguratorExplodeLine : ConfiguratorExplode //, IEndRenderHandler, IBeginRenderHandler
    {
        [Line]
        [SerializeField, Label ("Path")] LinePath linePath = new LinePath ();
        [SerializeField, Range (0, 2)] float duration = 0.5f;
        [SerializeField, Range (0, 1)] float position = 0;

        void Start()
        {
            position = 0;
        }

        protected override void OnValidate()
        {
            base.OnValidate ();
            if (!linePath.target) linePath.target = transform;
        }

        void OnDrawGizmosSelected()
        {
            if (!gameObject.isSelectedInEditor ()) return;
            linePath?.drawGizmos ();
        }

        Coroutine animateCoroutine;
        internal override void explode()
        {
            if (animateCoroutine != null) StopCoroutine (animateCoroutine);
            animateCoroutine = StartCoroutine (animateTo (1));
        }

        internal override void implode()
        {
            if (animateCoroutine != null) StopCoroutine (animateCoroutine);
            animateCoroutine = StartCoroutine (animateTo (0));
        }

        IEnumerator animateTo(float endPosition)
        {
            float startPosition = position;
            if (endPosition != startPosition)
            {
                float duration = this.duration * Mathf.Abs (endPosition - startPosition);
                for (Tween.UnitTime unitTime = new Tween.UnitTime (duration); unitTime < 1; position = Mathf.Lerp (startPosition, endPosition, unitTime))
                    yield return null;
            }
            position = endPosition;
        }

        void LateUpdate()
        {
            linePath.restoreLocalPosition ();
            if (isRenderable)
                linePath.setLocalPosition (position);
        }

        // void IBeginRenderHandler.OnBeginRender() { if (isRenderable) linePath.setLocalPosition (position); }
        // void IEndRenderHandler.OnEndRender() => linePath.restoreLocalPosition ();

        [Serializable]
        class LinePath
        {
            public Transform target;
            public Vector3 offset = Vector3.up * 2;
            public static implicit operator bool(LinePath empty) => empty != null;

            internal void drawGizmos()
            {
                if (!target) return;
                Gizmos.matrix = target.localToWorldMatrix;
                Vector3 p1 = target.localPosition, p2 = target.localPosition + offset;
                Gizmos.DrawLine (p1, p2);

            }

            [SerializeField] bool isAssigned;
            [SerializeField] Vector3 backupLocalPosition;
            [SerializeField] Vector3 assignedLocalPosition;

            internal void setLocalPosition(float position)
            {
                if (!target || isAssigned) return;
                backupLocalPosition = target.localPosition;
                assignedLocalPosition = target.localPosition = (Vector3) backupLocalPosition + offset * position;
                isAssigned = true;
            }

            internal void restoreLocalPosition()
            {
                if (!target || !isAssigned) return;
                Vector3 offset = assignedLocalPosition - target.localPosition;
                target.localPosition = (Vector3) backupLocalPosition + offset;
                isAssigned = false;
            }
        }
    }
}