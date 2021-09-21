// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 16/07/2021 09:18:01 by seantcooper
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Hawksbill
{
    public static class ObjectActions
    {
        public static IEnumerable<Item> GetItems<T>()
        {
            foreach (var o in Selection.objects)
            {
                var item = new Item (o);
                if (item.type == typeof (T))
                    yield return item;
            }
        }

        public class Item
        {
            public UnityEngine.Object obj;
            public Item(UnityEngine.Object obj) => this.obj = obj;
            public bool isPath => AssetDatabase.IsValidFolder (path);
            public string path => AssetDatabase.GetAssetPath (obj);
            public string folder => isPath ? path : String.Join ("/", path.Split ('/').TakeLessOne ());
            public string name => obj.name;
            public Type type => AssetDatabase.GetMainAssetTypeAtPath (path);
        }
    }
}