// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 10/08/2021 14:50:26 by seantcooper
using UnityEngine;
using Hawksbill;
using System.Dynamic;
using System.Collections.Generic;

namespace Hawksbill
{
    public class DynamicDictionary : DynamicObject
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object> ();
        public int Count => dictionary.Count;

        public DynamicDictionary(Dictionary<string, object> dictionary) =>
            this.dictionary = dictionary;

        public override bool TryGetMember(GetMemberBinder binder, out object result) =>
            dictionary.TryGetValue (binder.Name.ToLower (), out result);

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dictionary[binder.Name.ToLower ()] = value;
            return true;
        }

        public static implicit operator DynamicDictionary(Dictionary<string, object> v) => new DynamicDictionary (v);
        public static implicit operator Dictionary<string, object>(DynamicDictionary v) => v.dictionary;

    }
}