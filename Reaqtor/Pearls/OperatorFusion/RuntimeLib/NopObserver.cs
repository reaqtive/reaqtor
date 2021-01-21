// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Trivial no-op observer.
//
// BD - October 2014
//

using System;

namespace RuntimeLib
{
    public class NopObserver<T> : IObserver<T>
    {
        public static readonly IObserver<T> Instance = new NopObserver<T>();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
        }
    }
}
