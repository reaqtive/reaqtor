// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2014 - Created this file.
//

using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Nuqleon.Json.Expressions
{
    public partial class Expression
    {
        /// <summary>
        /// Creates an expression tree node representing an array with one element.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(Expression element1)
        {
            if (element1 == null)
                throw new ArgumentNullException(nameof(element1));

            return new ArrayExpression1(element1);
        }

        /// <summary>
        /// Creates an expression tree node representing an array with two elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(Expression element1, Expression element2)
        {
            if (element1 == null)
                throw new ArgumentNullException(nameof(element1));
            if (element2 == null)
                throw new ArgumentNullException(nameof(element2));

            return new ArrayExpression2(element1, element2);
        }

        /// <summary>
        /// Creates an expression tree node representing an array with three elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <param name="element3">Expression representing the third element in the array.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(Expression element1, Expression element2, Expression element3)
        {
            if (element1 == null)
                throw new ArgumentNullException(nameof(element1));
            if (element2 == null)
                throw new ArgumentNullException(nameof(element2));
            if (element3 == null)
                throw new ArgumentNullException(nameof(element3));

            return new ArrayExpression3(element1, element2, element3);
        }

        /// <summary>
        /// Creates an expression tree node representing an array with four elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <param name="element3">Expression representing the third element in the array.</param>
        /// <param name="element4">Expression representing the fourth element in the array.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(Expression element1, Expression element2, Expression element3, Expression element4)
        {
            if (element1 == null)
                throw new ArgumentNullException(nameof(element1));
            if (element2 == null)
                throw new ArgumentNullException(nameof(element2));
            if (element3 == null)
                throw new ArgumentNullException(nameof(element3));
            if (element4 == null)
                throw new ArgumentNullException(nameof(element4));

            return new ArrayExpression4(element1, element2, element3, element4);
        }

        /// <summary>
        /// Creates an expression tree node representing an array with five elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <param name="element3">Expression representing the third element in the array.</param>
        /// <param name="element4">Expression representing the fourth element in the array.</param>
        /// <param name="element5">Expression representing the fifth element in the array.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(Expression element1, Expression element2, Expression element3, Expression element4, Expression element5)
        {
            if (element1 == null)
                throw new ArgumentNullException(nameof(element1));
            if (element2 == null)
                throw new ArgumentNullException(nameof(element2));
            if (element3 == null)
                throw new ArgumentNullException(nameof(element3));
            if (element4 == null)
                throw new ArgumentNullException(nameof(element4));
            if (element5 == null)
                throw new ArgumentNullException(nameof(element5));

            return new ArrayExpression5(element1, element2, element3, element4, element5);
        }

        /// <summary>
        /// Creates an expression tree node representing an array with six elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <param name="element3">Expression representing the third element in the array.</param>
        /// <param name="element4">Expression representing the fourth element in the array.</param>
        /// <param name="element5">Expression representing the fifth element in the array.</param>
        /// <param name="element6">Expression representing the sixth element in the array.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(Expression element1, Expression element2, Expression element3, Expression element4, Expression element5, Expression element6)
        {
            if (element1 == null)
                throw new ArgumentNullException(nameof(element1));
            if (element2 == null)
                throw new ArgumentNullException(nameof(element2));
            if (element3 == null)
                throw new ArgumentNullException(nameof(element3));
            if (element4 == null)
                throw new ArgumentNullException(nameof(element4));
            if (element5 == null)
                throw new ArgumentNullException(nameof(element5));
            if (element6 == null)
                throw new ArgumentNullException(nameof(element6));

            return new ArrayExpression6(element1, element2, element3, element4, element5, element6);
        }

    }

    /// <summary>
    /// Expression tree node representing a JSON array with one element.
    /// </summary>
    internal sealed class ArrayExpression1 : ArrayExpressionN
    {
        /// <summary>
        /// Creates an instance of an expression tree node representing an array with one element.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        internal ArrayExpression1(Expression element1)
            : base(element1)
        {
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public override int ElementCount => 1;

        /// <summary>
        /// Gets the expression of the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element whose expression to get.</param>
        /// <returns>The expression representing the element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
        public override Expression GetElement(int index)
        {
            switch (index)
            {
                case 0:
                    return First;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Packs the elements in the array expression into a read-only collection.
        /// </summary>
        /// <param name="element1">First element in the array.</param>
        /// <returns>Read-only collection containing all of the elements in the array expression.</returns>
        protected override ReadOnlyCollection<Expression> Pack(Expression element1)
        {
            return new ReadOnlyCollection<Expression>(new[] { element1 });
        }

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="element1">First element in the array.</param>
        protected override void ToString(StringBuilder builder, Expression element1)
        {
            builder.Append('[');

            element1.ToStringCore(builder);

            builder.Append(']');
        }
    }

    /// <summary>
    /// Expression tree node representing a JSON array with two elements.
    /// </summary>
    internal sealed class ArrayExpression2 : ArrayExpressionN
    {
        /// <summary>
        /// Expression representing the second element in the array.
        /// </summary>
        private readonly Expression _element2;

        /// <summary>
        /// Creates an instance of an expression tree node representing an array with two elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        internal ArrayExpression2(Expression element1, Expression element2)
            : base(element1)
        {
            _element2 = element2;
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public override int ElementCount => 2;

        /// <summary>
        /// Gets the expression of the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element whose expression to get.</param>
        /// <returns>The expression representing the element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
        public override Expression GetElement(int index)
        {
            switch (index)
            {
                case 0:
                    return First;
                case 1:
                    return _element2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Packs the elements in the array expression into a read-only collection.
        /// </summary>
        /// <param name="element1">First element in the array.</param>
        /// <returns>Read-only collection containing all of the elements in the array expression.</returns>
        protected override ReadOnlyCollection<Expression> Pack(Expression element1)
        {
            return new ReadOnlyCollection<Expression>(new[] { element1, _element2 });
        }

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="element1">First element in the array.</param>
        protected override void ToString(StringBuilder builder, Expression element1)
        {
            builder.Append('[');

            element1.ToStringCore(builder);
            builder.Append(',');
            _element2.ToStringCore(builder);

            builder.Append(']');
        }
    }

    /// <summary>
    /// Expression tree node representing a JSON array with three elements.
    /// </summary>
    internal sealed class ArrayExpression3 : ArrayExpressionN
    {
        /// <summary>
        /// Expression representing the second element in the array.
        /// </summary>
        private readonly Expression _element2;

        /// <summary>
        /// Expression representing the third element in the array.
        /// </summary>
        private readonly Expression _element3;

        /// <summary>
        /// Creates an instance of an expression tree node representing an array with three elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <param name="element3">Expression representing the third element in the array.</param>
        internal ArrayExpression3(Expression element1, Expression element2, Expression element3)
            : base(element1)
        {
            _element2 = element2;
            _element3 = element3;
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public override int ElementCount => 3;

        /// <summary>
        /// Gets the expression of the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element whose expression to get.</param>
        /// <returns>The expression representing the element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
        public override Expression GetElement(int index)
        {
            switch (index)
            {
                case 0:
                    return First;
                case 1:
                    return _element2;
                case 2:
                    return _element3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Packs the elements in the array expression into a read-only collection.
        /// </summary>
        /// <param name="element1">First element in the array.</param>
        /// <returns>Read-only collection containing all of the elements in the array expression.</returns>
        protected override ReadOnlyCollection<Expression> Pack(Expression element1)
        {
            return new ReadOnlyCollection<Expression>(new[] { element1, _element2, _element3 });
        }

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="element1">First element in the array.</param>
        protected override void ToString(StringBuilder builder, Expression element1)
        {
            builder.Append('[');

            element1.ToStringCore(builder);
            builder.Append(',');
            _element2.ToStringCore(builder);
            builder.Append(',');
            _element3.ToStringCore(builder);

            builder.Append(']');
        }
    }

    /// <summary>
    /// Expression tree node representing a JSON array with four elements.
    /// </summary>
    internal sealed class ArrayExpression4 : ArrayExpressionN
    {
        /// <summary>
        /// Expression representing the second element in the array.
        /// </summary>
        private readonly Expression _element2;

        /// <summary>
        /// Expression representing the third element in the array.
        /// </summary>
        private readonly Expression _element3;

        /// <summary>
        /// Expression representing the fourth element in the array.
        /// </summary>
        private readonly Expression _element4;

        /// <summary>
        /// Creates an instance of an expression tree node representing an array with four elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <param name="element3">Expression representing the third element in the array.</param>
        /// <param name="element4">Expression representing the fourth element in the array.</param>
        internal ArrayExpression4(Expression element1, Expression element2, Expression element3, Expression element4)
            : base(element1)
        {
            _element2 = element2;
            _element3 = element3;
            _element4 = element4;
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public override int ElementCount => 4;

        /// <summary>
        /// Gets the expression of the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element whose expression to get.</param>
        /// <returns>The expression representing the element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
        public override Expression GetElement(int index)
        {
            switch (index)
            {
                case 0:
                    return First;
                case 1:
                    return _element2;
                case 2:
                    return _element3;
                case 3:
                    return _element4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Packs the elements in the array expression into a read-only collection.
        /// </summary>
        /// <param name="element1">First element in the array.</param>
        /// <returns>Read-only collection containing all of the elements in the array expression.</returns>
        protected override ReadOnlyCollection<Expression> Pack(Expression element1)
        {
            return new ReadOnlyCollection<Expression>(new[] { element1, _element2, _element3, _element4 });
        }

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="element1">First element in the array.</param>
        protected override void ToString(StringBuilder builder, Expression element1)
        {
            builder.Append('[');

            element1.ToStringCore(builder);
            builder.Append(',');
            _element2.ToStringCore(builder);
            builder.Append(',');
            _element3.ToStringCore(builder);
            builder.Append(',');
            _element4.ToStringCore(builder);

            builder.Append(']');
        }
    }

    /// <summary>
    /// Expression tree node representing a JSON array with five elements.
    /// </summary>
    internal sealed class ArrayExpression5 : ArrayExpressionN
    {
        /// <summary>
        /// Expression representing the second element in the array.
        /// </summary>
        private readonly Expression _element2;

        /// <summary>
        /// Expression representing the third element in the array.
        /// </summary>
        private readonly Expression _element3;

        /// <summary>
        /// Expression representing the fourth element in the array.
        /// </summary>
        private readonly Expression _element4;

        /// <summary>
        /// Expression representing the fifth element in the array.
        /// </summary>
        private readonly Expression _element5;

        /// <summary>
        /// Creates an instance of an expression tree node representing an array with five elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <param name="element3">Expression representing the third element in the array.</param>
        /// <param name="element4">Expression representing the fourth element in the array.</param>
        /// <param name="element5">Expression representing the fifth element in the array.</param>
        internal ArrayExpression5(Expression element1, Expression element2, Expression element3, Expression element4, Expression element5)
            : base(element1)
        {
            _element2 = element2;
            _element3 = element3;
            _element4 = element4;
            _element5 = element5;
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public override int ElementCount => 5;

        /// <summary>
        /// Gets the expression of the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element whose expression to get.</param>
        /// <returns>The expression representing the element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
        public override Expression GetElement(int index)
        {
            switch (index)
            {
                case 0:
                    return First;
                case 1:
                    return _element2;
                case 2:
                    return _element3;
                case 3:
                    return _element4;
                case 4:
                    return _element5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Packs the elements in the array expression into a read-only collection.
        /// </summary>
        /// <param name="element1">First element in the array.</param>
        /// <returns>Read-only collection containing all of the elements in the array expression.</returns>
        protected override ReadOnlyCollection<Expression> Pack(Expression element1)
        {
            return new ReadOnlyCollection<Expression>(new[] { element1, _element2, _element3, _element4, _element5 });
        }

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="element1">First element in the array.</param>
        protected override void ToString(StringBuilder builder, Expression element1)
        {
            builder.Append('[');

            element1.ToStringCore(builder);
            builder.Append(',');
            _element2.ToStringCore(builder);
            builder.Append(',');
            _element3.ToStringCore(builder);
            builder.Append(',');
            _element4.ToStringCore(builder);
            builder.Append(',');
            _element5.ToStringCore(builder);

            builder.Append(']');
        }
    }

    /// <summary>
    /// Expression tree node representing a JSON array with six elements.
    /// </summary>
    internal sealed class ArrayExpression6 : ArrayExpressionN
    {
        /// <summary>
        /// Expression representing the second element in the array.
        /// </summary>
        private readonly Expression _element2;

        /// <summary>
        /// Expression representing the third element in the array.
        /// </summary>
        private readonly Expression _element3;

        /// <summary>
        /// Expression representing the fourth element in the array.
        /// </summary>
        private readonly Expression _element4;

        /// <summary>
        /// Expression representing the fifth element in the array.
        /// </summary>
        private readonly Expression _element5;

        /// <summary>
        /// Expression representing the sixth element in the array.
        /// </summary>
        private readonly Expression _element6;

        /// <summary>
        /// Creates an instance of an expression tree node representing an array with six elements.
        /// </summary>
        /// <param name="element1">Expression representing the first element in the array.</param>
        /// <param name="element2">Expression representing the second element in the array.</param>
        /// <param name="element3">Expression representing the third element in the array.</param>
        /// <param name="element4">Expression representing the fourth element in the array.</param>
        /// <param name="element5">Expression representing the fifth element in the array.</param>
        /// <param name="element6">Expression representing the sixth element in the array.</param>
        internal ArrayExpression6(Expression element1, Expression element2, Expression element3, Expression element4, Expression element5, Expression element6)
            : base(element1)
        {
            _element2 = element2;
            _element3 = element3;
            _element4 = element4;
            _element5 = element5;
            _element6 = element6;
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public override int ElementCount => 6;

        /// <summary>
        /// Gets the expression of the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element whose expression to get.</param>
        /// <returns>The expression representing the element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
        public override Expression GetElement(int index)
        {
            switch (index)
            {
                case 0:
                    return First;
                case 1:
                    return _element2;
                case 2:
                    return _element3;
                case 3:
                    return _element4;
                case 4:
                    return _element5;
                case 5:
                    return _element6;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Packs the elements in the array expression into a read-only collection.
        /// </summary>
        /// <param name="element1">First element in the array.</param>
        /// <returns>Read-only collection containing all of the elements in the array expression.</returns>
        protected override ReadOnlyCollection<Expression> Pack(Expression element1)
        {
            return new ReadOnlyCollection<Expression>(new[] { element1, _element2, _element3, _element4, _element5, _element6 });
        }

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="element1">First element in the array.</param>
        protected override void ToString(StringBuilder builder, Expression element1)
        {
            builder.Append('[');

            element1.ToStringCore(builder);
            builder.Append(',');
            _element2.ToStringCore(builder);
            builder.Append(',');
            _element3.ToStringCore(builder);
            builder.Append(',');
            _element4.ToStringCore(builder);
            builder.Append(',');
            _element5.ToStringCore(builder);
            builder.Append(',');
            _element6.ToStringCore(builder);

            builder.Append(']');
        }
    }

}
