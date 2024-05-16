using System.Linq.Expressions;
using System.Reflection;

using Domain.Common;

using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence.Extensions;

public static class SoftDeletableExtensions
{
    public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
    {
        MethodInfo? methodInfo = typeof(SoftDeletableExtensions)
            .GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static);

        if (methodInfo == null)
        {
            throw new Exception($"Could not find the {typeof(SoftDeletableExtensions)} method");
        }

        object? filter = methodInfo.MakeGenericMethod(entityData.ClrType).Invoke(null, []);

        entityData.SetQueryFilter((LambdaExpression?)filter);
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDeletable
    {
        Expression<Func<TEntity, bool>> filter = x => !x.Deleted.HasValue;
        return filter;
    }
}