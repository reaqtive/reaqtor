// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;

using Reaqtive;

using Reaqtor;

namespace Playground
{
#pragma warning disable IDE0060 // Remove unused parameter (used in expression trees)

    /// <summary>
    /// Helper methods to make definition of artifacts more fluent.
    /// </summary>
    internal static class ArtifactDefinitionHelpers
    {
        //
        // NB: None of these have an implementation body; they're simply used within expression trees for artifact definitions, for CastVisitor to pick up on rx://builtin/id.
        //

        [KnownResource("rx://builtin/id")]
        public static ISubscribable<T> ToSubscribable<T>(this IReactiveQbservable<T> source) => throw new NotImplementedException();

        [KnownResource("rx://builtin/id")]
        public static IReactiveQbservable<T> ToQbservable<T>(this ISubscribable<T> source) => throw new NotImplementedException();

        [KnownResource("rx://builtin/id")]
        public static IReactiveQbserver<T> ToQbserver<T>(this IObserver<T> observer) => throw new NotImplementedException();
    }

#pragma warning restore IDE0060 // Remove unused parameter
}
