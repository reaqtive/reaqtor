// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Reaqtive.TestingFramework.TestRunner;

internal class TestRunner
{
    private readonly object _instance;

    public TestRunner(object instance, MethodInfo method)
    {
        _instance = instance;
        TestMethod = method;
    }

    public MethodInfo TestMethod { get; private set; }

    public void Run()
    {
        //
        // NB: Support for [ExpectedException] was dropped when moving to MSTest v4, which
        //     removed ExpectedExceptionAttribute; tests assert exceptions with
        //     Assert.ThrowsExactly<T> inside the test method instead.
        //
        try
        {
            if (TestMethod.ReturnType == typeof(Task))
            {
                try
                {
                    ((Task)TestMethod.Invoke(_instance, [])).Wait();
                }
                catch (AggregateException ex)
                {
                    throw ex.InnerException;
                }
            }
            else
            {
                TestMethod.Invoke(_instance, []);
            }
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }
}
