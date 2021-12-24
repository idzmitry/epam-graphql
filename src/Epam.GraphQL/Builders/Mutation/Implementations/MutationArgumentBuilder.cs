// Copyright © 2020 EPAM Systems, Inc. All Rights Reserved. All information contained herein is, and remains the
// property of EPAM Systems, Inc. and/or its suppliers and is protected by international intellectual
// property law. Dissemination of this information or reproduction of this material is strictly forbidden,
// unless prior written permission is obtained from EPAM Systems, Inc

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Epam.GraphQL.Builders.Loader;
using Epam.GraphQL.Configuration.Implementations.Fields.ResolvableFields;
using Epam.GraphQL.Extensions;
using Epam.GraphQL.Loaders;

namespace Epam.GraphQL.Builders.Mutation.Implementations
{
    internal class MutationArgumentBuilder<TEntity, TArgType, TExecutionContext> :
        MutationArgumentBuilderBase<IArgumentedField<TEntity, TArgType, TExecutionContext>, TEntity, TArgType, TExecutionContext>,
        IMutationArgumentBuilder<TArgType, TExecutionContext>
        where TEntity : class
    {
        public MutationArgumentBuilder(IArgumentedField<TEntity, TArgType, TExecutionContext> argumentedField)
            : base(argumentedField)
        {
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType, TExecutionContext>, TArgType, TExecutionContext> AsUnionOf<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType, TExecutionContext>, TArgType, TExecutionContext> AsUnionOf<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType, TExecutionContext>, TArgType, TExecutionContext> And<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AndImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType, TExecutionContext>, TArgType, TExecutionContext> And<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AndImpl<TEnumerable, TElementType>(build);
        }

        public IMutationArgumentBuilder<TArgType, TType, TExecutionContext> Argument<TType>(string name)
        {
            return ArgumentImpl<TType>(name);
        }

        public IMutationArgumentBuilder<TArgType, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgument<TProjection, TEntity1>(string name)
            where TProjection : Projection<TEntity1, TExecutionContext>
            where TEntity1 : class
        {
            return FilterArgumentImpl<TProjection, TEntity1>(name);
        }

        public IMutationArgumentBuilder<TArgType, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgument<TEntity1>(Type projectionType, string name)
            where TEntity1 : class
        {
            var methodInfo = typeof(IMutationArgumentBuilder<TArgType, TExecutionContext>).GetPublicGenericMethod(
                nameof(IMutationArgumentBuilder<TArgType, TExecutionContext>.FilterArgument),
                new[] { projectionType, typeof(TEntity1) },
                new[] { typeof(string) });

            return methodInfo.InvokeAndHoistBaseException<IMutationArgumentBuilder<TArgType, Expression<Func<TEntity1, bool>>, TExecutionContext>>(this, name);
        }

        private MutationArgumentBuilder<TEntity, TArgType, TExecutionContext> AsUnionOfImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            Field = Field.ApplyUnion(build, false);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType, TExecutionContext> AsUnionOfImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            Field = Field.ApplyUnion(build, true);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType, TExecutionContext> AndImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType, TExecutionContext> AndImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType, TType, TExecutionContext> ArgumentImpl<TType>(string name)
        {
            var argumentedField = Field.ApplyArgument<TType>(name);
            return new MutationArgumentBuilder<TEntity, TArgType, TType, TExecutionContext>(argumentedField);
        }

        private MutationArgumentBuilder<TEntity, TArgType, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgumentImpl<TProjection, TEntity1>(string name)
            where TProjection : Projection<TEntity1, TExecutionContext>
            where TEntity1 : class
        {
            var argumentedField = Field.ApplyFilterArgument<TProjection, TEntity1>(name);
            return new MutationArgumentBuilder<TEntity, TArgType, Expression<Func<TEntity1, bool>>, TExecutionContext>(argumentedField);
        }
    }

