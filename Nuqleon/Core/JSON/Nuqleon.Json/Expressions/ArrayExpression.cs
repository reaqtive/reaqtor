// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009, June 2014 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Expression tree node representing a JSON array.
    /// </summary>
    public abstract class ArrayExpression : Expression
    {
        #region Constructors

        /// <summary>
        /// Internal constructor to prevent derived types outside this assembly.
        /// </summary>
        internal ArrayExpression()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression tree objects for the array elements.
        /// </summary>
        public abstract ReadOnlyCollection<Expression> Elements { get; }

        /// <summary>
        /// Gets the type of the JSON expression tree node.
        /// </summary>
        public override ExpressionType NodeType => ExpressionType.Array;

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public abstract int ElementCount { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the expression of the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element whose expression to get.</param>
        /// <returns>The expression representing the element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
        public abstract Expression GetElement(int index);

        #endregion
    }

    /// <summary>
    /// Expression tree node representing a JSON array with an arbitrary number of elements.
    /// </summary>
    internal sealed class SimpleArrayExpression : ArrayExpression
    {
        #region Constructors

        /// <summary>
        /// Creates a new array expression tree node object with the given elements.
        /// </summary>
        /// <param name="elements">Expression tree objects for the array elements.</param>
        internal SimpleArrayExpression(IList<Expression> elements)
        {
            Elements = new ReadOnlyCollection<Expression>(elements);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression tree objects for the array elements.
        /// </summary>
        public override ReadOnlyCollection<Expression> Elements { get; }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public override int ElementCount => Elements.Count;

        #endregion

        #region Methods

        /// <summary>
        /// Gets the expression of the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element whose expression to get.</param>
        /// <returns>The expression representing the element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
        public override Expression GetElement(int index) => Elements[index];

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        internal override void ToStringCore(StringBuilder builder)
        {
            builder.Append('[');

            var n = Elements.Count;

            if (n > 0)
            {
                for (var i = 0; i < n; i++)
                {
                    var element = Elements[i];

                    element.ToStringCore(builder);

                    if (i < n - 1)
                    {
                        builder.Append(',');
                    }
                }
            }

            builder.Append(']');
        }

        #endregion
    }

    /// <summary>
    /// Expression tree node representing a JSON array with an fixed number of elements.
    /// </summary>
    internal abstract class ArrayExpressionN : ArrayExpression
    {
        /*
         * Implementation note:
         * 
         * The goal of ArrayExpressionN subclasses is to avoid the allocation of the ReadOnlyCollection<Expression>
         * containing the array's elements if the Elements property is not accessed. This is typically the case when
         * the Nuqleon.Json library is used to construct JSON expressions and serialize these to text by calling
         * ToString. To achieve this effect, the _element1 field is owned by the base class and will either contain
         * the first element or a ReadOnlyCollection<Expression> of the elements in the array. Upon first access (if
         * it occurs anyway) to the Elements property, a call is made to the Pack method to prompt the derived class
         * to package up all of its element expressions into a ReadOnlyCollection<Expression> which is then swapped
         * into the _element1 field. Both the Elements property and the ToString method avoid creating a collection
         * to hold the elements and will use the allocation-free route, unless a collection is already present.
         * 
         * With regards to thread-safety, notice that the first element is passed to the virtual methods for Pack
         * and ToString, and _element1 is kept private. This is to protect against the case where concurrent calls
         * to Pack are outstanding and the _element1 field is swapped out for a ReadOnlyCollection<Expression> so
         * it can no longer be cast to Expression. No locks are used (avoiding the allocation of another object) to
         * protect the swap of _element1. Worst case, some threads race in Pack, and only the winner swaps in its
         * resulting ReadOnlyCollection<Expression>, making other instances eligible for garbage collection.
         * 
         */

        #region Private fields

        /// <summary>
        /// First element, or a ReadOnlyCollection&lt;Expression&gt; after the first access to the Elements property has occurred.
        /// </summary>
        private object _element1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new array expression with the specified first element. Other elements are maintained by the derived class.
        /// </summary>
        /// <param name="element1">First element in the array.</param>
        public ArrayExpressionN(Expression element1)
        {
            _element1 = element1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression tree objects for the array elements.
        /// </summary>
        public override ReadOnlyCollection<Expression> Elements
        {
            get
            {
                var element1 = _element1;

                if (element1 is ReadOnlyCollection<Expression> elements)
                {
                    return elements;
                }
                else
                {
                    var res = Pack((Expression)element1);
                    _element1 = res;
                    return res;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the first element.
        /// </summary>
        protected Expression First
        {
            get
            {
                var element1 = _element1;

                return element1 is ReadOnlyCollection<Expression> elements ? elements[0] : (Expression)element1;
            }
        }

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        internal override void ToStringCore(StringBuilder builder)
        {
            var element1 = _element1;

            if (element1 is ReadOnlyCollection<Expression> elements)
            {
                builder.Append('[');

                var n = elements.Count;

                if (n > 0)
                {
                    for (var i = 0; i < n; i++)
                    {
                        var element = elements[i];

                        element.ToStringCore(builder);

                        if (i < n - 1)
                        {
                            builder.Append(',');
                        }
                    }
                }

                builder.Append(']');
            }
            else
            {
                ToString(builder, (Expression)element1);
            }
        }

        /// <summary>
        /// Packs the elements in the array expression into a read-only collection.
        /// </summary>
        /// <param name="element1">First element in the array.</param>
        /// <returns>Read-only collection containing all of the elements in the array expression.</returns>
        protected abstract ReadOnlyCollection<Expression> Pack(Expression element1);

        /// <summary>
        /// Appends the JSON fragment representing the array expression to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="element1">First element in the array.</param>
        protected abstract void ToString(StringBuilder builder, Expression element1);

        #endregion
    }
}
