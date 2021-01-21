// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    public partial class LetOptimizerTests
    {
        [TestMethod]
        public void LetCoalescer_Tupletization_Types1()
        {
            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        select (x0.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        select (x0.GetType() + " " + x1.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        let x12 = (ulong)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        let x12 = (ulong)i
                        let x13 = (long)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        let x12 = (ulong)i
                        let x13 = (long)i
                        let x14 = (uint)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        let x12 = (ulong)i
                        let x13 = (long)i
                        let x14 = (uint)i
                        let x15 = (int)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        let x12 = (ulong)i
                        let x13 = (long)i
                        let x14 = (uint)i
                        let x15 = (int)i
                        let x16 = (ushort)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        let x12 = (ulong)i
                        let x13 = (long)i
                        let x14 = (uint)i
                        let x15 = (int)i
                        let x16 = (ushort)i
                        let x17 = (short)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        let x12 = (ulong)i
                        let x13 = (long)i
                        let x14 = (uint)i
                        let x15 = (int)i
                        let x16 = (ushort)i
                        let x17 = (short)i
                        let x18 = (sbyte)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)i
                        let x2 = (sbyte)i
                        let x3 = (short)i
                        let x4 = (ushort)i
                        let x5 = (int)i
                        let x6 = (uint)i
                        let x7 = (long)i
                        let x8 = (ulong)i
                        let x9 = (float)i
                        let x10 = (double)i
                        let x11 = (float)i
                        let x12 = (ulong)i
                        let x13 = (long)i
                        let x14 = (uint)i
                        let x15 = (int)i
                        let x16 = (ushort)i
                        let x17 = (short)i
                        let x18 = (sbyte)i
                        let x19 = (byte)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType() + " " + x19.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

        }

        [TestMethod]
        public void LetCoalescer_Tupletization_Types2()
        {
            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        select (x0.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        select (x0.GetType() + " " + x1.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        let x12 = (ulong)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        let x12 = (ulong)0
                        let x13 = (long)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        let x12 = (ulong)0
                        let x13 = (long)0
                        let x14 = (uint)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        let x12 = (ulong)0
                        let x13 = (long)0
                        let x14 = (uint)0
                        let x15 = (int)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        let x12 = (ulong)0
                        let x13 = (long)0
                        let x14 = (uint)0
                        let x15 = (int)0
                        let x16 = (ushort)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        let x12 = (ulong)0
                        let x13 = (long)0
                        let x14 = (uint)0
                        let x15 = (int)0
                        let x16 = (ushort)0
                        let x17 = (short)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        let x12 = (ulong)0
                        let x13 = (long)0
                        let x14 = (uint)0
                        let x15 = (int)0
                        let x16 = (ushort)0
                        let x17 = (short)0
                        let x18 = (sbyte)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)0
                        let x1 = (byte)0
                        let x2 = (sbyte)0
                        let x3 = (short)0
                        let x4 = (ushort)0
                        let x5 = (int)0
                        let x6 = (uint)0
                        let x7 = (long)0
                        let x8 = (ulong)0
                        let x9 = (float)0
                        let x10 = (double)0
                        let x11 = (float)0
                        let x12 = (ulong)0
                        let x13 = (long)0
                        let x14 = (uint)0
                        let x15 = (int)0
                        let x16 = (ushort)0
                        let x17 = (short)0
                        let x18 = (sbyte)0
                        let x19 = (byte)0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType() + " " + x19.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2, counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

        }

        [TestMethod]
        public void LetCoalescer_Tupletization_Types3()
        {
            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        select (x0.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        select (x0.GetType() + " " + x1.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(3 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(4 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(5 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(6 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(7 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(8 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(9 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(10 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(11 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(12 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(13 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        let x12 = (ulong)x11
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(14 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        let x12 = (ulong)x11
                        let x13 = (long)x12
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(15 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        let x12 = (ulong)x11
                        let x13 = (long)x12
                        let x14 = (uint)x13
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(16 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        let x12 = (ulong)x11
                        let x13 = (long)x12
                        let x14 = (uint)x13
                        let x15 = (int)x14
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(17 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        let x12 = (ulong)x11
                        let x13 = (long)x12
                        let x14 = (uint)x13
                        let x15 = (int)x14
                        let x16 = (ushort)x15
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(18 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        let x12 = (ulong)x11
                        let x13 = (long)x12
                        let x14 = (uint)x13
                        let x15 = (int)x14
                        let x16 = (ushort)x15
                        let x17 = (short)x16
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(19 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        let x12 = (ulong)x11
                        let x13 = (long)x12
                        let x14 = (uint)x13
                        let x15 = (int)x14
                        let x16 = (ushort)x15
                        let x17 = (short)x16
                        let x18 = (sbyte)x17
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(20 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x1
                        let x3 = (short)x2
                        let x4 = (ushort)x3
                        let x5 = (int)x4
                        let x6 = (uint)x5
                        let x7 = (long)x6
                        let x8 = (ulong)x7
                        let x9 = (float)x8
                        let x10 = (double)x9
                        let x11 = (float)x10
                        let x12 = (ulong)x11
                        let x13 = (long)x12
                        let x14 = (uint)x13
                        let x15 = (int)x14
                        let x16 = (ushort)x15
                        let x17 = (short)x16
                        let x18 = (sbyte)x17
                        let x19 = (byte)x18
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType() + " " + x19.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(21 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

        }

        [TestMethod]
        public void LetCoalescer_Tupletization_Types4()
        {
            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        select (x0.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        select (x0.GetType() + " " + x1.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(3 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(3 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(4 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(4 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(5 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(5 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(6 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(6 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(7 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(7 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(8 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        let x12 = (ulong)x10
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(8 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        let x12 = (ulong)x10
                        let x13 = (long)x12
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(9 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        let x12 = (ulong)x10
                        let x13 = (long)x12
                        let x14 = (uint)x12
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(9 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        let x12 = (ulong)x10
                        let x13 = (long)x12
                        let x14 = (uint)x12
                        let x15 = (int)x14
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(10 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        let x12 = (ulong)x10
                        let x13 = (long)x12
                        let x14 = (uint)x12
                        let x15 = (int)x14
                        let x16 = (ushort)x14
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(10 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        let x12 = (ulong)x10
                        let x13 = (long)x12
                        let x14 = (uint)x12
                        let x15 = (int)x14
                        let x16 = (ushort)x14
                        let x17 = (short)x16
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(11 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        let x12 = (ulong)x10
                        let x13 = (long)x12
                        let x14 = (uint)x12
                        let x15 = (int)x14
                        let x16 = (ushort)x14
                        let x17 = (short)x16
                        let x18 = (sbyte)x16
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(11 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)x0
                        let x3 = (short)x2
                        let x4 = (ushort)x2
                        let x5 = (int)x4
                        let x6 = (uint)x4
                        let x7 = (long)x6
                        let x8 = (ulong)x6
                        let x9 = (float)x8
                        let x10 = (double)x8
                        let x11 = (float)x10
                        let x12 = (ulong)x10
                        let x13 = (long)x12
                        let x14 = (uint)x12
                        let x15 = (int)x14
                        let x16 = (ushort)x14
                        let x17 = (short)x16
                        let x18 = (sbyte)x16
                        let x19 = (byte)x18
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType() + " " + x19.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(12 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

        }

        [TestMethod]
        public void LetCoalescer_Tupletization_Types5()
        {
            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        select (x0.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(2 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        select (x0.GetType() + " " + x1.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(3 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(3 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(4 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(4 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(5 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(5 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(6 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(6 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(7 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(7 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(8 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        let x12 = (ulong)x9
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(8 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        let x12 = (ulong)x9
                        let x13 = (long)x12
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(9 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        let x12 = (ulong)x9
                        let x13 = (long)x12
                        let x14 = (uint)x11
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(9 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        let x12 = (ulong)x9
                        let x13 = (long)x12
                        let x14 = (uint)x11
                        let x15 = (int)x14
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(10 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        let x12 = (ulong)x9
                        let x13 = (long)x12
                        let x14 = (uint)x11
                        let x15 = (int)x14
                        let x16 = (ushort)x13
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(10 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        let x12 = (ulong)x9
                        let x13 = (long)x12
                        let x14 = (uint)x11
                        let x15 = (int)x14
                        let x16 = (ushort)x13
                        let x17 = (short)x16
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(11 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        let x12 = (ulong)x9
                        let x13 = (long)x12
                        let x14 = (uint)x11
                        let x15 = (int)x14
                        let x16 = (ushort)x13
                        let x17 = (short)x16
                        let x18 = (sbyte)x15
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(11 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

            {
                var e =
                    Infer(() => 
                        from i in Enumerable.Range(0, 1)
                        let x0 = (char)i
                        let x1 = (byte)x0
                        let x2 = (sbyte)i
                        let x3 = (short)x2
                        let x4 = (ushort)x1
                        let x5 = (int)x4
                        let x6 = (uint)x3
                        let x7 = (long)x6
                        let x8 = (ulong)x5
                        let x9 = (float)x8
                        let x10 = (double)x7
                        let x11 = (float)x10
                        let x12 = (ulong)x9
                        let x13 = (long)x12
                        let x14 = (uint)x11
                        let x15 = (int)x14
                        let x16 = (ushort)x13
                        let x17 = (short)x16
                        let x18 = (sbyte)x15
                        let x19 = (byte)x18
                        select (x0.GetType() + " " + x1.GetType() + " " + x2.GetType() + " " + x3.GetType() + " " + x4.GetType() + " " + x5.GetType() + " " + x6.GetType() + " " + x7.GetType() + " " + x8.GetType() + " " + x9.GetType() + " " + x10.GetType() + " " + x11.GetType() + " " + x12.GetType() + " " + x13.GetType() + " " + x14.GetType() + " " + x15.GetType() + " " + x16.GetType() + " " + x17.GetType() + " " + x18.GetType() + " " + x19.GetType()).ToString()
                    ).Body;

                var converter = new EnumerableToQueryTreeConverter();
                var q = converter.Convert(e);
                var l = new LetOptimizer();
                var o = l.Optimize(q);
                var r = o.Reduce();

                var counter = new SelectCountingVisitor();
                counter.Visit(r);
                Assert.AreEqual(12 , counter.Count); // one for the coalesced let and one for the final select
                Assert.AreEqual(e.Funcletize<IEnumerable<string>>().Compile()().First(), r.Funcletize<IEnumerable<string>>().Compile()().First());
            }

        }
    }
}
