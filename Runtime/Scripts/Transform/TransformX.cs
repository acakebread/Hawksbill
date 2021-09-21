// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 17/02/2021 16:59:14 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections.Generic;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hawksbill
{
    public static class TransformX
    {
        ///<summary>Returns a string containing an alpha character order!</summary>
        public static string getHierarchyOrder(this Transform transform) =>
            String.Join ("_", transform.parents (true).Select (t => t.GetSiblingIndex ().ToString ("X4")).Reverse ());

        ///<summary>Returns true and out int value containing the parent (-) or child (+) distance, and 0 for equal transform!</summary>
        public static bool getHierarchyDistance(this Transform transform1, Transform transform2, out int index)
        {
            if ((index = -transform1.parents (true).FindIndex (t => t == transform2)) < 1) return true;
            return ((index = transform1.children (true).FindIndex (t => t == transform2)) > -1);
        }

        ///<summary>Does sibling shares the same Parent?</summary>
        public static bool isSibling(this Transform sibling1, Transform sibling2) => sibling2.parent == sibling1.parent;

        ///<summary>Is the transform a child of this?</summary>
        /// transform.isChild(child);
        public static bool isChild(this Transform transform, Transform child) =>
            transform.GetComponentsInChildren<Transform> (true).FirstOrDefault (t => t == child);

        ///<summary>Is the transform a parent of this?</summary>
        /// transform.isParent(parent);
        public static bool isParent(this Transform transform, Transform parent) =>
            transform.GetComponentsInParent<Transform> (true).FirstOrDefault (t => t == parent);

        public static void reset(this Transform transform)
        {
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public static IEnumerable<Transform> children(this Transform transform, bool recursive = false)
        {
            IEnumerable<Transform> _children(Transform parent)
            {
                foreach (var child in parent)
                    yield return (Transform) child;
            }
            if (recursive)
            {
                var c = _children (transform);
                return c.Concat (c.SelectMany (t => t.children (true)));
            }
            return _children (transform);
        }

        public static IEnumerable<Transform> parents(this Transform transform, bool includeSelf = false)
        {
            if (includeSelf) yield return (Transform) transform;
            for (transform = transform.parent; transform; transform = transform.parent)
                yield return (Transform) transform;
        }

        public static T getParentComponent<T>(this Transform transform) where T : UnityEngine.Component =>
           transform.GetComponentsInParent<T> ().FirstOrDefault (t => t.transform != transform);

        public static void destroyChildren(this Transform transform)
        {
            var children = transform.children ().Select (t => t.gameObject).ToArray ();
            children.ForAll (g => g.destroy ());
        }

        // public static IEnumerable<T> children<T>(this Transform transform) where T : UnityEngine.Component
        // {
        //     foreach (var child in transform)
        //         yield return (child as Transform).GetComponent<T> ();
        // }

    }
}