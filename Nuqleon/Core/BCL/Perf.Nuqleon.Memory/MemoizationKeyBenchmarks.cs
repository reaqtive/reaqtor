// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Decision instrument for the USE_VALUETUPLE flag in FunctionMemoizationExtensions.Generated.tt:
// the N-ary Memoize extension methods key their cache on either the library's own Tuplet<...>
// structs (the shipping configuration) or BCL ValueTuple (the dormant #if USE_VALUETUPLE branch).
// Both key shapes are reachable through the public IMemoizer.Memoize<TKey, TResult> API, so these
// benchmarks measure exactly what the flag switches without rebuilding the library:
//
//   - the Tuplet side calls the shipping arity extension (memoizer.Memoize<T1, ..., TResult>);
//   - the ValueTuple side replicates the #if USE_VALUETUPLE branch verbatim.
//
// The interesting divergence is (a) hash/equality cost per lookup, in particular beyond arity 7
// where ValueTuple nests through TRest while Tuplet stays flat, and (b) reference-typed elements
// where per-element hashing dominates. Cache hits are measured (the body never runs on a hit, so
// the key is the entire per-invocation cost): a same-key series and a rotating series over 1,024
// pre-warmed keys to exercise hash distribution over a populated cache.
//

using System;
using System.Memory;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Perf.Nuqleon.Memory
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class MemoizationKeyBenchmarks
    {
        private const int KeyCount = 1024;
        private const int KeyMask = KeyCount - 1;

        private Func<int, int, int> _tupletInt2;
        private Func<int, int, int> _valueTupleInt2;
        private Func<int, int, int, int, int, int, int, int, int> _tupletInt8;
        private Func<int, int, int, int, int, int, int, int, int> _valueTupleInt8;
        private Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> _tupletInt16;
        private Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> _valueTupleInt16;
        private Func<string, string, int> _tupletString2;
        private Func<string, string, int> _valueTupleString2;

        private int[] _ints;
        private string[] _strings;
        private int _index;

        [GlobalSetup]
        public void Setup()
        {
            var memoizer = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            //
            // Tuplet keys: the shipping arity extensions (compiled without USE_VALUETUPLE).
            //
            _tupletInt2 = memoizer.Memoize<int, int, int>(static (a, b) => a + b).Delegate;
            _tupletInt8 = memoizer.Memoize<int, int, int, int, int, int, int, int, int>(static (a, b, c, d, e, f, g, h) => a + h).Delegate;
            _tupletInt16 = memoizer.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(static (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + p).Delegate;
            _tupletString2 = memoizer.Memoize<string, string, int>(static (a, b) => a.Length + b.Length).Delegate;

            //
            // ValueTuple keys: verbatim mirror of the #if USE_VALUETUPLE branch in
            // FunctionMemoizationExtensions.Generated.tt.
            //
            {
                var res = memoizer.Memoize<(int, int), int>(static a => a.Item1 + a.Item2, MemoizationOptions.None, comparer: null);
                var memoized = res.Delegate;
                _valueTupleInt2 = (t1, t2) => memoized((t1, t2));
            }
            {
                var res = memoizer.Memoize<(int, int, int, int, int, int, int, int), int>(static a => a.Item1 + a.Item8, MemoizationOptions.None, comparer: null);
                var memoized = res.Delegate;
                _valueTupleInt8 = (t1, t2, t3, t4, t5, t6, t7, t8) => memoized((t1, t2, t3, t4, t5, t6, t7, t8));
            }
            {
                var res = memoizer.Memoize<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int), int>(static a => a.Item1 + a.Item16, MemoizationOptions.None, comparer: null);
                var memoized = res.Delegate;
                _valueTupleInt16 = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16));
            }
            {
                var res = memoizer.Memoize<(string, string), int>(static a => a.Item1.Length + a.Item2.Length, MemoizationOptions.None, comparer: null);
                var memoized = res.Delegate;
                _valueTupleString2 = (t1, t2) => memoized((t1, t2));
            }

            //
            // Pre-warm 1,024 distinct keys per delegate so the rotating benchmarks measure pure
            // hits against a populated cache.
            //
            _ints = new int[KeyCount];
            _strings = new string[KeyCount];

            for (var i = 0; i < KeyCount; i++)
            {
                _ints[i] = i;
                _strings[i] = "key_" + i.ToString(System.Globalization.CultureInfo.InvariantCulture);

                _ = _tupletInt2(i, i);
                _ = _valueTupleInt2(i, i);
                _ = _tupletInt8(i, 2, 3, 4, 5, 6, 7, 8);
                _ = _valueTupleInt8(i, 2, 3, 4, 5, 6, 7, 8);
                _ = _tupletInt16(i, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
                _ = _valueTupleInt16(i, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
                _ = _tupletString2(_strings[i], _strings[i]);
                _ = _valueTupleString2(_strings[i], _strings[i]);
            }
        }

        //
        // Same key on every call: minimal, steady-state hit.
        //

        [BenchmarkCategory("Int2_SameKey"), Benchmark(Baseline = true)]
        public int Tuplet_Int2_SameKey() => _tupletInt2(19, 23);

        [BenchmarkCategory("Int2_SameKey"), Benchmark]
        public int ValueTuple_Int2_SameKey() => _valueTupleInt2(19, 23);

        [BenchmarkCategory("Int8_SameKey"), Benchmark(Baseline = true)]
        public int Tuplet_Int8_SameKey() => _tupletInt8(1, 2, 3, 4, 5, 6, 7, 8);

        [BenchmarkCategory("Int8_SameKey"), Benchmark]
        public int ValueTuple_Int8_SameKey() => _valueTupleInt8(1, 2, 3, 4, 5, 6, 7, 8);

        [BenchmarkCategory("Int16_SameKey"), Benchmark(Baseline = true)]
        public int Tuplet_Int16_SameKey() => _tupletInt16(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

        [BenchmarkCategory("Int16_SameKey"), Benchmark]
        public int ValueTuple_Int16_SameKey() => _valueTupleInt16(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

        [BenchmarkCategory("String2_SameKey"), Benchmark(Baseline = true)]
        public int Tuplet_String2_SameKey() => _tupletString2("key_19", "key_23");

        [BenchmarkCategory("String2_SameKey"), Benchmark]
        public int ValueTuple_String2_SameKey() => _valueTupleString2("key_19", "key_23");

        //
        // Rotating keys: hits across a populated cache; exercises hash distribution and equality
        // against varying values.
        //

        [BenchmarkCategory("Int2_Rotating"), Benchmark(Baseline = true)]
        public int Tuplet_Int2_Rotating()
        {
            var i = _ints[_index = (_index + 1) & KeyMask];
            return _tupletInt2(i, i);
        }

        [BenchmarkCategory("Int2_Rotating"), Benchmark]
        public int ValueTuple_Int2_Rotating()
        {
            var i = _ints[_index = (_index + 1) & KeyMask];
            return _valueTupleInt2(i, i);
        }

        [BenchmarkCategory("Int8_Rotating"), Benchmark(Baseline = true)]
        public int Tuplet_Int8_Rotating()
        {
            var i = _ints[_index = (_index + 1) & KeyMask];
            return _tupletInt8(i, 2, 3, 4, 5, 6, 7, 8);
        }

        [BenchmarkCategory("Int8_Rotating"), Benchmark]
        public int ValueTuple_Int8_Rotating()
        {
            var i = _ints[_index = (_index + 1) & KeyMask];
            return _valueTupleInt8(i, 2, 3, 4, 5, 6, 7, 8);
        }

        [BenchmarkCategory("Int16_Rotating"), Benchmark(Baseline = true)]
        public int Tuplet_Int16_Rotating()
        {
            var i = _ints[_index = (_index + 1) & KeyMask];
            return _tupletInt16(i, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        }

        [BenchmarkCategory("Int16_Rotating"), Benchmark]
        public int ValueTuple_Int16_Rotating()
        {
            var i = _ints[_index = (_index + 1) & KeyMask];
            return _valueTupleInt16(i, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        }

        [BenchmarkCategory("String2_Rotating"), Benchmark(Baseline = true)]
        public int Tuplet_String2_Rotating()
        {
            var s = _strings[_index = (_index + 1) & KeyMask];
            return _tupletString2(s, s);
        }

        [BenchmarkCategory("String2_Rotating"), Benchmark]
        public int ValueTuple_String2_Rotating()
        {
            var s = _strings[_index = (_index + 1) & KeyMask];
            return _valueTupleString2(s, s);
        }
    }
}
