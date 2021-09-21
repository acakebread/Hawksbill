// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 05/09/2021 11:09:35 by seantcooper
using System.Collections.Generic;
using UnityEngine;
using static Hawksbill.Configurator.ConfiguratorObject;
using static Hawksbill.Configurator.ConfiguratorSelectable;

namespace Hawksbill.Configurator
{
    ///<summary>Put text here to describe the Class</summary>
    public static class ConfiguratorManager
    {
        public class History
        {
            List<ConfiguratorSelectable> Items = new List<ConfiguratorSelectable> ();
            public static void Back() { }
            public static void Forward() { }
            public static void Add() { }
        }
    }
}