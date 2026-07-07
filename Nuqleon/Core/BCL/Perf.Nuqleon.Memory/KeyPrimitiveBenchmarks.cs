// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Companion to MemoizationKeyBenchmarks: measures the key primitives in isolation (hashing,
// equality, and a plain Dictionary lookup), taking the memoizer machinery out of the picture.
// This attributes the end-to-end differences to their mechanism:
//
//   - GetHashCode / Equals pairs show the raw per-operation cost of the flat Tuplet layout vs
//     ValueTuple (including its TRest nesting at arity 16);
//   - the Dictionary lookups over vary-first vs vary-last populations expose ValueTuple's hash
//     truncation (only the last 8 elements of a TRest-nested tuple contribute to GetHashCode),
//     which collapses vary-first populations into a single bucket.
//
// The DisassemblyDiagnoser output (BenchmarkDotNet.Artifacts) shows the generated code for the
// arity-2 primitives, making inlining/instruction-count differences directly inspectable.
//

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Perf.Nuqleon.Memory;

[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 3)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class KeyPrimitiveBenchmarks
{
    private const int KeyCount = 1024;
    private const int KeyMask = KeyCount - 1;

    private Tuplet<int, int> _tuplet2a, _tuplet2b;
    private (int, int) _vt2a, _vt2b;
    private Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> _tuplet16a, _tuplet16b;
    private (int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int) _vt16a, _vt16b;

    private Dictionary<Tuplet<int, int>, int> _dictTuplet2;
    private Dictionary<(int, int), int> _dictVt2;
    private Dictionary<Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int> _dictTuplet16First, _dictTuplet16Last;
    private Dictionary<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int), int> _dictVt16First, _dictVt16Last;

    private int _index;

    [GlobalSetup]
    public void Setup()
    {
        _tuplet2a = new(19, 23);
        _tuplet2b = new(19, 23);
        _vt2a = (19, 23);
        _vt2b = (19, 23);
        _tuplet16a = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        _tuplet16b = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        _vt16a = (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        _vt16b = (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

        _dictTuplet2 = [];
        _dictVt2 = [];
        _dictTuplet16First = [];
        _dictTuplet16Last = [];
        _dictVt16First = [];
        _dictVt16Last = [];

        for (var i = 0; i < KeyCount; i++)
        {
            _dictTuplet2[new(i, i)] = i;
            _dictVt2[(i, i)] = i;
            _dictTuplet16First[new(i, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)] = i;
            _dictTuplet16Last[new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, i)] = i;
            _dictVt16First[(i, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)] = i;
            _dictVt16Last[(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, i)] = i;
        }
    }

    //
    // Raw hashing.
    //

    [BenchmarkCategory("Hash_Int2"), Benchmark(Baseline = true)]
    public int Tuplet_Hash_Int2() => _tuplet2a.GetHashCode();

    [BenchmarkCategory("Hash_Int2"), Benchmark]
    public int ValueTuple_Hash_Int2() => _vt2a.GetHashCode();

    [BenchmarkCategory("Hash_Int16"), Benchmark(Baseline = true)]
    public int Tuplet_Hash_Int16() => _tuplet16a.GetHashCode();

    [BenchmarkCategory("Hash_Int16"), Benchmark]
    public int ValueTuple_Hash_Int16() => _vt16a.GetHashCode();

    //
    // Raw equality (equal operands: the full-comparison path a cache hit takes).
    //

    [BenchmarkCategory("Equals_Int2"), Benchmark(Baseline = true)]
    public bool Tuplet_Equals_Int2() => EqualityComparer<Tuplet<int, int>>.Default.Equals(_tuplet2a, _tuplet2b);

    [BenchmarkCategory("Equals_Int2"), Benchmark]
    public bool ValueTuple_Equals_Int2() => EqualityComparer<(int, int)>.Default.Equals(_vt2a, _vt2b);

    [BenchmarkCategory("Equals_Int16"), Benchmark(Baseline = true)]
    public bool Tuplet_Equals_Int16() => EqualityComparer<Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>.Default.Equals(_tuplet16a, _tuplet16b);

    [BenchmarkCategory("Equals_Int16"), Benchmark]
    public bool ValueTuple_Equals_Int16() => EqualityComparer<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int)>.Default.Equals(_vt16a, _vt16b);

    //
    // Plain Dictionary hit over 1,024 entries; vary-first vs vary-last populations.
    //

    [BenchmarkCategory("Dict_Int2"), Benchmark(Baseline = true)]
    public int Tuplet_Dict_Int2()
    {
        var i = _index = (_index + 1) & KeyMask;
        return _dictTuplet2[new(i, i)];
    }

    [BenchmarkCategory("Dict_Int2"), Benchmark]
    public int ValueTuple_Dict_Int2()
    {
        var i = _index = (_index + 1) & KeyMask;
        return _dictVt2[(i, i)];
    }

    [BenchmarkCategory("Dict_Int16_VaryFirst"), Benchmark(Baseline = true)]
    public int Tuplet_Dict_Int16_VaryFirst()
    {
        var i = _index = (_index + 1) & KeyMask;
        return _dictTuplet16First[new(i, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)];
    }

    [BenchmarkCategory("Dict_Int16_VaryFirst"), Benchmark]
    public int ValueTuple_Dict_Int16_VaryFirst()
    {
        var i = _index = (_index + 1) & KeyMask;
        return _dictVt16First[(i, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)];
    }

    [BenchmarkCategory("Dict_Int16_VaryLast"), Benchmark(Baseline = true)]
    public int Tuplet_Dict_Int16_VaryLast()
    {
        var i = _index = (_index + 1) & KeyMask;
        return _dictTuplet16Last[new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, i)];
    }

    [BenchmarkCategory("Dict_Int16_VaryLast"), Benchmark]
    public int ValueTuple_Dict_Int16_VaryLast()
    {
        var i = _index = (_index + 1) & KeyMask;
        return _dictVt16Last[(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, i)];
    }
}
