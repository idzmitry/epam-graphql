// Copyright © 2020 EPAM Systems, Inc. All Rights Reserved. All information contained herein is, and remains the
// property of EPAM Systems, Inc. and/or its suppliers and is protected by international intellectual
// property law. Dissemination of this information or reproduction of this material is strictly forbidden,
// unless prior written permission is obtained from EPAM Systems, Inc

using System;

#nullable enable

namespace Epam.GraphQL.EntityFrameworkCore
{
    public static class ResolveOptionsBuilderExtensions
    {
        public static ResolveOptionsBuilder DoNotSaveChanges(this ResolveOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            var extension = optionsBuilder.Options.FindExtension<EFCoreResolveOptionsExtension>();

            extension = extension != null
                ? new EFCoreResolveOptionsExtension(extension)
                : new EFCoreResolveOptionsExtension();

            extension.DoNotSaveChanges = true;

            optionsBuilder.AddOrUpdateExtension(extension);

            return optionsBuilder;
        }

        public static ResolveOptionsBuilder DoNotAddNewEntitiesToDbContext(this ResolveOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            var extension = optionsBuilder.Options.FindExtension<EFCoreResolveOptionsExtension>();

            extension = extension != null
                ? new EFCoreResolveOptionsExtension(extension)
                : new EFCoreResolveOptionsExtension();

            extension.DoNotAddNewEntitiesToDbContext = true;

            optionsBuilder.AddOrUpdateExtension(extension);

            return optionsBuilder;
        }
    }
}
