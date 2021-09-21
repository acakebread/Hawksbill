// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;
using System.Linq;
using System.Collections.Generic;

namespace Hawksbill.Serialization.Json
{
    public class SurrogateSelector
    {
        public bool has<T>() => has (typeof (T));
        public bool has(Type type) => surrogates.ContainsKey (type);

        public ISurrogate get<T>() => get (typeof (T));
        public ISurrogate get(Type type) => surrogates[type];

        public SurrogateSelector(IList<ISurrogate> surrogates)
        {
            this.surrogates = surrogates.ToDictionary (s => s.type, s => s);
        }
        Dictionary<Type, ISurrogate> surrogates;
    }

    public interface ISurrogate
    {
        Type type { get; }
        object getObjectData(object obj, SurrogateSelector selector);
        object setObjectData(object obj, SurrogateSelector selector);
    }
}