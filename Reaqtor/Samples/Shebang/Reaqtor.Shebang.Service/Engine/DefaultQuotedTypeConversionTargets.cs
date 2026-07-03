// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;

using Reaqtive;

using Reaqtor.Expressions.Core;
using Reaqtor.QueryEngine;
using Reaqtor.Reliable;

namespace Reaqtor.Shebang.Service
{
    public sealed class DefaultQuotedTypeConversionTargets : IQuotedTypeConversionTargets
    {
#pragma warning disable format // (Formatted as a table.)

        private static readonly FrozenDictionary<Type, Type> s_map = new Dictionary<Type, Type>()
        {
            { typeof(ReactiveQbservable),     typeof(Subscribable)           },
            { typeof(ReactiveQbserver),       typeof(Observer)               },
            { typeof(ReactiveQubjectFactory), typeof(ReliableSubjectFactory) },
        }.ToFrozenDictionary();

#pragma warning restore format

        public IReadOnlyDictionary<Type, Type> TypeMap => s_map;
    }
}
