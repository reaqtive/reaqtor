// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reliable;
using Reaqtor.TestingFramework;

namespace Reaqtor.QueryEngine
{
    internal class TestReliableExecutionEnvironment : TestExecutionEnvironment, IReliableExecutionEnvironment
    {
        public IReliableMultiSubject<TInput, TOutput> GetReliableSubject<TInput, TOutput>(Uri uri)
        {
            return (IReliableMultiSubject<TInput, TOutput>)GetArtifact(uri);
        }
    }
}
