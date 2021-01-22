// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;

namespace Tests.ReifiedOperations
{
    public interface IAssert
    {
        void AreEqual<T>(T expected, T actual, string message, params object[] arguments);
        void AreNotEqual<T>(T expected, T actual, string message, params object[] arguments);
        void AreSame<T>(T expected, T actual, string message, params object[] arguments) where T : class;
        void AreNotSame<T>(T expected, T actual, string message, params object[] arguments) where T : class;
        void AreSequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message, params object[] arguments);
        void AreNotSequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message, params object[] arguments);
        void IsFalse(bool condition, string message, params object[] arguments);
        void IsTrue(bool condition, string message, params object[] arguments);
        void ThrowsException<TException>(Action action, string message, params object[] arguments) where TException : Exception;
    }
}
