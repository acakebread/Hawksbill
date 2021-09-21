// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Hawksbill.Serialization
{
    public interface ISurrogate : ISerializationSurrogate
    {
        void AddToSelector(SurrogateSelector selector);
    }
}
