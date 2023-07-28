// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtive.TestingFramework.TestRunner
{
    internal class TestClassRunner : IDisposable
    {
        private readonly Type _type;
        private int _assemblyInitialized = 0;

        public TestClassRunner(Type type)
        {
            _type = type;
            InitializeAssembly();
        }

        public int TotalFailures { get; private set; } = 0;

        public int TotalRuns { get; private set; } = 0;

        public void Run(int repeat)
        {
            var instance = Activator.CreateInstance(_type);
            var testMethods = _type.GetMethods().Where(m => m.IsDefined(typeof(TestMethodAttribute)));
            InitializeClass();
            foreach (var testMethod in testMethods)
            {
                if (testMethod.IsDefined(typeof(IgnoreAttribute)))
                {
                    Ignore(testMethod);
                }
                else
                {
                    RepeatTest(instance, testMethod, repeat);
                }
            }
            CleanupClass();
        }

        private void InitializeAssembly()
        {
            if (Interlocked.Exchange(ref _assemblyInitialized, 1) == 0)
            {
                var assemblyInitialize = _type.GetMethods()
                    .SingleOrDefault(m => m.IsDefined(typeof(AssemblyInitializeAttribute)));
                // TODO: revisit a mocked TestContext
                assemblyInitialize?.Invoke(null, new object[] { null });
            }
        }

        private void CleanupAssembly()
        {
            var assemblyCleanup = _type.GetMethods()
                .SingleOrDefault(m => m.IsDefined(typeof(AssemblyCleanupAttribute)));
            // TODO: revisit a mocked TestContext
            assemblyCleanup?.Invoke(null, new object[] { null });
        }

        private void InitializeClass()
        {
            var classInitializer = _type.GetMethods()
                .SingleOrDefault(m => m.IsDefined(typeof(ClassInitializeAttribute)));
            // TODO: revisit a mocked TestContext
            classInitializer?.Invoke(null, new object[] { null });
        }

        private void CleanupClass()
        {
            var classCleanup = _type.GetMethods()
                .SingleOrDefault(m => m.IsDefined(typeof(ClassCleanupAttribute)));
            // TODO: revisit a mocked TestContext
            classCleanup?.Invoke(null, Array.Empty<object>());
        }

        private void InitializeTest(object instance)
        {
            var testInitializer = _type.GetMethods()
                .SingleOrDefault(m => m.IsDefined(typeof(TestInitializeAttribute)));
            testInitializer?.Invoke(instance, Array.Empty<object>());
        }

        private void CleanupTest(object instance)
        {
            var testCleanup = _type.GetMethods()
                .SingleOrDefault(m => m.IsDefined(typeof(TestCleanupAttribute)));
            testCleanup?.Invoke(instance, Array.Empty<object>());
        }

        private void RepeatTest(object instance, MethodInfo method, int repeat)
        {
            var test = new TestRunner(instance, method);
            var stopwatch = new Stopwatch();
            var failureCount = 0;
            for (var i = 0; i < repeat; ++i)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (By design for test runner.)

                try
                {
                    stopwatch.Start();
                    InitializeTest(instance);
                    try
                    {
                        test.Run();
                        TotalRuns++;
                    }
                    finally
                    {
                        CleanupTest(instance);
                    }
                    stopwatch.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    failureCount++;
                    TotalFailures++;
                }

#pragma warning restore CA1031
#pragma warning restore IDE0079
            }
            Report(method, failureCount, repeat, stopwatch.ElapsedMilliseconds);
        }

        private static void Report(MethodInfo method, int fails, int repeat, long elapsed)
        {
            var color = Console.ForegroundColor;
            string msg;
            if (fails > 0)
            {
                msg = string.Format(CultureInfo.InvariantCulture, "Failed ({1}/{2}) {0} [avg. {3}ms]", method.Name, repeat - fails, repeat, Math.Ceiling(((double)elapsed) / repeat));
                color = ConsoleColor.Red;
            }
            else
            {
                msg = string.Format(CultureInfo.InvariantCulture, "Passed {0} [avg. {1}ms]", method.Name, Math.Ceiling(((double)elapsed) / repeat));
            }
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = prev;
        }

        private static void Ignore(MethodInfo method)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Ignore {0}", method.Name));
            Console.ForegroundColor = prev;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _assemblyInitialized, 0) == 1)
            {
                CleanupAssembly();
            }
        }
    }
}
