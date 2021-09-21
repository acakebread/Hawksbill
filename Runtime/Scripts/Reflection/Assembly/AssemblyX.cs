// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 18/05/2021 17:50:17 by seantcooper
using UnityEngine;
using Hawksbill;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Hawksbill.Reflection
{
    public static class AssemblyX
    {
        public static IEnumerable<Type> GetAllTypesInheriting(Type type) =>
            AppDomain.CurrentDomain.GetAssemblies ().SelectMany (assembly => assembly.GetTypes ()).
                Where (assemblyType => type.IsAssignableFrom (assemblyType)).
                Select (assemblyType => assemblyType).Except (new Type[] { type }).ToArray ();
    }
}