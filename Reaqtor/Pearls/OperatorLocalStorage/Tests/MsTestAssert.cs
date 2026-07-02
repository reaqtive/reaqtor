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
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.ReifiedOperations;

namespace Tests
{
    internal sealed class MsTestAssert : IAssert
    {
        public static readonly MsTestAssert Instance = new();

        private MsTestAssert() { }

        public void AreEqual<T>(T expected, T actual, string message, params object[] arguments) => Assert.AreEqual(expected, actual, message, arguments);

        public void AreNotEqual<T>(T expected, T actual, string message, params object[] arguments) => Assert.AreNotEqual(expected, actual, message, arguments);

        public void AreNotSame<T>(T expected, T actual, string message, params object[] arguments) where T : class => Assert.AreNotSame(expected, actual, message, arguments);

        public void AreNotSequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message, params object[] arguments) => Assert.IsFalse(expected.SequenceEqual(actual), message, arguments);

        public void AreSame<T>(T expected, T actual, string message, params object[] arguments) where T : class => Assert.AreSame(expected, actual, message, arguments);

        public void AreSequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message, params object[] arguments) => Assert.IsTrue(expected.SequenceEqual(actual), message, arguments);

        public void IsFalse(bool condition, string message, params object[] arguments) => Assert.IsFalse(condition, message, arguments);

        public void IsTrue(bool condition, string message, params object[] arguments) => Assert.IsTrue(condition, message, arguments);

        public void ThrowsException<TException>(Action action, string message, params object[] arguments) where TException : Exception => Assert.ThrowsException<TException>(action, message, arguments);
    }
}
