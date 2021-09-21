// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using System;

public static class TypeEx
{
    public static bool IsGenericType(this Type type, Type compare) =>
        type.IsGenericType && type.GetGenericTypeDefinition () == compare;
}