// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace DelegatingBinder
{
    internal interface IQProvider
    {
        IService Service { get; } // NOTE: This conflates factories and remoting targets; the real IRP does separate those properly

        IQbservable<T> CreateObservable<T>(Expression expression);
        IQbserver<T> CreateObserver<T>(Expression expression);
        IQubscription CreateSubscription(Expression expression);
        IQubjectFactory<T> CreateQubjectFactory<T>(Expression expression);
    }
}
