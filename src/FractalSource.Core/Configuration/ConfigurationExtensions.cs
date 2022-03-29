using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
// ReSharper disable InvertIf
// ReSharper disable MergeIntoPattern

namespace FractalSource.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetKey<T>(this IHasConfigurationName configuration, Expression<Func<T>> expression)
        {
            var sectionName = configuration.Name;
            var keyName = expression.GetMemberName();

            return $"{sectionName}:{keyName}";
        }

        public static string GetKey(this IHasConfigurationName configuration, string keyName)
        {
            var sectionName = configuration.Name;

            return $"{sectionName}:{keyName}";
        }

        public static T GetValue<T>(this IConfiguration target, Expression<Func<T>> expression)
        {
            var configuration = expression.GetMemberInstance() as IHasConfigurationName;
            var valueKey = configuration!.GetKey(expression);

            return target.GetValue<T>(valueKey);
        }

        public static IEnumerable<T> GetEnumerable<T>(this IConfiguration target, Expression<Func<IEnumerable<T>>> expression, string sectionKeyName)
        {
            var configuration = expression.GetMemberInstance() as IHasConfigurationName;
            var sectionKey = configuration!.GetKey(sectionKeyName);

            var section = target.GetSection(sectionKey);
   
            return section.Get<List<T>>();
        }

        public static IEnumerable<T> GetEnumerable<T>(this IConfiguration target, Expression<Func<IEnumerable<T>>> expression)
        {
            var configuration = expression.GetMemberInstance() as IHasConfigurationName;
            var sectionKey = configuration!.GetKey(expression);

            var section = target.GetSection(sectionKey);

            return section.Get<T[]>();
        }

        public static IEnumerable<T> GetEnumerationValue<T>(this IConfiguration target, Expression<Func<IEnumerable<T>>> expression)
        {
            var configuration = expression.GetMemberInstance() as IHasConfigurationName;
            var sectionKey = configuration!.GetKey(expression);

            var section = target.GetSection(sectionKey);
            var values = section.AsEnumerable()
                .Select(pair => pair.Value)
                .Where(value => value != null)
                .Cast<T>()
                .ToList();

            return values;
        }

        private static string GetMemberName<T>(this Expression<Func<T>> lambdaExpression)
        {
            if (lambdaExpression is LambdaExpression lambda)
            {
                if (lambda.Body is MemberExpression member)
                {
                    return member.Member.Name;
                }
            }

            throw new ArgumentException($"Parameter '{nameof(lambdaExpression)}' is not a valid LambdaExpression.");
        }

        private static object GetMemberInstance<T>(this Expression<Func<T>> lambdaExpression)
        {
            if (lambdaExpression is LambdaExpression lambda)
            {
                if (lambda.Body is MemberExpression member)
                {
                    if (member.Expression is ConstantExpression constant)
                    {
                        return constant.Value!;
                    }
                }
            }

            throw new ArgumentException($"Parameter '{nameof(lambdaExpression)}' is not a valid LambdaExpression.");
        }
    }
}