// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Collections.Generic
{
    internal static class EqualityComparerExtensions
    {
        public static IEqualityComparer<Tuplet<T1, T2>> CombineWithTuplet<T1, T2>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2)
        {
            return new TupletEqualityComparer<T1, T2>(comparer1, comparer2);
        }

        private sealed class TupletEqualityComparer<T1, T2> : IEqualityComparer<Tuplet<T1, T2>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
            }

            public bool Equals(Tuplet<T1, T2> x, Tuplet<T1, T2> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2);

            public int GetHashCode(Tuplet<T1, T2> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3>> CombineWithTuplet<T1, T2, T3>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3)
        {
            return new TupletEqualityComparer<T1, T2, T3>(comparer1, comparer2, comparer3);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3> : IEqualityComparer<Tuplet<T1, T2, T3>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
            }

            public bool Equals(Tuplet<T1, T2, T3> x, Tuplet<T1, T2, T3> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3);

            public int GetHashCode(Tuplet<T1, T2, T3> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4>> CombineWithTuplet<T1, T2, T3, T4>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4>(comparer1, comparer2, comparer3, comparer4);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4> : IEqualityComparer<Tuplet<T1, T2, T3, T4>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4> x, Tuplet<T1, T2, T3, T4> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4);

            public int GetHashCode(Tuplet<T1, T2, T3, T4> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5>> CombineWithTuplet<T1, T2, T3, T4, T5>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5>(comparer1, comparer2, comparer3, comparer4, comparer5);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5> x, Tuplet<T1, T2, T3, T4, T5> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6>> CombineWithTuplet<T1, T2, T3, T4, T5, T6>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6> x, Tuplet<T1, T2, T3, T4, T5, T6> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7> x, Tuplet<T1, T2, T3, T4, T5, T6, T7> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;
            private readonly IEqualityComparer<T13> _comparer13;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
                _comparer13 = comparer13;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12)
                && _comparer13.Equals(x.Item13, y.Item13);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12),
                    _comparer13.GetHashCode(obj.Item13)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;
            private readonly IEqualityComparer<T13> _comparer13;
            private readonly IEqualityComparer<T14> _comparer14;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
                _comparer13 = comparer13;
                _comparer14 = comparer14;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12)
                && _comparer13.Equals(x.Item13, y.Item13)
                && _comparer14.Equals(x.Item14, y.Item14);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12),
                    _comparer13.GetHashCode(obj.Item13),
                    _comparer14.GetHashCode(obj.Item14)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14, comparer15);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;
            private readonly IEqualityComparer<T13> _comparer13;
            private readonly IEqualityComparer<T14> _comparer14;
            private readonly IEqualityComparer<T15> _comparer15;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
                _comparer13 = comparer13;
                _comparer14 = comparer14;
                _comparer15 = comparer15;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12)
                && _comparer13.Equals(x.Item13, y.Item13)
                && _comparer14.Equals(x.Item14, y.Item14)
                && _comparer15.Equals(x.Item15, y.Item15);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12),
                    _comparer13.GetHashCode(obj.Item13),
                    _comparer14.GetHashCode(obj.Item14),
                    _comparer15.GetHashCode(obj.Item15)
                );
        }

        public static IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> CombineWithTuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15, IEqualityComparer<T16> comparer16)
        {
            return new TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14, comparer15, comparer16);
        }

        private sealed class TupletEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IEqualityComparer<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;
            private readonly IEqualityComparer<T13> _comparer13;
            private readonly IEqualityComparer<T14> _comparer14;
            private readonly IEqualityComparer<T15> _comparer15;
            private readonly IEqualityComparer<T16> _comparer16;

            public TupletEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15, IEqualityComparer<T16> comparer16)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
                _comparer13 = comparer13;
                _comparer14 = comparer14;
                _comparer15 = comparer15;
                _comparer16 = comparer16;
            }

            public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> x, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12)
                && _comparer13.Equals(x.Item13, y.Item13)
                && _comparer14.Equals(x.Item14, y.Item14)
                && _comparer15.Equals(x.Item15, y.Item15)
                && _comparer16.Equals(x.Item16, y.Item16);

            public int GetHashCode(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12),
                    _comparer13.GetHashCode(obj.Item13),
                    _comparer14.GetHashCode(obj.Item14),
                    _comparer15.GetHashCode(obj.Item15),
                    _comparer16.GetHashCode(obj.Item16)
                );
        }

        public static IEqualityComparer<(T1, T2)> CombineWithValueTuple<T1, T2>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2) => new ValueTupleEqualityComparer<T1, T2>(comparer1, comparer2);

        private sealed class ValueTupleEqualityComparer<T1, T2> : IEqualityComparer<(T1 Item1, T2 Item2)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
            }

            public bool Equals((T1 Item1, T2 Item2) x, (T1 Item1, T2 Item2) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2);

            public int GetHashCode((T1 Item1, T2 Item2) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2)
                );
        }

        public static IEqualityComparer<(T1, T2, T3)> CombineWithValueTuple<T1, T2, T3>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3) => new ValueTupleEqualityComparer<T1, T2, T3>(comparer1, comparer2, comparer3);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3) x, (T1 Item1, T2 Item2, T3 Item3) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4)> CombineWithValueTuple<T1, T2, T3, T4>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4) => new ValueTupleEqualityComparer<T1, T2, T3, T4>(comparer1, comparer2, comparer3, comparer4);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5)> CombineWithValueTuple<T1, T2, T3, T4, T5>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5>(comparer1, comparer2, comparer3, comparer4, comparer5);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;
            private readonly IEqualityComparer<T13> _comparer13;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
                _comparer13 = comparer13;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12)
                && _comparer13.Equals(x.Item13, y.Item13);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12),
                    _comparer13.GetHashCode(obj.Item13)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;
            private readonly IEqualityComparer<T13> _comparer13;
            private readonly IEqualityComparer<T14> _comparer14;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
                _comparer13 = comparer13;
                _comparer14 = comparer14;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12)
                && _comparer13.Equals(x.Item13, y.Item13)
                && _comparer14.Equals(x.Item14, y.Item14);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12),
                    _comparer13.GetHashCode(obj.Item13),
                    _comparer14.GetHashCode(obj.Item14)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14, comparer15);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14, T15 Item15)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;
            private readonly IEqualityComparer<T13> _comparer13;
            private readonly IEqualityComparer<T14> _comparer14;
            private readonly IEqualityComparer<T15> _comparer15;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
                _comparer13 = comparer13;
                _comparer14 = comparer14;
                _comparer15 = comparer15;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14, T15 Item15) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14, T15 Item15) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12)
                && _comparer13.Equals(x.Item13, y.Item13)
                && _comparer14.Equals(x.Item14, y.Item14)
                && _comparer15.Equals(x.Item15, y.Item15);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14, T15 Item15) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12),
                    _comparer13.GetHashCode(obj.Item13),
                    _comparer14.GetHashCode(obj.Item14),
                    _comparer15.GetHashCode(obj.Item15)
                );
        }

        public static IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> CombineWithValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15, IEqualityComparer<T16> comparer16) => new ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14, comparer15, comparer16);

        private sealed class ValueTupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IEqualityComparer<(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14, T15 Item15, T16 Item16)>
        {
            private readonly IEqualityComparer<T1> _comparer1;
            private readonly IEqualityComparer<T2> _comparer2;
            private readonly IEqualityComparer<T3> _comparer3;
            private readonly IEqualityComparer<T4> _comparer4;
            private readonly IEqualityComparer<T5> _comparer5;
            private readonly IEqualityComparer<T6> _comparer6;
            private readonly IEqualityComparer<T7> _comparer7;
            private readonly IEqualityComparer<T8> _comparer8;
            private readonly IEqualityComparer<T9> _comparer9;
            private readonly IEqualityComparer<T10> _comparer10;
            private readonly IEqualityComparer<T11> _comparer11;
            private readonly IEqualityComparer<T12> _comparer12;
            private readonly IEqualityComparer<T13> _comparer13;
            private readonly IEqualityComparer<T14> _comparer14;
            private readonly IEqualityComparer<T15> _comparer15;
            private readonly IEqualityComparer<T16> _comparer16;

            public ValueTupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15, IEqualityComparer<T16> comparer16)
            {
                _comparer1 = comparer1;
                _comparer2 = comparer2;
                _comparer3 = comparer3;
                _comparer4 = comparer4;
                _comparer5 = comparer5;
                _comparer6 = comparer6;
                _comparer7 = comparer7;
                _comparer8 = comparer8;
                _comparer9 = comparer9;
                _comparer10 = comparer10;
                _comparer11 = comparer11;
                _comparer12 = comparer12;
                _comparer13 = comparer13;
                _comparer14 = comparer14;
                _comparer15 = comparer15;
                _comparer16 = comparer16;
            }

            public bool Equals((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14, T15 Item15, T16 Item16) x, (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14, T15 Item15, T16 Item16) y) =>
                   _comparer1.Equals(x.Item1, y.Item1)
                && _comparer2.Equals(x.Item2, y.Item2)
                && _comparer3.Equals(x.Item3, y.Item3)
                && _comparer4.Equals(x.Item4, y.Item4)
                && _comparer5.Equals(x.Item5, y.Item5)
                && _comparer6.Equals(x.Item6, y.Item6)
                && _comparer7.Equals(x.Item7, y.Item7)
                && _comparer8.Equals(x.Item8, y.Item8)
                && _comparer9.Equals(x.Item9, y.Item9)
                && _comparer10.Equals(x.Item10, y.Item10)
                && _comparer11.Equals(x.Item11, y.Item11)
                && _comparer12.Equals(x.Item12, y.Item12)
                && _comparer13.Equals(x.Item13, y.Item13)
                && _comparer14.Equals(x.Item14, y.Item14)
                && _comparer15.Equals(x.Item15, y.Item15)
                && _comparer16.Equals(x.Item16, y.Item16);

            public int GetHashCode((T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9, T10 Item10, T11 Item11, T12 Item12, T13 Item13, T14 Item14, T15 Item15, T16 Item16) obj) =>
                HashHelpers.Combine(
                    _comparer1.GetHashCode(obj.Item1),
                    _comparer2.GetHashCode(obj.Item2),
                    _comparer3.GetHashCode(obj.Item3),
                    _comparer4.GetHashCode(obj.Item4),
                    _comparer5.GetHashCode(obj.Item5),
                    _comparer6.GetHashCode(obj.Item6),
                    _comparer7.GetHashCode(obj.Item7),
                    _comparer8.GetHashCode(obj.Item8),
                    _comparer9.GetHashCode(obj.Item9),
                    _comparer10.GetHashCode(obj.Item10),
                    _comparer11.GetHashCode(obj.Item11),
                    _comparer12.GetHashCode(obj.Item12),
                    _comparer13.GetHashCode(obj.Item13),
                    _comparer14.GetHashCode(obj.Item14),
                    _comparer15.GetHashCode(obj.Item15),
                    _comparer16.GetHashCode(obj.Item16)
                );
        }

    }
}
