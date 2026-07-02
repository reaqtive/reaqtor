// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static partial class AssertEx
    {
        public static void AreSequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var expList = expected.ToList();
            var actList = actual.ToList();

            if (expList.Count != actList.Count)
            {
                Assert.Fail("Sequences do not have equal length." + Environment.NewLine + GetValues());
            }

            if (!expList.SequenceEqual(actList))
            {
                Assert.Fail("Sequences are not equal." + Environment.NewLine + GetValues());
            }

            string GetValues()
            {
                var exp = string.Join(", ", expList);
                var act = string.Join(", ", actList);

                return $"Expected = {{{exp}}}{Environment.NewLine}  Actual = {{{act}}}";
            }
        }

    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FlakyTestMethodAttribute : TestMethodAttribute
    {
        private readonly int _repeatCount;

        public FlakyTestMethodAttribute(int repeatCount, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
            : base(callerFilePath, callerLineNumber) => _repeatCount = repeatCount;

        public override async Task<TestResult[]> ExecuteAsync(ITestMethod testMethod)
        {
            var res = new List<TestResult>();

            for (int i = 0; i < _repeatCount; i++)
            {
                foreach (var x in await base.ExecuteAsync(testMethod).ConfigureAwait(false))
                {
                    x.DisplayName = testMethod.TestMethodName + " - Iteration " + (i + 1);
                    res.Add(x);
                }
            }

            return [.. res];
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class FlakyTestClassAttribute : TestClassAttribute
    {
        private readonly int _repeatCount;

        public FlakyTestClassAttribute(int repeatCount) => _repeatCount = repeatCount;

        public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
        {
            return new FlakyTestMethodAttribute(_repeatCount);
        }
    }
}
