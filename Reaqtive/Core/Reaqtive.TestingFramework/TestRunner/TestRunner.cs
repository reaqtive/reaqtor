// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtive.TestingFramework.TestRunner
{
    internal class TestRunner
    {
        private readonly object _instance;
        private readonly Lazy<ExpectedExceptionAttribute> _expectedException;

        public TestRunner(object instance, MethodInfo method)
        {
            _instance = instance;
            TestMethod = method;
            _expectedException = new Lazy<ExpectedExceptionAttribute>(
                () => method.GetCustomAttribute<ExpectedExceptionAttribute>());
        }

        public MethodInfo TestMethod { get; private set; }

        public void Run()
        {
            try
            {
                if (TestMethod.ReturnType == typeof(Task))
                {
                    try
                    {
                        ((Task)TestMethod.Invoke(_instance, Array.Empty<object>())).Wait();
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
                else
                {
                    TestMethod.Invoke(_instance, Array.Empty<object>());
                }
            }
            catch (TargetInvocationException ex)
            {
                if (!IsExpected(ex.InnerException))
                {
                    throw ex.InnerException;
                }
            }
            catch (Exception ex)
            {
                if (!IsExpected(ex))
                {
                    throw;
                }
            }
        }

        private bool IsExpected(Exception ex)
        {
            var expected = _expectedException.Value;
            if (expected == null)
            {
                return false;
            }
            else if (expected.AllowDerivedTypes)
            {
                return expected.ExceptionType.IsAssignableFrom(ex.GetType());
            }
            else
            {
                return expected.ExceptionType == ex.GetType();
            }
        }
    }
}
