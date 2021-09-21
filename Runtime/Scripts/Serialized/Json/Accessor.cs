// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DictStrObj = System.Collections.Generic.Dictionary<string, object>;
using System.Collections;

namespace Hawksbill.Serialization.Json
{
    public sealed class Accessor // : IEnumerable<object>
    {
        object data;

        public static Accessor Parse(string json) => new Accessor { data = JsonUtils.FromJson (json) };

        public bool isPrimitive => !isArray && !isDict;
        public bool isArray => data.GetType () == typeof (object[]);
        public bool isDict => data.GetType ().IsGenericType (typeof (Dictionary<,>));

        public object[] asArray => (data as object[]);
        public DictStrObj asDict => (data as DictStrObj);
        public DictStrObj[] asDictArray => (isArray ? this.asArray : new object[] { asDict }).Select (o => (o as DictStrObj)).ToArray ();

        /// <summary>Index value as an Array</summary>
        public Accessor this[int index]
        {
            get
            {
                if (isArray) return new Accessor { data = (data as object[])[index] };
                throw new Exception ("Accessor data type is not object[]!");
            }
        }

        /// <summary>Lookup value as Dictionary<,></summary>
        public Accessor this[string key]
        {
            get
            {
                if (isDict) return new Accessor { data = (data as DictStrObj)[key] };
                throw new Exception ("Accessor data type is not Dictionary<,>!");
            }
        }

        // public static implicit operator object[](Accessor a) => a.data as object[];
        // public static implicit operator DictStrObj(Accessor a) => a.data as DictStrObj;
        // public static implicit operator DictStrObj[](Accessor a) => (a.data as object[]).Cast<DictStrObj> ().ToArray ();
        //.Select (o => (o as DictStrObj)).ToArray ();
        //(a.isArray ? (a.data as object[]) : new object[] { a.data as DictStrObj }).Select (o => (o as DictStrObj)).ToArray ();


        // public Accessor find(string path)
        // {
        //     if (String.IsNullOrEmpty (path)) return this;
        //     Accessor accessor = this;
        //     foreach (var field in path.Split ('.'))
        //     {
        //         if (accessor.exists (field)) accessor = accessor[field];
        //         else return null;
        //     }
        //     return accessor;
        // }

        // public enum Type
        // {
        //     Primitive = 0,
        //     Array = 1,
        //     Dict = 2,
        //     DictArray = 3,
        // }


        // public static explicit operator Accessor(string s) => new Accessor (s);

        //SerializedObject Object;
    }
}


// public object[] asArray => (data as object[]);
// public Dictionary<string, object> asDict => (data as Dictionary<string, object>);
// public Dictionary<string, object>[] asDictArray => (isArray ? this.asArray : new object[] { asDict }).Select (o => (o as Dictionary<string, object>)).ToArray ();
// public Accessor[] asAccessors => asArray.Select (o => new Accessor (o)).ToArray ();
// public object asValue => data;
// public T[] array<T>() => asArray.Select (o => (T) o).ToArray ();
// public T get<T>() => (T) data;
// public int count => isArray ? asArray.Length : asDict.Count;

// public Type type
// {
//     get
//     {
//         var type = data.GetType ();
//         if (type == typeof (object[]))
//         {
//             var item = (data as object[]).FirstOrDefault ();
//             if(item)
//             if ((object[]))
//                 return Type.Array;
//         }
//         if (type.IsGenericType (typeof (Dictionary<,>))) return Type.Dict;
//         return Type.Primitive;
//     }
// }
