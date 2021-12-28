// Copyright © 2020 EPAM Systems, Inc. All Rights Reserved. All information contained herein is, and remains the
// property of EPAM Systems, Inc. and/or its suppliers and is protected by international intellectual
// property law. Dissemination of this information or reproduction of this material is strictly forbidden,
// unless prior written permission is obtained from EPAM Systems, Inc

using System;
using System.Linq;
using System.Linq.Expressions;
using Epam.GraphQL.Configuration.Implementations.FieldResolvers;
using Epam.GraphQL.Extensions;
using Epam.GraphQL.Search;
using GraphQL;

#nullable enable

namespace Epam.GraphQL.Configuration.Implementations.Fields.ChildFields
{
    internal class QueryableField<TEntity, TReturnType, TExecutionContext> : QueryableFieldBase<TEntity, TReturnType, TExecutionContext>
        where TEntity : class
    {
        public QueryableField(
            RelationRegistry<TExecutionContext> registry,
            BaseObjectGraphTypeConfigurator<TEntity, TExecutionContext> parent,
            string name,
            Func<TExecutionContext, IQueryable<TReturnType>> query,
            Expression<Func<TEntity, TReturnType, bool>>? condition,
            IGraphTypeDescriptor<TReturnType, TExecutionContext> elementGraphType,
            ISearcher<TReturnType, TExecutionContext>? searcher,
            Func<IQueryable<TReturnType>, IOrderedQueryable<TReturnType>>? orderBy,
            Func<IOrderedQueryable<TReturnType>, IOrderedQueryable<TReturnType>>? thenBy)
            : this(
                  registry,
                  parent,
                  name,
                  CreateResolver(
                      parent,
                      name,
                      ctx => query(ctx.GetUserContext<TExecutionContext>()),
                      condition,
                      elementGraphType,
                      elementGraphType.Configurator ?? throw new NotSupportedException(),
                      searcher,
                      orderBy,
                      thenBy),
                  elementGraphType,
                  elementGraphType.Configurator,
                  arguments: null,
                  searcher,
                  orderBy,
                  thenBy)
        {
        }

        public QueryableField(
            RelationRegistry<TExecutionContext> registry,
            BaseObjectGraphTypeConfigurator<TEntity, TExecutionContext> parent,
            string name,
            IQueryableResolver<TEntity, TReturnType, TExecutionContext> resolver,
            IGraphTypeDescriptor<TReturnType, TExecutionContext> elementGraphType,
            LazyQueryArguments? arguments,
            ISearcher<TReturnType, TExecutionContext>? searcher,
            Func<IQueryable<TReturnType>, IOrderedQueryable<TReturnType>>? orderBy,
            Func<IOrderedQueryable<TReturnType>, IOrderedQueryable<TReturnType>>? thenBy)
            : base(
                  registry,
                  parent,
                  name,
                  resolver,
                  elementGraphType,
                  elementGraphType.Configurator,
                  arguments,
                  searcher,
                  orderBy,
                  thenBy)
        {
        }

        public QueryableField(
            RelationRegistry<TExecutionContext> registry,
            BaseObjectGraphTypeConfigurator<TEntity, TExecutionContext> parent,
            string name,
            IQueryableResolver<TEntity, TReturnType, TExecutionContext> resolver,
            IGraphTypeDescriptor<TReturnType, TExecutionContext> elementGraphType,
            IObjectGraphTypeConfigurator<TReturnType, TExecutionContext>? configurator,
            LazyQueryArguments? arguments,
            ISearcher<TReturnType, TExecutionContext>? searcher,
            Func<IQueryable<TReturnType>, IOrderedQueryable<TReturnType>>? orderBy,
            Func<IOrderedQueryable<TReturnType>, IOrderedQueryable<TReturnType>>? thenBy)
            : base(
                  registry,
                  parent,
                  name,
                  resolver,
                  elementGraphType,
                  configurator,
                  arguments,
                  searcher,
                  orderBy,
                  thenBy)
        {
        }

        public override QueryableFieldBase<TEntity, TReturnType, TExecutionContext> ApplyConnection(Expression<Func<IQueryable<TReturnType>, IOrderedQueryable<TReturnType>>> order)
        {
            var connectionField = new ConnectionQueryableField<TEntity, TReturnType, TExecutionContext>(
                Registry,
                Parent,
                Name,
                QueryableFieldResolver,
                ElementGraphType,
                ObjectGraphTypeConfigurator ?? throw new NotSupportedException(),
                Arguments,
                Searcher,
                order.Compile(),
                order.GetThenBy().Compile());
            return ApplyField(connectionField);
        }

        protected override EnumerableFieldBase<TEntity, TReturnType1, TExecutionContext> CreateSelect<TReturnType1>(Expression<Func<TReturnType, TReturnType1>> selector, IGraphTypeDescriptor<TReturnType1, TExecutionContext> graphType)
        {
            var queryableField = new QueryableField<TEntity, TReturnType1, TExecutionContext>(
                Registry,
                Parent,
                Name,
                QueryableFieldResolver.Select(selector, graphType.Configurator?.ProxyAccessor),
                graphType,
                Arguments,
                searcher: null,
                orderBy: null,
                thenBy: null);

            return queryableField;
        }

        protected override QueryableFieldBase<TEntity, TReturnType, TExecutionContext> ReplaceResolver(IQueryableResolver<TEntity, TReturnType, TExecutionContext> resolver)
        {
            return new QueryableField<TEntity, TReturnType, TExecutionContext>(
                Registry,
                Parent,
                Name,
                resolver,
                ElementGraphType,
                ObjectGraphTypeConfigurator,
                Arguments,
                Searcher,
                OrderBy,
                ThenBy);
        }

        private static IQueryableResolver<TEntity, TReturnType, TExecutionContext> CreateResolver(
            BaseObjectGraphTypeConfigurator<TEntity, TExecutionContext> parent,
            string name,
            Func<IResolveFieldContext, IQueryable<TReturnType>> query,
            Expression<Func<TEntity, TReturnType, bool>>? condition,
            IGraphTypeDescriptor<TReturnType, TExecutionContext> elementGraphType,
            IObjectGraphTypeConfigurator<TReturnType, TExecutionContext> configurator,
            ISearcher<TReturnType, TExecutionContext>? searcher,
            Func<IQueryable<TReturnType>, IOrderedQueryable<TReturnType>>? orderBy,
            Func<IOrderedQueryable<TReturnType>, IOrderedQueryable<TReturnType>>? thenBy)
        {
            var func = GetQuery(configurator, query);

            if (condition != null)
            {
                return new QueryableAsyncFuncResolver<TEntity, TReturnType, TExecutionContext>(
                    name,
                    func,
                    condition,
                    (ctx, items) => items,
                    ApplySort(configurator.Sorters, searcher, orderBy, thenBy),
                    parent.ProxyAccessor,
                    configurator.ProxyAccessor);
            }

            return new QueryableFuncResolver<TEntity, TReturnType, TExecutionContext>(
                elementGraphType.Configurator?.ProxyAccessor,
                func,
                (ctx, items) => items,
                ApplySort(configurator.Sorters, searcher, orderBy, thenBy));
        }
    }
}
