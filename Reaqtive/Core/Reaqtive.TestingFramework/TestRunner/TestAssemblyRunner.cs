// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtive.TestingFramework.TestRunner
{
    public sealed class TestAssemblyRunner : MarshalByRefObject, IDisposable
    {
        private List<TestClassRunner> _testClasses;

        public int TotalFailures => _testClasses.Sum(c => c.TotalFailures);

        public int TotalRuns => _testClasses.Sum(c => c.TotalRuns);

        public void Configure(string assemblyName)
        {
            _testClasses?.ForEach(r => r.Dispose());

            _testClasses = CreateTestClassRunners(Assembly.LoadFrom(assemblyName));
        }

        public void Run(int repeat)
        {
            var sw = Stopwatch.StartNew();
            _testClasses.ForEach(r => r.Run(repeat));
            Report(sw.Elapsed);
        }

        public void Dispose()
        {
            _testClasses.ForEach(r => r.Dispose());
        }

#if NET6_0
        [Obsolete("This Remoting API is not supported and throws PlatformNotSupportedException.")]
#endif
        public override object InitializeLifetimeService() => null;

        private void Report(TimeSpan elapsed)
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = TotalFailures > 0
                ? ConsoleColor.Red
                : ConsoleColor.Green;

            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Total: {0} Passed: {1} Failed: {2} in {3} ms", TotalRuns, TotalRuns - TotalFailures, TotalFailures, elapsed.TotalMilliseconds));

            Console.ForegroundColor = originalColor;
        }

        private static List<TestClassRunner> CreateTestClassRunners(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsDefined(typeof(TestClassAttribute))).Select(t => new TestClassRunner(t)).ToList();
        }
    }
}
