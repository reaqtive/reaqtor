// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Created this type.
//

using System.Collections;
using System.Collections.Generic;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1508 // False positive on Item1?.ToString() usage. These can be null.

namespace System
{
    /// <summary>
    /// Represents an n-tuple, where n is 17 or greater.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">Type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">Type of the tuple's third component.</typeparam>
    /// <typeparam name="T4">Type of the tuple's fourth component.</typeparam>
    /// <typeparam name="T5">Type of the tuple's fifth component.</typeparam>
    /// <typeparam name="T6">Type of the tuple's sixth component.</typeparam>
    /// <typeparam name="T7">Type of the tuple's seventh component.</typeparam>
    /// <typeparam name="T8">Type of the tuple's eighth component.</typeparam>
    /// <typeparam name="T9">Type of the tuple's ninth component.</typeparam>
    /// <typeparam name="T10">Type of the tuple's tenth component.</typeparam>
    /// <typeparam name="T11">Type of the tuple's eleventh component.</typeparam>
    /// <typeparam name="T12">Type of the tuple's twelfth component.</typeparam>
    /// <typeparam name="T13">Type of the tuple's thirteenth component.</typeparam>
    /// <typeparam name="T14">Type of the tuple's fourteenth component.</typeparam>
    /// <typeparam name="T15">Type of the tuple's fifteenth component.</typeparam>
    /// <typeparam name="T16">Type of the tuple's sixteenth component.</typeparam>
    /// <typeparam name="TRest">Any generic Tuplet object that defines the types of the tuple's remaining components.</typeparam>
    public readonly struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing an n-tuple, where n is 17 or greater.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        /// <param name="item2">Value of the tuple's second component.</param>
        /// <param name="item3">Value of the tuple's third component.</param>
        /// <param name="item4">Value of the tuple's fourth component.</param>
        /// <param name="item5">Value of the tuple's fifth component.</param>
        /// <param name="item6">Value of the tuple's sixth component.</param>
        /// <param name="item7">Value of the tuple's seventh component.</param>
        /// <param name="item8">Value of the tuple's eighth component.</param>
        /// <param name="item9">Value of the tuple's ninth component.</param>
        /// <param name="item10">Value of the tuple's tenth component.</param>
        /// <param name="item11">Value of the tuple's eleventh component.</param>
        /// <param name="item12">Value of the tuple's twelfth component.</param>
        /// <param name="item13">Value of the tuple's thirteenth component.</param>
        /// <param name="item14">Value of the tuple's fourteenth component.</param>
        /// <param name="item15">Value of the tuple's fifteenth component.</param>
        /// <param name="item16">Value of the tuple's sixteenth component.</param>
        /// <param name="rest">Value of the tuple's remainder components.</param>
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16, TRest rest)
        {
            if (rest is not ITuplet)
                throw new ArgumentException("Remainder function arguments should be an Tuplet type.", nameof(rest));

            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
            Item10 = item10;
            Item11 = item11;
            Item12 = item12;
            Item13 = item13;
            Item14 = item14;
            Item15 = item15;
            Item16 = item16;
            Rest = rest;
        }

        /// <summary>
        /// Gets the value of the tuple's first component.
        /// </summary>
        public T1 Item1 { get; }

        /// <summary>
        /// Gets the value of the tuple's second component.
        /// </summary>
        public T2 Item2 { get; }

        /// <summary>
        /// Gets the value of the tuple's third component.
        /// </summary>
        public T3 Item3 { get; }

        /// <summary>
        /// Gets the value of the tuple's fourth component.
        /// </summary>
        public T4 Item4 { get; }

        /// <summary>
        /// Gets the value of the tuple's fifth component.
        /// </summary>
        public T5 Item5 { get; }

        /// <summary>
        /// Gets the value of the tuple's sixth component.
        /// </summary>
        public T6 Item6 { get; }

        /// <summary>
        /// Gets the value of the tuple's seventh component.
        /// </summary>
        public T7 Item7 { get; }

        /// <summary>
        /// Gets the value of the tuple's eighth component.
        /// </summary>
        public T8 Item8 { get; }

        /// <summary>
        /// Gets the value of the tuple's ninth component.
        /// </summary>
        public T9 Item9 { get; }

        /// <summary>
        /// Gets the value of the tuple's tenth component.
        /// </summary>
        public T10 Item10 { get; }

        /// <summary>
        /// Gets the value of the tuple's eleventh component.
        /// </summary>
        public T11 Item11 { get; }

        /// <summary>
        /// Gets the value of the tuple's twelfth component.
        /// </summary>
        public T12 Item12 { get; }

        /// <summary>
        /// Gets the value of the tuple's thirteenth component.
        /// </summary>
        public T13 Item13 { get; }

        /// <summary>
        /// Gets the value of the tuple's fourteenth component.
        /// </summary>
        public T14 Item14 { get; }

        /// <summary>
        /// Gets the value of the tuple's fifteenth component.
        /// </summary>
        public T15 Item15 { get; }

        /// <summary>
        /// Gets the value of the tuple's sixteenth component.
        /// </summary>
        public T16 Item16 { get; }

        /// <summary>
        /// Gets the value of the tuple's remainder components.
        /// </summary>
        public TRest Rest { get; }

        /// <summary>
        /// Gets a hash code for the current value.
        /// </summary>
        /// <returns>A hash code for the current value.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<T1>.Default.GetHashCode(Item1),
                FastEqualityComparer<T2>.Default.GetHashCode(Item2),
                FastEqualityComparer<T3>.Default.GetHashCode(Item3),
                FastEqualityComparer<T4>.Default.GetHashCode(Item4),
                FastEqualityComparer<T5>.Default.GetHashCode(Item5),
                FastEqualityComparer<T6>.Default.GetHashCode(Item6),
                FastEqualityComparer<T7>.Default.GetHashCode(Item7),
                FastEqualityComparer<T8>.Default.GetHashCode(Item8),
                FastEqualityComparer<T9>.Default.GetHashCode(Item9),
                FastEqualityComparer<T10>.Default.GetHashCode(Item10),
                FastEqualityComparer<T11>.Default.GetHashCode(Item11),
                FastEqualityComparer<T12>.Default.GetHashCode(Item12),
                FastEqualityComparer<T13>.Default.GetHashCode(Item13),
                FastEqualityComparer<T14>.Default.GetHashCode(Item14),
                FastEqualityComparer<T15>.Default.GetHashCode(Item15),
                FastEqualityComparer<T16>.Default.GetHashCode(Item16),
                FastEqualityComparer<TRest>.Default.GetHashCode(Rest));

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) =>
            obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> tuplet &&
            Equals(tuplet);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            && FastEqualityComparer<T3>.Default.Equals(Item3, other.Item3)
            && FastEqualityComparer<T4>.Default.Equals(Item4, other.Item4)
            && FastEqualityComparer<T5>.Default.Equals(Item5, other.Item5)
            && FastEqualityComparer<T6>.Default.Equals(Item6, other.Item6)
            && FastEqualityComparer<T7>.Default.Equals(Item7, other.Item7)
            && FastEqualityComparer<T8>.Default.Equals(Item8, other.Item8)
            && FastEqualityComparer<T9>.Default.Equals(Item9, other.Item9)
            && FastEqualityComparer<T10>.Default.Equals(Item10, other.Item10)
            && FastEqualityComparer<T11>.Default.Equals(Item11, other.Item11)
            && FastEqualityComparer<T12>.Default.Equals(Item12, other.Item12)
            && FastEqualityComparer<T13>.Default.Equals(Item13, other.Item13)
            && FastEqualityComparer<T14>.Default.Equals(Item14, other.Item14)
            && FastEqualityComparer<T15>.Default.Equals(Item15, other.Item15)
            && FastEqualityComparer<T16>.Default.Equals(Item16, other.Item16)
            && FastEqualityComparer<TRest>.Default.Equals(Rest, other.Rest)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> other)
        {
            var res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T2>.Default.Compare(Item2, other.Item2);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T3>.Default.Compare(Item3, other.Item3);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T4>.Default.Compare(Item4, other.Item4);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T5>.Default.Compare(Item5, other.Item5);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T6>.Default.Compare(Item6, other.Item6);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T7>.Default.Compare(Item7, other.Item7);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T8>.Default.Compare(Item8, other.Item8);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T9>.Default.Compare(Item9, other.Item9);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T10>.Default.Compare(Item10, other.Item10);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T11>.Default.Compare(Item11, other.Item11);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T12>.Default.Compare(Item12, other.Item12);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T13>.Default.Compare(Item13, other.Item13);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T14>.Default.Compare(Item14, other.Item14);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T15>.Default.Compare(Item15, other.Item15);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T16>.Default.Compare(Item16, other.Item16);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<TRest>.Default.Compare(Rest, other.Rest);

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => Rest is ITuplet rest
                ? "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ", " + Item15?.ToString() + ", " + Item16?.ToString() + ", " + rest.ToStringEnd()
                : "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ", " + Item15?.ToString() + ", " + Item16?.ToString() + ", " + Rest.ToString() + ")";

        string ITuplet.ToStringEnd() => Rest is ITuplet rest
                ? Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ", " + Item15?.ToString() + ", " + Item16?.ToString() + ", " + rest.ToStringEnd()
                : Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ", " + Item15?.ToString() + ", " + Item16?.ToString() + ", " + Rest.ToString() + ")";

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other is not Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest>)
            {
                return false;
            }

            var tuplet = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest>)other;

            return comparer.Equals(Item1, tuplet.Item1)
                && comparer.Equals(Item2, tuplet.Item2)
                && comparer.Equals(Item3, tuplet.Item3)
                && comparer.Equals(Item4, tuplet.Item4)
                && comparer.Equals(Item5, tuplet.Item5)
                && comparer.Equals(Item6, tuplet.Item6)
                && comparer.Equals(Item7, tuplet.Item7)
                && comparer.Equals(Item8, tuplet.Item8)
                && comparer.Equals(Item9, tuplet.Item9)
                && comparer.Equals(Item10, tuplet.Item10)
                && comparer.Equals(Item11, tuplet.Item11)
                && comparer.Equals(Item12, tuplet.Item12)
                && comparer.Equals(Item13, tuplet.Item13)
                && comparer.Equals(Item14, tuplet.Item14)
                && comparer.Equals(Item15, tuplet.Item15)
                && comparer.Equals(Item16, tuplet.Item16)
                && comparer.Equals(Rest, tuplet.Rest)
                ;
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            // NB: Tuple<T1,...,T7,TRest> does only use the last 8 elements of a tuple to compute
            //     the hash, likely to speed up the process. We may want to evaluate whether we
            //     want to prune the hash calculation too.

            return
                HashHelpers.Combine(
                    comparer.GetHashCode(Item1),
                    comparer.GetHashCode(Item2),
                    comparer.GetHashCode(Item3),
                    comparer.GetHashCode(Item4),
                    comparer.GetHashCode(Item5),
                    comparer.GetHashCode(Item6),
                    comparer.GetHashCode(Item7),
                    comparer.GetHashCode(Item8),
                    comparer.GetHashCode(Item9),
                    comparer.GetHashCode(Item10),
                    comparer.GetHashCode(Item11),
                    comparer.GetHashCode(Item12),
                    comparer.GetHashCode(Item13),
                    comparer.GetHashCode(Item14),
                    comparer.GetHashCode(Item15),
                    comparer.GetHashCode(Item16),
                    comparer.GetHashCode(Rest));
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (other is not Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest>)
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest>.", nameof(other));
            }

            var tuplet = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRest>)other;

            var res = comparer.Compare(Item1, tuplet.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, tuplet.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, tuplet.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, tuplet.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, tuplet.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, tuplet.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, tuplet.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, tuplet.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, tuplet.Item9);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item10, tuplet.Item10);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item11, tuplet.Item11);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item12, tuplet.Item12);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item13, tuplet.Item13);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item14, tuplet.Item14);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item15, tuplet.Item15);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item16, tuplet.Item16);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Rest, tuplet.Rest);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }
}
