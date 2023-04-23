using System;
using System.Linq;
using System.Linq.Expressions;

namespace CRUDConsoleApp
{
    public static class LinqExtensions
    {
        public enum Order
        {
            Ascending,
            Descending
        }

        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string propertyName, Order direction)
        {
            var lambda = CreateLambda<T>(propertyName);
            return direction == Order.Ascending ? query.OrderBy(lambda) : query.OrderByDescending(lambda);
        }

        private static Expression<Func<T, object>> CreateLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.PropertyOrField(parameter, propertyName);
            var conversion = Expression.Convert(member, typeof(object));
            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }
    }
}
