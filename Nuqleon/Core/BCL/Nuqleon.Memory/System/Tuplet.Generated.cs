// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Represents a 1-tuple value.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1> : ITuplet, IEquatable<Tuplet<T1>>, IComparable, IComparable<Tuplet<T1>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 1-tuple.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        public Tuplet(T1 item1)
        {
            Item1 = item1;
        }

        /// <summary>
        /// Gets the value of the tuple's first component.
        /// </summary>
        public T1 Item1 { get; }

        /// <summary>
        /// Gets a hash code for the current value.
        /// </summary>
        /// <returns>A hash code for the current value.</returns>
        public override int GetHashCode() =>
            FastEqualityComparer<T1>.Default.GetHashCode(Item1);

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1> && Equals((Tuplet<T1>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1> first, Tuplet<T1> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1> first, Tuplet<T1> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1> first, Tuplet<T1> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1> first, Tuplet<T1> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1> first, Tuplet<T1> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1> first, Tuplet<T1> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1>))
            {
                return false;
            }

            var args = (Tuplet<T1>)other;

            return comparer.Equals(Item1, args.Item1)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            return comparer.GetHashCode(Item1);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1>.", nameof(other));
            }

            var args = (Tuplet<T1>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 2-tuple value.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">Type of the tuple's second component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2> : ITuplet, IEquatable<Tuplet<T1, T2>>, IComparable, IComparable<Tuplet<T1, T2>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 2-tuple.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        /// <param name="item2">Value of the tuple's second component.</param>
        public Tuplet(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
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
        /// Gets a hash code for the current value.
        /// </summary>
        /// <returns>A hash code for the current value.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<T1>.Default.GetHashCode(Item1),
                FastEqualityComparer<T2>.Default.GetHashCode(Item2)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2> && Equals((Tuplet<T1, T2>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

            if (res != 0)
            {
                return res;
            }

            res = FastComparer<T2>.Default.Compare(Item2, other.Item2);

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2> first, Tuplet<T1, T2> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2> first, Tuplet<T1, T2> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2> first, Tuplet<T1, T2> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2> first, Tuplet<T1, T2> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2> first, Tuplet<T1, T2> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2> first, Tuplet<T1, T2> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            return
                HashHelpers.Combine(
                    comparer.GetHashCode(Item1),
                    comparer.GetHashCode(Item2)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2>.", nameof(other));
            }

            var args = (Tuplet<T1, T2>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 3-tuple value.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">Type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">Type of the tuple's third component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3> : ITuplet, IEquatable<Tuplet<T1, T2, T3>>, IComparable, IComparable<Tuplet<T1, T2, T3>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 3-tuple.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        /// <param name="item2">Value of the tuple's second component.</param>
        /// <param name="item3">Value of the tuple's third component.</param>
        public Tuplet(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
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
        /// Gets a hash code for the current value.
        /// </summary>
        /// <returns>A hash code for the current value.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<T1>.Default.GetHashCode(Item1),
                FastEqualityComparer<T2>.Default.GetHashCode(Item2),
                FastEqualityComparer<T3>.Default.GetHashCode(Item3)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3> && Equals((Tuplet<T1, T2, T3>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            && FastEqualityComparer<T3>.Default.Equals(Item3, other.Item3)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3> first, Tuplet<T1, T2, T3> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3> first, Tuplet<T1, T2, T3> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3> first, Tuplet<T1, T2, T3> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3> first, Tuplet<T1, T2, T3> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3> first, Tuplet<T1, T2, T3> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3> first, Tuplet<T1, T2, T3> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            return
                HashHelpers.Combine(
                    comparer.GetHashCode(Item1),
                    comparer.GetHashCode(Item2),
                    comparer.GetHashCode(Item3)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 4-tuple value.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">Type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">Type of the tuple's third component.</typeparam>
    /// <typeparam name="T4">Type of the tuple's fourth component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 4-tuple.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        /// <param name="item2">Value of the tuple's second component.</param>
        /// <param name="item3">Value of the tuple's third component.</param>
        /// <param name="item4">Value of the tuple's fourth component.</param>
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
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
        /// Gets a hash code for the current value.
        /// </summary>
        /// <returns>A hash code for the current value.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<T1>.Default.GetHashCode(Item1),
                FastEqualityComparer<T2>.Default.GetHashCode(Item2),
                FastEqualityComparer<T3>.Default.GetHashCode(Item3),
                FastEqualityComparer<T4>.Default.GetHashCode(Item4)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4> && Equals((Tuplet<T1, T2, T3, T4>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            && FastEqualityComparer<T3>.Default.Equals(Item3, other.Item3)
            && FastEqualityComparer<T4>.Default.Equals(Item4, other.Item4)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4> first, Tuplet<T1, T2, T3, T4> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4> first, Tuplet<T1, T2, T3, T4> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4> first, Tuplet<T1, T2, T3, T4> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4> first, Tuplet<T1, T2, T3, T4> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4> first, Tuplet<T1, T2, T3, T4> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4> first, Tuplet<T1, T2, T3, T4> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            return
                HashHelpers.Combine(
                    comparer.GetHashCode(Item1),
                    comparer.GetHashCode(Item2),
                    comparer.GetHashCode(Item3),
                    comparer.GetHashCode(Item4)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 5-tuple value.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">Type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">Type of the tuple's third component.</typeparam>
    /// <typeparam name="T4">Type of the tuple's fourth component.</typeparam>
    /// <typeparam name="T5">Type of the tuple's fifth component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 5-tuple.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        /// <param name="item2">Value of the tuple's second component.</param>
        /// <param name="item3">Value of the tuple's third component.</param>
        /// <param name="item4">Value of the tuple's fourth component.</param>
        /// <param name="item5">Value of the tuple's fifth component.</param>
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
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
        /// Gets a hash code for the current value.
        /// </summary>
        /// <returns>A hash code for the current value.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<T1>.Default.GetHashCode(Item1),
                FastEqualityComparer<T2>.Default.GetHashCode(Item2),
                FastEqualityComparer<T3>.Default.GetHashCode(Item3),
                FastEqualityComparer<T4>.Default.GetHashCode(Item4),
                FastEqualityComparer<T5>.Default.GetHashCode(Item5)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5> && Equals((Tuplet<T1, T2, T3, T4, T5>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            && FastEqualityComparer<T3>.Default.Equals(Item3, other.Item3)
            && FastEqualityComparer<T4>.Default.Equals(Item4, other.Item4)
            && FastEqualityComparer<T5>.Default.Equals(Item5, other.Item5)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5> first, Tuplet<T1, T2, T3, T4, T5> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5> first, Tuplet<T1, T2, T3, T4, T5> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5> first, Tuplet<T1, T2, T3, T4, T5> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5> first, Tuplet<T1, T2, T3, T4, T5> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5> first, Tuplet<T1, T2, T3, T4, T5> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5> first, Tuplet<T1, T2, T3, T4, T5> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            return
                HashHelpers.Combine(
                    comparer.GetHashCode(Item1),
                    comparer.GetHashCode(Item2),
                    comparer.GetHashCode(Item3),
                    comparer.GetHashCode(Item4),
                    comparer.GetHashCode(Item5)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 6-tuple value.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">Type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">Type of the tuple's third component.</typeparam>
    /// <typeparam name="T4">Type of the tuple's fourth component.</typeparam>
    /// <typeparam name="T5">Type of the tuple's fifth component.</typeparam>
    /// <typeparam name="T6">Type of the tuple's sixth component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 6-tuple.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        /// <param name="item2">Value of the tuple's second component.</param>
        /// <param name="item3">Value of the tuple's third component.</param>
        /// <param name="item4">Value of the tuple's fourth component.</param>
        /// <param name="item5">Value of the tuple's fifth component.</param>
        /// <param name="item6">Value of the tuple's sixth component.</param>
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
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
                FastEqualityComparer<T6>.Default.GetHashCode(Item6)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6> && Equals((Tuplet<T1, T2, T3, T4, T5, T6>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            && FastEqualityComparer<T3>.Default.Equals(Item3, other.Item3)
            && FastEqualityComparer<T4>.Default.Equals(Item4, other.Item4)
            && FastEqualityComparer<T5>.Default.Equals(Item5, other.Item5)
            && FastEqualityComparer<T6>.Default.Equals(Item6, other.Item6)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6> first, Tuplet<T1, T2, T3, T4, T5, T6> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6> first, Tuplet<T1, T2, T3, T4, T5, T6> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6> first, Tuplet<T1, T2, T3, T4, T5, T6> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6> first, Tuplet<T1, T2, T3, T4, T5, T6> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6> first, Tuplet<T1, T2, T3, T4, T5, T6> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6> first, Tuplet<T1, T2, T3, T4, T5, T6> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            return
                HashHelpers.Combine(
                    comparer.GetHashCode(Item1),
                    comparer.GetHashCode(Item2),
                    comparer.GetHashCode(Item3),
                    comparer.GetHashCode(Item4),
                    comparer.GetHashCode(Item5),
                    comparer.GetHashCode(Item6)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 7-tuple value.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">Type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">Type of the tuple's third component.</typeparam>
    /// <typeparam name="T4">Type of the tuple's fourth component.</typeparam>
    /// <typeparam name="T5">Type of the tuple's fifth component.</typeparam>
    /// <typeparam name="T6">Type of the tuple's sixth component.</typeparam>
    /// <typeparam name="T7">Type of the tuple's seventh component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 7-tuple.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        /// <param name="item2">Value of the tuple's second component.</param>
        /// <param name="item3">Value of the tuple's third component.</param>
        /// <param name="item4">Value of the tuple's fourth component.</param>
        /// <param name="item5">Value of the tuple's fifth component.</param>
        /// <param name="item6">Value of the tuple's sixth component.</param>
        /// <param name="item7">Value of the tuple's seventh component.</param>
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
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
                FastEqualityComparer<T7>.Default.GetHashCode(Item7)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            && FastEqualityComparer<T3>.Default.Equals(Item3, other.Item3)
            && FastEqualityComparer<T4>.Default.Equals(Item4, other.Item4)
            && FastEqualityComparer<T5>.Default.Equals(Item5, other.Item5)
            && FastEqualityComparer<T6>.Default.Equals(Item6, other.Item6)
            && FastEqualityComparer<T7>.Default.Equals(Item7, other.Item7)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7> first, Tuplet<T1, T2, T3, T4, T5, T6, T7> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7> first, Tuplet<T1, T2, T3, T4, T5, T6, T7> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7> first, Tuplet<T1, T2, T3, T4, T5, T6, T7> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7> first, Tuplet<T1, T2, T3, T4, T5, T6, T7> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7> first, Tuplet<T1, T2, T3, T4, T5, T6, T7> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7> first, Tuplet<T1, T2, T3, T4, T5, T6, T7> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            return
                HashHelpers.Combine(
                    comparer.GetHashCode(Item1),
                    comparer.GetHashCode(Item2),
                    comparer.GetHashCode(Item3),
                    comparer.GetHashCode(Item4),
                    comparer.GetHashCode(Item5),
                    comparer.GetHashCode(Item6),
                    comparer.GetHashCode(Item7)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 8-tuple value.
    /// </summary>
    /// <typeparam name="T1">Type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">Type of the tuple's second component.</typeparam>
    /// <typeparam name="T3">Type of the tuple's third component.</typeparam>
    /// <typeparam name="T4">Type of the tuple's fourth component.</typeparam>
    /// <typeparam name="T5">Type of the tuple's fifth component.</typeparam>
    /// <typeparam name="T6">Type of the tuple's sixth component.</typeparam>
    /// <typeparam name="T7">Type of the tuple's seventh component.</typeparam>
    /// <typeparam name="T8">Type of the tuple's eighth component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 8-tuple.
        /// </summary>
        /// <param name="item1">Value of the tuple's first component.</param>
        /// <param name="item2">Value of the tuple's second component.</param>
        /// <param name="item3">Value of the tuple's third component.</param>
        /// <param name="item4">Value of the tuple's fourth component.</param>
        /// <param name="item5">Value of the tuple's fifth component.</param>
        /// <param name="item6">Value of the tuple's sixth component.</param>
        /// <param name="item7">Value of the tuple's seventh component.</param>
        /// <param name="item8">Value of the tuple's eighth component.</param>
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
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
                FastEqualityComparer<T8>.Default.GetHashCode(Item8)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            && FastEqualityComparer<T3>.Default.Equals(Item3, other.Item3)
            && FastEqualityComparer<T4>.Default.Equals(Item4, other.Item4)
            && FastEqualityComparer<T5>.Default.Equals(Item5, other.Item5)
            && FastEqualityComparer<T6>.Default.Equals(Item6, other.Item6)
            && FastEqualityComparer<T7>.Default.Equals(Item7, other.Item7)
            && FastEqualityComparer<T8>.Default.Equals(Item8, other.Item8)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            return
                HashHelpers.Combine(
                    comparer.GetHashCode(Item1),
                    comparer.GetHashCode(Item2),
                    comparer.GetHashCode(Item3),
                    comparer.GetHashCode(Item4),
                    comparer.GetHashCode(Item5),
                    comparer.GetHashCode(Item6),
                    comparer.GetHashCode(Item7),
                    comparer.GetHashCode(Item8)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 9-tuple value.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 9-tuple.
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
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            Item9 = item9;
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
                FastEqualityComparer<T9>.Default.GetHashCode(Item9)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> other) =>
               FastEqualityComparer<T1>.Default.Equals(Item1, other.Item1)
            && FastEqualityComparer<T2>.Default.Equals(Item2, other.Item2)
            && FastEqualityComparer<T3>.Default.Equals(Item3, other.Item3)
            && FastEqualityComparer<T4>.Default.Equals(Item4, other.Item4)
            && FastEqualityComparer<T5>.Default.Equals(Item5, other.Item5)
            && FastEqualityComparer<T6>.Default.Equals(Item6, other.Item6)
            && FastEqualityComparer<T7>.Default.Equals(Item7, other.Item7)
            && FastEqualityComparer<T8>.Default.Equals(Item8, other.Item8)
            && FastEqualityComparer<T9>.Default.Equals(Item9, other.Item9)
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                && comparer.Equals(Item9, args.Item9)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

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
                    comparer.GetHashCode(Item9)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, args.Item9);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 10-tuple value.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 10-tuple.
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
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
        {
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
                FastEqualityComparer<T10>.Default.GetHashCode(Item10)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other) =>
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
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                && comparer.Equals(Item9, args.Item9)
                && comparer.Equals(Item10, args.Item10)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

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
                    comparer.GetHashCode(Item10)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, args.Item9);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item10, args.Item10);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 11-tuple value.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 11-tuple.
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
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11)
        {
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
                FastEqualityComparer<T11>.Default.GetHashCode(Item11)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other) =>
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
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                && comparer.Equals(Item9, args.Item9)
                && comparer.Equals(Item10, args.Item10)
                && comparer.Equals(Item11, args.Item11)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

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
                    comparer.GetHashCode(Item11)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, args.Item9);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item10, args.Item10);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item11, args.Item11);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 12-tuple value.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 12-tuple.
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
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12)
        {
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
                FastEqualityComparer<T12>.Default.GetHashCode(Item12)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other) =>
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
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                && comparer.Equals(Item9, args.Item9)
                && comparer.Equals(Item10, args.Item10)
                && comparer.Equals(Item11, args.Item11)
                && comparer.Equals(Item12, args.Item12)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

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
                    comparer.GetHashCode(Item12)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, args.Item9);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item10, args.Item10);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item11, args.Item11);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item12, args.Item12);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 13-tuple value.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 13-tuple.
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
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13)
        {
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
                FastEqualityComparer<T13>.Default.GetHashCode(Item13)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other) =>
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
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                && comparer.Equals(Item9, args.Item9)
                && comparer.Equals(Item10, args.Item10)
                && comparer.Equals(Item11, args.Item11)
                && comparer.Equals(Item12, args.Item12)
                && comparer.Equals(Item13, args.Item13)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

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
                    comparer.GetHashCode(Item13)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, args.Item9);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item10, args.Item10);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item11, args.Item11);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item12, args.Item12);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item13, args.Item13);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 14-tuple value.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 14-tuple.
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
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14)
        {
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
                FastEqualityComparer<T14>.Default.GetHashCode(Item14)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> other) =>
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
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                && comparer.Equals(Item9, args.Item9)
                && comparer.Equals(Item10, args.Item10)
                && comparer.Equals(Item11, args.Item11)
                && comparer.Equals(Item12, args.Item12)
                && comparer.Equals(Item13, args.Item13)
                && comparer.Equals(Item14, args.Item14)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

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
                    comparer.GetHashCode(Item14)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, args.Item9);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item10, args.Item10);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item11, args.Item11);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item12, args.Item12);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item13, args.Item13);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item14, args.Item14);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 15-tuple value.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 15-tuple.
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
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15)
        {
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
                FastEqualityComparer<T15>.Default.GetHashCode(Item15)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> other) =>
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
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ", " + Item15?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ", " + Item15?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                && comparer.Equals(Item9, args.Item9)
                && comparer.Equals(Item10, args.Item10)
                && comparer.Equals(Item11, args.Item11)
                && comparer.Equals(Item12, args.Item12)
                && comparer.Equals(Item13, args.Item13)
                && comparer.Equals(Item14, args.Item14)
                && comparer.Equals(Item15, args.Item15)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

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
                    comparer.GetHashCode(Item15)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, args.Item9);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item10, args.Item10);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item11, args.Item11);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item12, args.Item12);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item13, args.Item13);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item14, args.Item14);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item15, args.Item15);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

    /// <summary>
    /// Represents a 16-tuple value.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tuplet", Justification = "Tuplets are low-footprint tuples, i.e. ones that are stored as values.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Comparable to Tuple design.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "IComparable support mainly for structural comparison purposes; no way/use to make/making operator overloads private; similar to Tuple design.")]
    public struct Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : ITuplet, IEquatable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>, IComparable, IComparable<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>, IStructuralEquatable, IStructuralComparable
    {
        /// <summary>
        /// Creates a value representing a 16-tuple.
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
        public Tuplet(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16)
        {
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
                FastEqualityComparer<T16>.Default.GetHashCode(Item16)
            );

        /// <summary>
        /// Checks whether the current value is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> && Equals((Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>)obj);

        /// <summary>
        /// Checks whether the current value is equal to the specified value.
        /// </summary>
        /// <param name="other">The value to compare the current value against.</param>
        /// <returns>true if the current value is equal to the specified value; otherwise, false.</returns>
        public bool Equals(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> other) =>
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
            ;

        /// <summary>
        /// Compares the current value with another value and returns an integer that indicates whether the current value precedes, follows, or occurs in the same position in the sort order as the other value.
        /// </summary>
        /// <param name="other">A value to compare with this value.</param>
        /// <returns>A value that indicates the relative order of the values being compared.</returns>
        public int CompareTo(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> other)
        {
            var res = 0;

            res = FastComparer<T1>.Default.Compare(Item1, other.Item1);

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

            return res;
        }

        /// <summary>
        /// Checks whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are equal; otherwise, false.</returns>
        public static bool operator ==(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> second) => first.Equals(second);

        /// <summary>
        /// Checks whether the specified values are not equal.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the values are not equal; otherwise, false.</returns>
        public static bool operator !=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> second) => !first.Equals(second);

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than the second value; otherwise, false.</returns>
        public static bool operator <(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> second) => first.CompareTo(second) < 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is less than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is less than or equal to the second value; otherwise, false.</returns>
        public static bool operator <=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> second) => first.CompareTo(second) <= 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than the second value; otherwise, false.</returns>
        public static bool operator >(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> second) => first.CompareTo(second) > 0;

        /// <summary>
        /// Checks whether the <paramref name="first"/> value is larger than or equal to the <paramref name="second"/> value.
        /// </summary>
        /// <param name="first">The first value to compare.</param>
        /// <param name="second">The second value to compare.</param>
        /// <returns>true if the first value is larger than or equal to the second value; otherwise, false.</returns>
        public static bool operator >=(Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> first, Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> second) => first.CompareTo(second) >= 0;

        /// <summary>
        /// Gets a string representation of the current value.
        /// </summary>
        /// <returns>A string representation for the current value.</returns>
        public override string ToString() => "(" + Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ", " + Item15?.ToString() + ", " + Item16?.ToString() + ")";

        string ITuplet.ToStringEnd() => Item1?.ToString() + ", " + Item2?.ToString() + ", " + Item3?.ToString() + ", " + Item4?.ToString() + ", " + Item5?.ToString() + ", " + Item6?.ToString() + ", " + Item7?.ToString() + ", " + Item8?.ToString() + ", " + Item9?.ToString() + ", " + Item10?.ToString() + ", " + Item11?.ToString() + ", " + Item12?.ToString() + ", " + Item13?.ToString() + ", " + Item14?.ToString() + ", " + Item15?.ToString() + ", " + Item16?.ToString() + ")";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>))
            {
                return false;
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>)other;

            return comparer.Equals(Item1, args.Item1)
                && comparer.Equals(Item2, args.Item2)
                && comparer.Equals(Item3, args.Item3)
                && comparer.Equals(Item4, args.Item4)
                && comparer.Equals(Item5, args.Item5)
                && comparer.Equals(Item6, args.Item6)
                && comparer.Equals(Item7, args.Item7)
                && comparer.Equals(Item8, args.Item8)
                && comparer.Equals(Item9, args.Item9)
                && comparer.Equals(Item10, args.Item10)
                && comparer.Equals(Item11, args.Item11)
                && comparer.Equals(Item12, args.Item12)
                && comparer.Equals(Item13, args.Item13)
                && comparer.Equals(Item14, args.Item14)
                && comparer.Equals(Item15, args.Item15)
                && comparer.Equals(Item16, args.Item16)
                ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

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
                    comparer.GetHashCode(Item16)
                );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Errors.ThrowArgumentNull does the check.")]
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            Errors.ThrowArgumentNull(comparer, nameof(comparer));

            if (other == null)
            {
                return 1;
            }

            if (!(other is Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>))
            {
                throw new ArgumentException("The specified argument is not of type Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>.", nameof(other));
            }

            var args = (Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>)other;

            var res = 0;

            res = comparer.Compare(Item1, args.Item1);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item2, args.Item2);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item3, args.Item3);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item4, args.Item4);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item5, args.Item5);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item6, args.Item6);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item7, args.Item7);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item8, args.Item8);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item9, args.Item9);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item10, args.Item10);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item11, args.Item11);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item12, args.Item12);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item13, args.Item13);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item14, args.Item14);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item15, args.Item15);

            if (res != 0)
            {
                return res;
            }

            res = comparer.Compare(Item16, args.Item16);

            return res;
        }

        int IComparable.CompareTo(object obj) => ((IStructuralComparable)this).CompareTo(obj, FastComparer<object>.Default);
    }

}
