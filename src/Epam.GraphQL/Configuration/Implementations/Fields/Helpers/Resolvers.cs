// Copyright © 2020 EPAM Systems, Inc. All Rights Reserved. All information contained herein is, and remains the
// property of EPAM Systems, Inc. and/or its suppliers and is protected by international intellectual
// property law. Dissemination of this information or reproduction of this material is strictly forbidden,
// unless prior written permission is obtained from EPAM Systems, Inc

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Epam.GraphQL.Extensions;
using Epam.GraphQL.Helpers;
using Epam.GraphQL.Relay;
using GraphQL;

namespace Epam.GraphQL.Configuration.Implementations.Fields.Helpers
{
    internal static class Resolvers
    {
        public static Connection<Proxy<TReturnType>> Resolve<TReturnType>(IResolveFieldContext context, IQueryable<Proxy<TReturnType>> children)
        {
            var first = context.GetFirst();
            var last = context.GetLast();
            var after = context.GetAfter();
            var before = context.GetBefore();

            var shouldComputeCount = context.HasTotalCount();
            var shouldComputeEndOffset = context.HasEndCursor();
            var shouldComputeEdges = context.HasEdges();
            var shouldComputeItems = context.HasItems();

            var connection = ConnectionUtils.ToConnection(
                children,
                () => context.GetPath(),
                context.GetQueryExecuter(),
                first,
                last,
                before,
                after,
                shouldComputeCount,
                shouldComputeEndOffset,
                shouldComputeEdges,
                shouldComputeItems);

            return connection;
        }

        public static Func<IResolveFieldContext, IQueryable<Proxy<TChildEntity>>, Connection<object>> ToGroupConnection<TChildEntity, TExecutionContext>()
        {
            return (context, children) =>
            {
                var subFields = context.GetGroupConnectionQueriedFields();
                var aggregateQueriedFields = context.GetGroupConnectionAggregateQueriedFields();

                var sourceType = children.ElementType;

                var first = context.GetFirst();
                var last = context.GetLast();
                var after = context.GetAfter();
                var before = context.GetBefore();

                var shouldComputeCount = context.HasTotalCount();
                var shouldComputeEndOffset = context.HasEndCursor();
                var shouldComputeEdges = context.HasEdges();
                var shouldComputeItems = context.HasItems();

                IQueryable<object> items;
                if (aggregateQueriedFields.Contains("count"))
                {
                    var param = Expression.Parameter(sourceType);
                    var member = Expression.Property(param, sourceType.GetProperty("$count", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase));
                    var result = Expression.Lambda(member, param);

                    var lambda = ExpressionHelpers.MakeMemberInit<GroupResult<Proxy<TChildEntity>>>(sourceType)
                        .Property(result => result.Item, ExpressionHelpers.MakeIdentity(sourceType))
                        .Property(result => result.Count, result)
                        .Lambda();

                    // TODO Get rid of cast
                    items = (IQueryable<object>)children.SafeNull().AsQueryable().ApplySelect(lambda);
                }
                else
                {
                    items = children.SafeNull().AsQueryable().Select(entity => new GroupResult<Proxy<TChildEntity>>
                    {
                        Item = entity,
                    });
                }

                return ConnectionUtils.ToConnection(
                    items,
                    () => context.GetPath(),
                    context.GetQueryExecuter(),
                    first,
                    last,
                    before,
                    after,
                    shouldComputeCount,
                    shouldComputeEndOffset,
                    shouldComputeEdges,
                    shouldComputeItems);
            };
        }

        public static Func<IResolveFieldContext, TReturnType> ConvertFieldResolver<TEntity, TReturnType, TExecutionContext>(Func<TExecutionContext, TEntity, TReturnType> func)
            where TEntity : class
        {
            return ctx => func(ctx.GetUserContext<TExecutionContext>(), ctx.Source is Proxy<TEntity> proxy ? proxy.GetOriginal() : (TEntity)ctx.Source);
        }

        public static Func<IResolveFieldContext, TReturnType> ConvertFieldResolver<TReturnType, TExecutionContext>(Func<TExecutionContext, TReturnType> func)
        {
            return ctx => func(ctx.GetUserContext<TExecutionContext>());
        }
    }
}
