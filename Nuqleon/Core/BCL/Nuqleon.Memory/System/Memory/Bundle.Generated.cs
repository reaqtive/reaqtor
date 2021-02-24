// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Memory
{
    /// <summary>
    /// Provides a set of factory methods to create bundles of objects which are stored in a memory-efficient manner.
    /// </summary>
    public partial class Bundle
    {
        /// <summary>
        /// Creates a bundle with one item.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1>(T1 item1) => new Bundle<T1>(item1);

        /// <summary>
        /// Creates a bundle with two items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2>(T1 item1, T2 item2) => new Bundle<T1, T2>(item1, item2);

        /// <summary>
        /// Creates a bundle with three items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) => new Bundle<T1, T2, T3>(item1, item2, item3);

        /// <summary>
        /// Creates a bundle with four items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) => new Bundle<T1, T2, T3, T4>(item1, item2, item3, item4);

        /// <summary>
        /// Creates a bundle with five items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) => new Bundle<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);

        /// <summary>
        /// Creates a bundle with six items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) => new Bundle<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);

        /// <summary>
        /// Creates a bundle with seven items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) => new Bundle<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);

        /// <summary>
        /// Creates a bundle with eight items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8);

        /// <summary>
        /// Creates a bundle with nine items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <param name="item9">The ninth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(item1, item2, item3, item4, item5, item6, item7, item8, item9);

        /// <summary>
        /// Creates a bundle with ten items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <param name="item9">The ninth item in the bundle.</param>
        /// <param name="item10">The tenth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10);

        /// <summary>
        /// Creates a bundle with eleven items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <param name="item9">The ninth item in the bundle.</param>
        /// <param name="item10">The tenth item in the bundle.</param>
        /// <param name="item11">The eleventh item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11);

        /// <summary>
        /// Creates a bundle with twelve items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <param name="item9">The ninth item in the bundle.</param>
        /// <param name="item10">The tenth item in the bundle.</param>
        /// <param name="item11">The eleventh item in the bundle.</param>
        /// <param name="item12">The twelfth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12);

        /// <summary>
        /// Creates a bundle with thirteen items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <param name="item9">The ninth item in the bundle.</param>
        /// <param name="item10">The tenth item in the bundle.</param>
        /// <param name="item11">The eleventh item in the bundle.</param>
        /// <param name="item12">The twelfth item in the bundle.</param>
        /// <param name="item13">The thirteenth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13);

        /// <summary>
        /// Creates a bundle with fourteen items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <param name="item9">The ninth item in the bundle.</param>
        /// <param name="item10">The tenth item in the bundle.</param>
        /// <param name="item11">The eleventh item in the bundle.</param>
        /// <param name="item12">The twelfth item in the bundle.</param>
        /// <param name="item13">The thirteenth item in the bundle.</param>
        /// <param name="item14">The fourteenth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14);

        /// <summary>
        /// Creates a bundle with fifteen items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <param name="item9">The ninth item in the bundle.</param>
        /// <param name="item10">The tenth item in the bundle.</param>
        /// <param name="item11">The eleventh item in the bundle.</param>
        /// <param name="item12">The twelfth item in the bundle.</param>
        /// <param name="item13">The thirteenth item in the bundle.</param>
        /// <param name="item14">The fourteenth item in the bundle.</param>
        /// <param name="item15">The fifteenth item in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15);

        /// <summary>
        /// Creates a bundle with fifteen items and a bundle with remainder items.
        /// </summary>
        /// <param name="item1">The first item in the bundle.</param>
        /// <param name="item2">The second item in the bundle.</param>
        /// <param name="item3">The third item in the bundle.</param>
        /// <param name="item4">The fourth item in the bundle.</param>
        /// <param name="item5">The fifth item in the bundle.</param>
        /// <param name="item6">The sixth item in the bundle.</param>
        /// <param name="item7">The seventh item in the bundle.</param>
        /// <param name="item8">The eighth item in the bundle.</param>
        /// <param name="item9">The ninth item in the bundle.</param>
        /// <param name="item10">The tenth item in the bundle.</param>
        /// <param name="item11">The eleventh item in the bundle.</param>
        /// <param name="item12">The twelfth item in the bundle.</param>
        /// <param name="item13">The thirteenth item in the bundle.</param>
        /// <param name="item14">The fourteenth item in the bundle.</param>
        /// <param name="item15">The fifteenth item in the bundle.</param>
        /// <param name="rest">The remainder items in the bundle.</param>
        /// <returns>Bundle value with the specified items.</returns>
        public static IReadOnlyIndexed Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, TRest rest) => new Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRest>(item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12, item13, item14, item15, rest);

    }

    partial struct Bundle<T1> : IReadOnlyIndexed
    {
        private readonly T1 _item1;

        public Bundle(T1 item1)
        {
            _item1 = item1;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;

        public Bundle(T1 item1, T2 item2)
        {
            _item1 = item1;
            _item2 = item2;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;

        public Bundle(T1 item1, T2 item2, T3 item3)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;
        private readonly T9 _item9;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
            _item9 = item9;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    case 8:
                        return _item9;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;
        private readonly T9 _item9;
        private readonly T10 _item10;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
            _item9 = item9;
            _item10 = item10;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    case 8:
                        return _item9;
                    case 9:
                        return _item10;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;
        private readonly T9 _item9;
        private readonly T10 _item10;
        private readonly T11 _item11;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
            _item9 = item9;
            _item10 = item10;
            _item11 = item11;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    case 8:
                        return _item9;
                    case 9:
                        return _item10;
                    case 10:
                        return _item11;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;
        private readonly T9 _item9;
        private readonly T10 _item10;
        private readonly T11 _item11;
        private readonly T12 _item12;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
            _item9 = item9;
            _item10 = item10;
            _item11 = item11;
            _item12 = item12;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    case 8:
                        return _item9;
                    case 9:
                        return _item10;
                    case 10:
                        return _item11;
                    case 11:
                        return _item12;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;
        private readonly T9 _item9;
        private readonly T10 _item10;
        private readonly T11 _item11;
        private readonly T12 _item12;
        private readonly T13 _item13;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
            _item9 = item9;
            _item10 = item10;
            _item11 = item11;
            _item12 = item12;
            _item13 = item13;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    case 8:
                        return _item9;
                    case 9:
                        return _item10;
                    case 10:
                        return _item11;
                    case 11:
                        return _item12;
                    case 12:
                        return _item13;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;
        private readonly T9 _item9;
        private readonly T10 _item10;
        private readonly T11 _item11;
        private readonly T12 _item12;
        private readonly T13 _item13;
        private readonly T14 _item14;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
            _item9 = item9;
            _item10 = item10;
            _item11 = item11;
            _item12 = item12;
            _item13 = item13;
            _item14 = item14;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    case 8:
                        return _item9;
                    case 9:
                        return _item10;
                    case 10:
                        return _item11;
                    case 11:
                        return _item12;
                    case 12:
                        return _item13;
                    case 13:
                        return _item14;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;
        private readonly T9 _item9;
        private readonly T10 _item10;
        private readonly T11 _item11;
        private readonly T12 _item12;
        private readonly T13 _item13;
        private readonly T14 _item14;
        private readonly T15 _item15;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
            _item9 = item9;
            _item10 = item10;
            _item11 = item11;
            _item12 = item12;
            _item13 = item13;
            _item14 = item14;
            _item15 = item15;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    case 8:
                        return _item9;
                    case 9:
                        return _item10;
                    case 10:
                        return _item11;
                    case 11:
                        return _item12;
                    case 12:
                        return _item13;
                    case 13:
                        return _item14;
                    case 14:
                        return _item15;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }
    }

    partial struct Bundle<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRest> : IReadOnlyIndexed
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;
        private readonly T6 _item6;
        private readonly T7 _item7;
        private readonly T8 _item8;
        private readonly T9 _item9;
        private readonly T10 _item10;
        private readonly T11 _item11;
        private readonly T12 _item12;
        private readonly T13 _item13;
        private readonly T14 _item14;
        private readonly T15 _item15;
        private readonly TRest _rest;

        public Bundle(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, TRest rest)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _item8 = item8;
            _item9 = item9;
            _item10 = item10;
            _item11 = item11;
            _item12 = item12;
            _item13 = item13;
            _item14 = item14;
            _item15 = item15;
            _rest = rest;
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _item1;
                    case 1:
                        return _item2;
                    case 2:
                        return _item3;
                    case 3:
                        return _item4;
                    case 4:
                        return _item5;
                    case 5:
                        return _item6;
                    case 6:
                        return _item7;
                    case 7:
                        return _item8;
                    case 8:
                        return _item9;
                    case 9:
                        return _item10;
                    case 10:
                        return _item11;
                    case 11:
                        return _item12;
                    case 12:
                        return _item13;
                    case 13:
                        return _item14;
                    case 14:
                        return _item15;
                    default:
                        var rest = (IReadOnlyIndexed)_rest;
                        return rest[index - 15];
                }
            }
        }
    }

}
