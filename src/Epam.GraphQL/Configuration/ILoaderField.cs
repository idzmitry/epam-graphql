// Copyright © 2020 EPAM Systems, Inc. All Rights Reserved. All information contained herein is, and remains the
// property of EPAM Systems, Inc. and/or its suppliers and is protected by international intellectual
// property law. Dissemination of this information or reproduction of this material is strictly forbidden,
// unless prior written permission is obtained from EPAM Systems, Inc

using Epam.GraphQL.Helpers;

namespace Epam.GraphQL.Configuration
{
    public interface ILoaderField<TParentEntity, TEntity, TExecutionContext> :
        IInlineLoaderField<ILoaderField<TParentEntity, TEntity, TExecutionContext>, TParentEntity, TEntity, TExecutionContext>,
        IConnectableField<IVoid, TEntity>,
        IConnectableField<IConnectionField>
    {
    }
}
