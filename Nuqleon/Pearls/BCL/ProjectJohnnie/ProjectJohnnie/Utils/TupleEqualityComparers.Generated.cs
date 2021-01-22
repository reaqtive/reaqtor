// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace ProjectJohnnie
{
    internal sealed class TupleEqualityComparer<T1> : IEqualityComparer<Tuple<T1>>
    {
        private readonly IEqualityComparer<T1> _comparer1;

        public TupleEqualityComparer(IEqualityComparer<T1> comparer1)
        {
            _comparer1 = comparer1;
        }

        public bool Equals(Tuple<T1> x, Tuple<T1> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return true;

            return _comparer1.Equals(x.Item1, y.Item1);
        }

        public int GetHashCode(Tuple<T1> obj)
        {
            if (obj == null)
                return 0;

            return HashHelpers.Combine(_comparer1.GetHashCode(obj.Item1));
        }

    }

    internal sealed class TupleEqualityComparer<T1, T2> : IEqualityComparer<Tuple<T1, T2>>
    {
        private readonly IEqualityComparer<T1> _comparer1;
        private readonly IEqualityComparer<T2> _comparer2;

        public TupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2)
        {
            _comparer1 = comparer1;
            _comparer2 = comparer2;
        }

        public bool Equals(Tuple<T1, T2> x, Tuple<T1, T2> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return true;

            return _comparer1.Equals(x.Item1, y.Item1) && _comparer2.Equals(x.Item2, y.Item2);
        }

        public int GetHashCode(Tuple<T1, T2> obj)
        {
            if (obj == null)
                return 0;

            return HashHelpers.Combine(_comparer1.GetHashCode(obj.Item1), _comparer2.GetHashCode(obj.Item2));
        }

    }

    internal sealed class TupleEqualityComparer<T1, T2, T3> : IEqualityComparer<Tuple<T1, T2, T3>>
    {
        private readonly IEqualityComparer<T1> _comparer1;
        private readonly IEqualityComparer<T2> _comparer2;
        private readonly IEqualityComparer<T3> _comparer3;

        public TupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3)
        {
            _comparer1 = comparer1;
            _comparer2 = comparer2;
            _comparer3 = comparer3;
        }

        public bool Equals(Tuple<T1, T2, T3> x, Tuple<T1, T2, T3> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return true;

            return _comparer1.Equals(x.Item1, y.Item1) && _comparer2.Equals(x.Item2, y.Item2) && _comparer3.Equals(x.Item3, y.Item3);
        }

        public int GetHashCode(Tuple<T1, T2, T3> obj)
        {
            if (obj == null)
                return 0;

            return HashHelpers.Combine(_comparer1.GetHashCode(obj.Item1), _comparer2.GetHashCode(obj.Item2), _comparer3.GetHashCode(obj.Item3));
        }

    }

    internal sealed class TupleEqualityComparer<T1, T2, T3, T4> : IEqualityComparer<Tuple<T1, T2, T3, T4>>
    {
        private readonly IEqualityComparer<T1> _comparer1;
        private readonly IEqualityComparer<T2> _comparer2;
        private readonly IEqualityComparer<T3> _comparer3;
        private readonly IEqualityComparer<T4> _comparer4;

        public TupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4)
        {
            _comparer1 = comparer1;
            _comparer2 = comparer2;
            _comparer3 = comparer3;
            _comparer4 = comparer4;
        }

        public bool Equals(Tuple<T1, T2, T3, T4> x, Tuple<T1, T2, T3, T4> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return true;

            return _comparer1.Equals(x.Item1, y.Item1) && _comparer2.Equals(x.Item2, y.Item2) && _comparer3.Equals(x.Item3, y.Item3) && _comparer4.Equals(x.Item4, y.Item4);
        }

        public int GetHashCode(Tuple<T1, T2, T3, T4> obj)
        {
            if (obj == null)
                return 0;

            return HashHelpers.Combine(_comparer1.GetHashCode(obj.Item1), _comparer2.GetHashCode(obj.Item2), _comparer3.GetHashCode(obj.Item3), _comparer4.GetHashCode(obj.Item4));
        }

    }

    internal sealed class TupleEqualityComparer<T1, T2, T3, T4, T5> : IEqualityComparer<Tuple<T1, T2, T3, T4, T5>>
    {
        private readonly IEqualityComparer<T1> _comparer1;
        private readonly IEqualityComparer<T2> _comparer2;
        private readonly IEqualityComparer<T3> _comparer3;
        private readonly IEqualityComparer<T4> _comparer4;
        private readonly IEqualityComparer<T5> _comparer5;

        public TupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5)
        {
            _comparer1 = comparer1;
            _comparer2 = comparer2;
            _comparer3 = comparer3;
            _comparer4 = comparer4;
            _comparer5 = comparer5;
        }

        public bool Equals(Tuple<T1, T2, T3, T4, T5> x, Tuple<T1, T2, T3, T4, T5> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return true;

            return _comparer1.Equals(x.Item1, y.Item1) && _comparer2.Equals(x.Item2, y.Item2) && _comparer3.Equals(x.Item3, y.Item3) && _comparer4.Equals(x.Item4, y.Item4) && _comparer5.Equals(x.Item5, y.Item5);
        }

        public int GetHashCode(Tuple<T1, T2, T3, T4, T5> obj)
        {
            if (obj == null)
                return 0;

            return HashHelpers.Combine(_comparer1.GetHashCode(obj.Item1), _comparer2.GetHashCode(obj.Item2), _comparer3.GetHashCode(obj.Item3), _comparer4.GetHashCode(obj.Item4), _comparer5.GetHashCode(obj.Item5));
        }

    }

    internal sealed class TupleEqualityComparer<T1, T2, T3, T4, T5, T6> : IEqualityComparer<Tuple<T1, T2, T3, T4, T5, T6>>
    {
        private readonly IEqualityComparer<T1> _comparer1;
        private readonly IEqualityComparer<T2> _comparer2;
        private readonly IEqualityComparer<T3> _comparer3;
        private readonly IEqualityComparer<T4> _comparer4;
        private readonly IEqualityComparer<T5> _comparer5;
        private readonly IEqualityComparer<T6> _comparer6;

        public TupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6)
        {
            _comparer1 = comparer1;
            _comparer2 = comparer2;
            _comparer3 = comparer3;
            _comparer4 = comparer4;
            _comparer5 = comparer5;
            _comparer6 = comparer6;
        }

        public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> x, Tuple<T1, T2, T3, T4, T5, T6> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return true;

            return _comparer1.Equals(x.Item1, y.Item1) && _comparer2.Equals(x.Item2, y.Item2) && _comparer3.Equals(x.Item3, y.Item3) && _comparer4.Equals(x.Item4, y.Item4) && _comparer5.Equals(x.Item5, y.Item5) && _comparer6.Equals(x.Item6, y.Item6);
        }

        public int GetHashCode(Tuple<T1, T2, T3, T4, T5, T6> obj)
        {
            if (obj == null)
                return 0;

            return HashHelpers.Combine(_comparer1.GetHashCode(obj.Item1), _comparer2.GetHashCode(obj.Item2), _comparer3.GetHashCode(obj.Item3), _comparer4.GetHashCode(obj.Item4), _comparer5.GetHashCode(obj.Item5), _comparer6.GetHashCode(obj.Item6));
        }

    }

    internal sealed class TupleEqualityComparer<T1, T2, T3, T4, T5, T6, T7> : IEqualityComparer<Tuple<T1, T2, T3, T4, T5, T6, T7>>
    {
        private readonly IEqualityComparer<T1> _comparer1;
        private readonly IEqualityComparer<T2> _comparer2;
        private readonly IEqualityComparer<T3> _comparer3;
        private readonly IEqualityComparer<T4> _comparer4;
        private readonly IEqualityComparer<T5> _comparer5;
        private readonly IEqualityComparer<T6> _comparer6;
        private readonly IEqualityComparer<T7> _comparer7;

        public TupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7)
        {
            _comparer1 = comparer1;
            _comparer2 = comparer2;
            _comparer3 = comparer3;
            _comparer4 = comparer4;
            _comparer5 = comparer5;
            _comparer6 = comparer6;
            _comparer7 = comparer7;
        }

        public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> x, Tuple<T1, T2, T3, T4, T5, T6, T7> y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return true;

            return _comparer1.Equals(x.Item1, y.Item1) && _comparer2.Equals(x.Item2, y.Item2) && _comparer3.Equals(x.Item3, y.Item3) && _comparer4.Equals(x.Item4, y.Item4) && _comparer5.Equals(x.Item5, y.Item5) && _comparer6.Equals(x.Item6, y.Item6) && _comparer7.Equals(x.Item7, y.Item7);
        }

        public int GetHashCode(Tuple<T1, T2, T3, T4, T5, T6, T7> obj)
        {
            if (obj == null)
                return 0;

            return HashHelpers.Combine(_comparer1.GetHashCode(obj.Item1), _comparer2.GetHashCode(obj.Item2), _comparer3.GetHashCode(obj.Item3), _comparer4.GetHashCode(obj.Item4), _comparer5.GetHashCode(obj.Item5), _comparer6.GetHashCode(obj.Item6), _comparer7.GetHashCode(obj.Item7));
        }

    }

}
