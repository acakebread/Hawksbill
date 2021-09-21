// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 22/04/2021 08:33:38 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Collections.Generic;
using System.Linq;

namespace Hawksbill
{
    ///<summary>Put text here to describe the Class</summary>
    public class PoolContainer : MonoBehaviour
    {
        public PoolItem prefab;
        [Range (1, 5000)] public int maxCount = 1000;
        public bool preallocate;
        public bool disabled;
        [ReadOnly] public int totalCount;
        [ReadOnly] public int usedCount;
        [ReadOnly] public int freeCount;

        HashSet<PoolItem> used;
        HashSet<PoolItem> free;
        int count => used.Count + free.Count;

        void Start()
        {
            used = new HashSet<PoolItem> ();
            free = new HashSet<PoolItem> ();
        }

        void updateDebug()
        {
#if UNITY_EDITOR
            totalCount = count;
            usedCount = used.Count;
            freeCount = free.Count;
#endif
        }

        public PoolItem instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            PoolItem item;
            if ((item = freePop ()))
            {
                usedAdd (item);
                freeRemove (item);
                if (parent) item.transform.parent = parent;
                item.transform.SetPositionAndRotation (position, rotation);
                item.activate ();
            }
            else if (free.Count == 0 && count < maxCount)
            {
                usedAdd (item = Instantiate (prefab, position, rotation, parent == null ? transform : parent));
                item.container = this;
            }
            else item = null;
            updateDebug ();
            return item;
        }

        public void destroy(PoolItem item)
        {
            usedRemove (item);
            if (item.transform.parent != transform) item.transform.parent = transform;
            item.deactivate ();
            freeAdd (item);
            updateDebug ();
        }

        void usedRemove(PoolItem item) { if (!disabled) used.Remove (item); }
        void usedAdd(PoolItem item) { if (!disabled) used.Add (item); }
        void freeRemove(PoolItem item) { if (!disabled) free.Remove (item); }
        void freeAdd(PoolItem item) { if (!disabled) free.Add (item); }
        PoolItem freePop() => free.FirstOrDefault ();
    }
}