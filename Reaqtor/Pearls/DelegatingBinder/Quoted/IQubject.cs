// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace DelegatingBinder
{
    internal interface IQubject<T> : IQbservable<T>, IQbserver<T>, IQProviderBound
    {
        void Dispose();
    }
}
