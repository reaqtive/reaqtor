// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using Nuqleon.DataModel.Serialization.Binary;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.Serialization.Binary
{
    [TestClass]
    public partial class DataModelSerializerFactoryParallelTests
    {
        private static readonly MemoryStreamPool s_streamPool = MemoryStreamPool.Create(1024);

        private static readonly int DegreeOfParallelism = Environment.ProcessorCount;
        private static readonly int Repeat = DegreeOfParallelism * 10;
        private static readonly ParallelOptions s_parallelOptions = new()
        {
            MaxDegreeOfParallelism = DegreeOfParallelism
        };

        private readonly DataModelSerializerFactoryTestCase testCase = new();

        #region DataModelSerializerFactory.TestCases

        [TestMethod]
        public void DataModelSerializerFactory_Primitives()
        {
            Run(testCase.DataModelSerializerFactory_Primitives_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Arrays()
        {
            Run(testCase.DataModelSerializerFactory_Arrays_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Lists()
        {
            Run(testCase.DataModelSerializerFactory_Lists_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Enums()
        {
            Run(testCase.DataModelSerializerFactory_Enums_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Record()
        {
            Run(testCase.DataModelSerializerFactory_Record_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Anonymous()
        {
            Run(testCase.DataModelSerializerFactory_Anonymous_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Entity()
        {
            Run(testCase.DataModelSerializerFactory_Entity_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Tuples()
        {
            Run(testCase.DataModelSerializerFactory_Tuples_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_ArrayConversions()
        {
            Run(testCase.DataModelSerializerFactory_ArrayConversions_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_EnumConversions()
        {
            Run(testCase.DataModelSerializerFactory_EnumConversions_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_StructuralConversions()
        {
            Run(testCase.DataModelSerializerFactory_StructuralConversions_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Expressions()
        {
            Run(testCase.DataModelSerializerFactory_Expressions_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_StructuralTypeCycle()
        {
            Run(testCase.DataModelSerializerFactory_StructuralTypeCycle_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_StructuralTypeCycle_Anonymous()
        {
            Run(testCase.DataModelSerializerFactory_StructuralTypeCycle_Anonymous_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_ManOrBoy()
        {
            Run(testCase.DataModelSerializerFactory_ManOrBoy_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_IntArrayArray()
        {
            Run(testCase.DataModelSerializerFactory_IntArrayArray_Tests());
        }

        #endregion

        private static void Run(DataModelSerializerFactoryTestCase.Tests tests)
        {
            // Each test individually
            var constantExpressionSerializer = new DataModelSerializerFactoryTestCase.ConstantExpressionSerializer();
            var factory = new DataTypeBinarySerializer(constantExpressionSerializer);
            constantExpressionSerializer.DataSerializer = factory;

            foreach (var test in tests)
            {
                Parallel.ForEach(
                    Enumerable.Repeat(test, Repeat),
                    s_parallelOptions,
                    t =>
                    {
                        using var holder = s_streamPool.New();

                        factory.Serialize(t.InputType, holder.MemoryStream, t.Value);

                        holder.MemoryStream.Position = 0;

                        var rt = factory.Deserialize(t.OutputType, holder.MemoryStream);

                        Assert.IsTrue(t.Comparer.Equals(t.Value, rt), "Expected: {0} Actual: {1}", t.Value, rt);
                    }
                );
            }

            // Each test in random order
            var r = new Random();
            Parallel.ForEach(
                Enumerable.Repeat(tests, Repeat).SelectMany(t => t).OrderBy(_ => r.Next()),
                t =>
                {
                    using var holder = s_streamPool.New();

                    factory.Serialize(t.InputType, holder.MemoryStream, t.Value);

                    holder.MemoryStream.Position = 0;

                    var rt = factory.Deserialize(t.OutputType, holder.MemoryStream);

                    Assert.IsTrue(t.Comparer.Equals(t.Value, rt), "Expected: {0} Actual: {1}", t.Value, rt);
                }
            );
        }
    }
}