    internal class MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TExecutionContext> :
        MutationArgumentBuilderBase<IArgumentedField<TEntity, TArgType1, TArgType2, TExecutionContext>, TEntity, TArgType1, TArgType2, TExecutionContext>,
        IMutationArgumentBuilder<TArgType1, TArgType2, TExecutionContext>
        where TEntity : class
    {
        public MutationArgumentBuilder(IArgumentedField<TEntity, TArgType1, TArgType2, TExecutionContext> argumentedField)
            : base(argumentedField)
        {
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TExecutionContext>, TArgType1, TArgType2, TExecutionContext> AsUnionOf<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TExecutionContext>, TArgType1, TArgType2, TExecutionContext> AsUnionOf<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TExecutionContext>, TArgType1, TArgType2, TExecutionContext> And<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AndImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TExecutionContext>, TArgType1, TArgType2, TExecutionContext> And<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AndImpl<TEnumerable, TElementType>(build);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, TType, TExecutionContext> Argument<TType>(string name)
        {
            return ArgumentImpl<TType>(name);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgument<TProjection, TEntity1>(string name)
            where TProjection : Projection<TEntity1, TExecutionContext>
            where TEntity1 : class
        {
            return FilterArgumentImpl<TProjection, TEntity1>(name);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgument<TEntity1>(Type projectionType, string name)
            where TEntity1 : class
        {
            var methodInfo = typeof(IMutationArgumentBuilder<TArgType1, TArgType2, TExecutionContext>).GetPublicGenericMethod(
                nameof(IMutationArgumentBuilder<TArgType1, TArgType2, TExecutionContext>.FilterArgument),
                new[] { projectionType, typeof(TEntity1) },
                new[] { typeof(string) });

            return methodInfo.InvokeAndHoistBaseException<IMutationArgumentBuilder<TArgType1, TArgType2, Expression<Func<TEntity1, bool>>, TExecutionContext>>(this, name);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TExecutionContext> AsUnionOfImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            Field = Field.ApplyUnion(build, false);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TExecutionContext> AsUnionOfImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            Field = Field.ApplyUnion(build, true);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TExecutionContext> AndImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TExecutionContext> AndImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TType, TExecutionContext> ArgumentImpl<TType>(string name)
        {
            var argumentedField = Field.ApplyArgument<TType>(name);
            return new MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TType, TExecutionContext>(argumentedField);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgumentImpl<TProjection, TEntity1>(string name)
            where TProjection : Projection<TEntity1, TExecutionContext>
            where TEntity1 : class
        {
            var argumentedField = Field.ApplyFilterArgument<TProjection, TEntity1>(name);
            return new MutationArgumentBuilder<TEntity, TArgType1, TArgType2, Expression<Func<TEntity1, bool>>, TExecutionContext>(argumentedField);
        }
    }

    internal class MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TExecutionContext> :
        MutationArgumentBuilderBase<IArgumentedField<TEntity, TArgType1, TArgType2, TArgType3, TExecutionContext>, TEntity, TArgType1, TArgType2, TArgType3, TExecutionContext>,
        IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TExecutionContext>
        where TEntity : class
    {
        public MutationArgumentBuilder(IArgumentedField<TEntity, TArgType1, TArgType2, TArgType3, TExecutionContext> argumentedField)
            : base(argumentedField)
        {
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TExecutionContext>, TArgType1, TArgType2, TArgType3, TExecutionContext> AsUnionOf<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TExecutionContext>, TArgType1, TArgType2, TArgType3, TExecutionContext> AsUnionOf<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TExecutionContext>, TArgType1, TArgType2, TArgType3, TExecutionContext> And<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AndImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TExecutionContext>, TArgType1, TArgType2, TArgType3, TExecutionContext> And<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AndImpl<TEnumerable, TElementType>(build);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TType, TExecutionContext> Argument<TType>(string name)
        {
            return ArgumentImpl<TType>(name);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgument<TProjection, TEntity1>(string name)
            where TProjection : Projection<TEntity1, TExecutionContext>
            where TEntity1 : class
        {
            return FilterArgumentImpl<TProjection, TEntity1>(name);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgument<TEntity1>(Type projectionType, string name)
            where TEntity1 : class
        {
            var methodInfo = typeof(IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TExecutionContext>).GetPublicGenericMethod(
                nameof(IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TExecutionContext>.FilterArgument),
                new[] { projectionType, typeof(TEntity1) },
                new[] { typeof(string) });

            return methodInfo.InvokeAndHoistBaseException<IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, Expression<Func<TEntity1, bool>>, TExecutionContext>>(this, name);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TExecutionContext> AsUnionOfImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            Field = Field.ApplyUnion(build, false);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TExecutionContext> AsUnionOfImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            Field = Field.ApplyUnion(build, true);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TExecutionContext> AndImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TExecutionContext> AndImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TType, TExecutionContext> ArgumentImpl<TType>(string name)
        {
            var argumentedField = Field.ApplyArgument<TType>(name);
            return new MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TType, TExecutionContext>(argumentedField);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgumentImpl<TProjection, TEntity1>(string name)
            where TProjection : Projection<TEntity1, TExecutionContext>
            where TEntity1 : class
        {
            var argumentedField = Field.ApplyFilterArgument<TProjection, TEntity1>(name);
            return new MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, Expression<Func<TEntity1, bool>>, TExecutionContext>(argumentedField);
        }
    }

    internal class MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> :
        MutationArgumentBuilderBase<IArgumentedField<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>, TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>,
        IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>
        where TEntity : class
    {
        public MutationArgumentBuilder(IArgumentedField<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> argumentedField)
            : base(argumentedField)
        {
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> AsUnionOf<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> AsUnionOf<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> And<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AndImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> And<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AndImpl<TEnumerable, TElementType>(build);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TArgType4, TType, TExecutionContext> Argument<TType>(string name)
        {
            return ArgumentImpl<TType>(name);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TArgType4, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgument<TProjection, TEntity1>(string name)
            where TProjection : Projection<TEntity1, TExecutionContext>
            where TEntity1 : class
        {
            return FilterArgumentImpl<TProjection, TEntity1>(name);
        }

        public IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TArgType4, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgument<TEntity1>(Type projectionType, string name)
            where TEntity1 : class
        {
            var methodInfo = typeof(IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>).GetPublicGenericMethod(
                nameof(IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext>.FilterArgument),
                new[] { projectionType, typeof(TEntity1) },
                new[] { typeof(string) });

            return methodInfo.InvokeAndHoistBaseException<IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TArgType4, Expression<Func<TEntity1, bool>>, TExecutionContext>>(this, name);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> AsUnionOfImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            Field = Field.ApplyUnion(build, false);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> AsUnionOfImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            Field = Field.ApplyUnion(build, true);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> AndImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TExecutionContext> AndImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TType, TExecutionContext> ArgumentImpl<TType>(string name)
        {
            var argumentedField = Field.ApplyArgument<TType>(name);
            return new MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TType, TExecutionContext>(argumentedField);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, Expression<Func<TEntity1, bool>>, TExecutionContext> FilterArgumentImpl<TProjection, TEntity1>(string name)
            where TProjection : Projection<TEntity1, TExecutionContext>
            where TEntity1 : class
        {
            var argumentedField = Field.ApplyFilterArgument<TProjection, TEntity1>(name);
            return new MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, Expression<Func<TEntity1, bool>>, TExecutionContext>(argumentedField);
        }
    }

    internal class MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> :
        MutationArgumentBuilderBase<IArgumentedField<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext>, TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext>,
        IMutationArgumentBuilder<TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext>
        where TEntity : class
    {
        public MutationArgumentBuilder(IArgumentedField<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> argumentedField)
            : base(argumentedField)
        {
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext>, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> AsUnionOf<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext>, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> AsUnionOf<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext>, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> And<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AndImpl(build);
        }

        public IMutationFieldBuilder<IMutationFieldBuilderBase<TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext>, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> And<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AndImpl<TEnumerable, TElementType>(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> AsUnionOfImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            Field = Field.ApplyUnion(build, false);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> AsUnionOfImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            Field = Field.ApplyUnion(build, true);
            return this;
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> AndImpl<TType>(Action<IInlineObjectBuilder<TType, TExecutionContext>> build)
            where TType : class
        {
            return AsUnionOfImpl(build);
        }

        private MutationArgumentBuilder<TEntity, TArgType1, TArgType2, TArgType3, TArgType4, TArgType5, TExecutionContext> AndImpl<TEnumerable, TElementType>(Action<IInlineObjectBuilder<TElementType, TExecutionContext>> build)
            where TEnumerable : class, IEnumerable<TElementType>
            where TElementType : class
        {
            return AsUnionOfImpl<TEnumerable, TElementType>(build);
        }
    }
}
