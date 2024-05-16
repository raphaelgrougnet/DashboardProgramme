﻿using Application.Common;

namespace Application.Extensions;

public static class ExceptionExtensions
{
    public static Error ErrorObject(this Exception exception)
    {
        return new Error(exception.GetType().Name, exception.Message);
    }
}