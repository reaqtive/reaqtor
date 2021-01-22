// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization.Binary;

using BenchmarkDotNet.Attributes;

#if USE_SLIM
using ExpressionFactory = System.Linq.Expressions.ExpressionSlimFactory;
#endif

namespace BinaryExpressionSerialization
{
    public class Serialize
    {
        private readonly CustomBonsaiSerializer _jsonSerializer;
        private readonly ExpressionSerializer _binarySerializer;

        private readonly Expression _expr;

        public Serialize()
        {
            _jsonSerializer = new CustomBonsaiSerializer();
            _binarySerializer = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);

            _expr = (Expression<Func<IEnumerable<int>>>)(() => Enumerable.Range(0, 10).Where(x => x > 0).Select(x => x * x));
        }

        [Benchmark]
        public void Json()
        {
            _ = _jsonSerializer.Serialize(_expr.ToExpressionSlim());
        }

        [Benchmark]
        public void Binary()
        {
            _ = _binarySerializer.Serialize(_expr.ToExpressionSlim());
        }
    }

    public class Deserialize
    {
        private readonly CustomBonsaiSerializer _jsonSerializer;
        private readonly ExpressionSerializer _binarySerializer;

        private readonly string _json;
        private readonly byte[] _binary;

        public Deserialize()
        {
            _jsonSerializer = new CustomBonsaiSerializer();
            _binarySerializer = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);

            var expr = (Expression<Func<IEnumerable<int>>>)(() => Enumerable.Range(0, 10).Where(x => x > 0).Select(x => x * x));

            _json = _jsonSerializer.Serialize(expr.ToExpressionSlim());
            _binary = _binarySerializer.Serialize(expr.ToExpressionSlim());
        }

        [Benchmark]
        public void Json()
        {
            _ = _jsonSerializer.Deserialize(_json);
        }

        [Benchmark]
        public void Binary()
        {
            _ = _binarySerializer.Deserialize(_binary);
        }
    }
}
