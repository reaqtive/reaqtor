// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Nuqleon.DataModel;
using Nuqleon.DataModel.Serialization.Binary;
using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.Serialization.Binary
{
    [TestClass]
    public class DataTypeToCycleDetectorTests
    {
        [TestMethod]
        public void DataTypeToCycleDetector_OnlyCyclePaths()
        {
            var instance = new MyCyclicType
            {
                Inner = new InnerType
                {
                    Maker = new CycleMaker
                    {
                        Cycle = new MyCyclicType(),
                        Bar = "bar",
                    },
                    Dummy = new DummyPath
                    {
                        Foo = 42,
                    }
                }
            };

            var cycleDetector = new DataTypeToCycleDetector();
            var expression = (Expression<Action<object, HashSet<object>>>)cycleDetector.Visit(DataType.FromType(typeof(MyCyclicType), allowCycles: true));
            var compiled = expression.Compile();
            compiled(instance, new HashSet<object>());
        }

        [TestMethod]
        public void DataTypeToCycleDetector_Hierarchical()
        {
            var cycleDetector = new DataTypeToCycleDetector();
            // Any cyclic properties would be ignored on the parent class.
            Assert.IsNull(cycleDetector.Visit(DataType.FromType(typeof(DerivedCycle), allowCycles: true)));
        }

#pragma warning disable IDE0052 // Remove unread private members (kept because DataModel uses reflection)
        private class MyCyclicType
        {
            [Mapping("Inner")]
            public InnerType Inner { get; set; }
        }

        private class InnerType
        {
            private DummyPath _dummy;

            [Mapping("Maker")]
            public CycleMaker Maker { get; set; }

            [Mapping("Dummy")]
            public DummyPath Dummy
            {
                get => throw new InvalidOperationException();
                set => _dummy = value;
            }
        }

        private class CycleMaker
        {
            private string _bar;

            [Mapping("Cycle")]
            public MyCyclicType Cycle { get; set; }

            [Mapping("Bar")]
            public string Bar
            {
                get => throw new InvalidOperationException();
                set => _bar = value;
            }
        }

        private class DummyPath
        {
            private int _foo;

            [Mapping("Foo")]
            public int Foo
            {
                get => throw new InvalidOperationException();
                set => _foo = value;
            }
        }

        private class HierarchicalCycle
        {
            [Mapping("Bar")]
            public string Bar { get; set; }
        }

        private class DerivedCycle : HierarchicalCycle
        {
            [Mapping("Foo")]
            public int Foo { get; set; }

            [Mapping("Inner")]
            public HierarchicalInner Inner { get; set; }
        }

        private class HierarchicalInner
        {
            [Mapping("Cycle")]
            public HierarchicalCycle Cycle { get; set; }
        }
#pragma warning restore IDE0052 // Remove unread private members
    }
}
