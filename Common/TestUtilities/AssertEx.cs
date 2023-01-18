// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static class AssertEx
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

        public static void ThrowsException<T>(Action action, Action<T> assert)
            where T : Exception
        {
            var hasThrown = false;

            try
            {
                action();
            }
            catch (T ex)
            {
                if (typeof(T) != ex.GetType())
                    Assert.Fail();

                hasThrown = true;
                assert(ex);
            }

            Assert.IsTrue(hasThrown);
        }

        public static void ThrowsException<T>(Func<object> action, Action<T> assert)
            where T : Exception
        {
            ThrowsException(() => { _ = action(); }, assert);
        }

        public static async Task ThrowsExceptionAsync<T>(Func<Task> action, Action<T> assert)
            where T : Exception
        {
            try
            {
                await action();
            }
            catch (T ex)
            {
                if (typeof(T) != ex.GetType())
                    Assert.Fail();

                assert(ex);
                return;
            }

            Assert.Fail();
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FlakyTestMethodAttribute : TestMethodAttribute
    {
        private readonly int _repeatCount;

        public FlakyTestMethodAttribute(int repeatCount) => _repeatCount = repeatCount;

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var res = new List<TestResult>();

            for (int i = 0; i < _repeatCount; i++)
            {
                foreach (var x in base.Execute(testMethod))
                {
                    x.DisplayName = testMethod.TestMethodName + " - Iteration " + (i + 1);
                    res.Add(x);
                }
            }

            return res.ToArray();
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
