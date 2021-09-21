// Copyright (MIT LICENSE) 2021 HAWKSBILL (https://www.hawksbill.com). created 30/08/2021 14:18:18 by seantcooper
using System;

namespace Hawksbill
{
    [AttributeUsage (AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ExtensionExcludeAttribute : Attribute
    {
        public bool useBaseClass;
        public ExtensionExcludeAttribute(bool useBaseClass = false)
        {
            this.useBaseClass = useBaseClass;
        }
    }
}