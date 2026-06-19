// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        private class SimpleTunnel<T> : NotSoSimpleSubject<T>
        {
            private readonly IObserver<bool> _refCount;

            public SimpleTunnel(TestExecutionEnvironment parent, Uri streamUri, IObserver<bool> refCount)
                : base(parent, streamUri)
            {
                _refCount = refCount;
            }

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
