// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtive;
using Reaqtive.Scheduler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class OperatorContextTests
    {
        [TestMethod]
        public void OperatorContext_ArgumentChecking()
        {
            var ur = new Uri("bing://foo/bar");

            using var ph = PhysicalScheduler.Create();
            using var sh = new LogicalScheduler(ph);

            var tc = new TraceSource("foo");
            var ee = new Environment();

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => new OperatorContext(default(Uri), sh, tc, ee));
            Assert.ThrowsException<ArgumentNullException>(() => new OperatorContext(ur, default(IScheduler), tc, ee));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void OperatorContext_Simple()
        {
            var ur = new Uri("bing://foo/bar");

            using var ph = PhysicalScheduler.Create();
            using var sh = new LogicalScheduler(ph);

            var tc = new TraceSource("foo");
            var ee = new Environment();

            var ctx = new OperatorContext(ur, sh, tc, ee);

            Assert.AreSame(ur, ctx.InstanceId);
            Assert.AreSame(sh, ctx.Scheduler);
            Assert.AreSame(tc, ctx.TraceSource);
            Assert.AreSame(ee, ctx.ExecutionEnvironment);

            Assert.IsFalse(ctx.TryGetElement<string>("foo", out var s) && s == null);
        }

        private sealed class Environment : IExecutionEnvironment
        {
            public IMultiSubject<TInput, TOutput> GetSubject<TInput, TOutput>(Uri uri)
            {
                throw new NotImplementedException();
            }

            public ISubscription GetSubscription(Uri uri)
            {
                throw new NotImplementedException();
            }
        }
    }
}
