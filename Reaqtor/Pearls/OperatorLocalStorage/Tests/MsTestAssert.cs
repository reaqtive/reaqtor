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
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.ReifiedOperations;

namespace Tests;

internal sealed class MsTestAssert : IAssert
{
    public static readonly MsTestAssert Instance = new();

    private MsTestAssert() { }

    //
    // NB: MSTest v4 removed the (string message, params object[] parameters) assert overloads,
    //     so the format arguments are folded into the message up front.
    //

    public void AreEqual<T>(T expected, T actual, string message, params object[] arguments) => Assert.AreEqual(expected, actual, Format(message, arguments));

    public void AreNotEqual<T>(T expected, T actual, string message, params object[] arguments) => Assert.AreNotEqual(expected, actual, Format(message, arguments));

    public void AreNotSame<T>(T expected, T actual, string message, params object[] arguments) where T : class => Assert.AreNotSame(expected, actual, Format(message, arguments));

    public void AreNotSequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message, params object[] arguments) => Assert.IsFalse(expected.SequenceEqual(actual), Format(message, arguments));

    public void AreSame<T>(T expected, T actual, string message, params object[] arguments) where T : class => Assert.AreSame(expected, actual, Format(message, arguments));

    public void AreSequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message, params object[] arguments) => Assert.IsTrue(expected.SequenceEqual(actual), Format(message, arguments));

    public void IsFalse(bool condition, string message, params object[] arguments) => Assert.IsFalse(condition, Format(message, arguments));

    public void IsTrue(bool condition, string message, params object[] arguments) => Assert.IsTrue(condition, Format(message, arguments));

    public void ThrowsException<TException>(Action action, string message, params object[] arguments) where TException : Exception => Assert.ThrowsExactly<TException>(action, Format(message, arguments));

    private static string Format(string message, object[] arguments) => arguments is { Length: > 0 } ? string.Format(CultureInfo.InvariantCulture, message, arguments) : message;
}
