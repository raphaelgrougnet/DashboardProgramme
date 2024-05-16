﻿using System.ComponentModel;
using System.Reflection;

namespace Application.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo == null)
        {
            return string.Empty;
        }

        DescriptionAttribute? attribute =
            (DescriptionAttribute?)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
        return attribute != null ? attribute.Description : value.ToString();
    }
}