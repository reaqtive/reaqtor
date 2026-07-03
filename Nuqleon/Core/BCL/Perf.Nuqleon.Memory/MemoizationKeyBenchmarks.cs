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
// Cache hits are measured (the body never runs on a hit, so the key is the entire per-invocation
// cost): a same-key series and a rotating series over 1,024 pre-warmed keys.
//
// FINDINGS (2026-07-03, .NET 10, ShortRun; see also KeyPrimitiveBenchmarks):
//
// - Neither key type has faster primitives. Tuplet's FastEqualityComparer/FastComparer are just
//   cached EqualityComparer<T>.Default/Comparer<T>.Default, and its hash combiner is the same
//   corefx rol5 that ValueTuple uses. At arity 16 with collision-free keys, ValueTuple measured
//   marginally FASTER end to end (~0.9x).
//
// - The dramatic arity-16 results (50-128x in favor of Tuplet) are entirely a hash-quality
//   effect: ValueTuple.GetHashCode on a TRest-nested tuple (arity > 8) incorporates ONLY THE
//   LAST 8 elements. Keys that differ only in earlier elements all share one hash code, so the
//   cache dictionary degenerates into a single bucket with O(n) scans. The vary-first rotation
//   here hits that cliff (1,024 keys -> 1 distinct hash); the vary-last twins avoid it and show
//   the two representations equivalent. Tuplet hashes all elements at every arity and has no
//   such cliff.
//
// - The low-arity difference (~1.2-1.4x) is a small constant factor: ValueTuple's GetHashCode
//   carries a random-seed load and extra combine steps (19 B vs 76 B of JITted code at arity 2),
//   amplified through the memoizer's wrapper chain, with heavily overlapping error bars.
//
// Verdict: Tuplet's advantage is collision-robustness for arity > 8 keys - a memoized function
// whose varying arguments sit in early positions (with stable trailing arguments) collapses to
// O(n) lookups under ValueTuple keys. That, not raw speed, is the reason to keep Tuplet.
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
        private Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> _tupletInt16VaryLast;
        private Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> _valueTupleInt16VaryLast;
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
            // Vary-last twins for the arity-16 shapes, on their own memoizer so their caches hold
            // only vary-last keys: ValueTuple's GetHashCode ignores all but the last 8 elements of
            // a TRest-nested tuple, so vary-FIRST populations collapse into a single hash bucket
            // while vary-LAST populations spread normally. Comparing the two isolates that
            // collision effect from the intrinsic cost of nested hashing.
            //
            var memoizerVaryLast = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            _tupletInt16VaryLast = memoizerVaryLast.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(static (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + p).Delegate;
            {
                var res = memoizerVaryLast.Memoize<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int), int>(static a => a.Item1 + a.Item16, MemoizationOptions.None, comparer: null);
                var memoized = res.Delegate;
                _valueTupleInt16VaryLast = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16));
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
                _ = _tupletInt16VaryLast(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, i);
                _ = _valueTupleInt16VaryLast(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, i);
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

        //
        // Vary-last arity-16 rotation: collision-free for both key kinds.
        //

        [BenchmarkCategory("Int16_RotatingVaryLast"), Benchmark(Baseline = true)]
        public int Tuplet_Int16_RotatingVaryLast()
        {
            var i = _ints[_index = (_index + 1) & KeyMask];
            return _tupletInt16VaryLast(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, i);
        }

        [BenchmarkCategory("Int16_RotatingVaryLast"), Benchmark]
        public int ValueTuple_Int16_RotatingVaryLast()
        {
            var i = _ints[_index = (_index + 1) & KeyMask];
            return _valueTupleInt16VaryLast(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, i);
        }
    }
}
