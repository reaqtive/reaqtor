// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        private class SimpleTunnel<T>(TestExecutionEnvironment parent, Uri streamUri, IObserver<bool> refCount) : NotSoSimpleSubject<T>(parent, streamUri)
        {
            private readonly IObserver<bool> _refCount = refCount;

            protected override void AddRef()
            {
                base.AddRef();

                _refCount.OnNext(true);
            }

            protected override void Release()
            {
                base.Release();

                _refCount.OnNext(false);
            }
        }
    }
}
