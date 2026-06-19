// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of an array type.
    /// </summary>
    public abstract class ArrayTypeSlim : TypeSlim
    {
        #region Constructors

        /// <summary>
        /// Creates a new array type representation object.
        /// </summary>
        /// <param name="elementType">Element type of the array.</param>
        private ArrayTypeSlim(TypeSlim elementType)
        {
            ElementType = elementType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the kind of the type.
        /// </summary>
        public sealed override TypeSlimKind Kind => TypeSlimKind.Array;

        /// <summary>
        /// Gets the element type.
        /// </summary>
        public TypeSlim ElementType { get; }

        /// <summary>
        /// Gets the rank of the array, i.e. the number of dimensions. If the rank is null, the array is single-dimensional.
        /// </summary>
        public abstract int? Rank { get; }

        #endregion

        #region Methods

        // NB: Update is kept internal for now until we make this available as a proper API.
        //     It's only used by the visitor at this point.

        /// <summary>
        /// Returns a new lightweight representation of an array type, or the current instance if nothing has changed.
        /// </summary>
        /// <param name="elementType">The new element type.</param>
        /// <returns>A lightweight representation of an array type with the specified element type.</returns>
        internal ArrayTypeSlim Update(TypeSlim elementType)
        {
            if (elementType != ElementType)
            {
                return Rewrite(elementType);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a new lightweight representation of an array type with the specified element type.
        /// </summary>
        /// <param name="elementType">The new element type.</param>
        /// <returns>A lightweight representation of an array type with the specified element type.</returns>
        protected abstract ArrayTypeSlim Rewrite(TypeSlim elementType);

        internal static ArrayTypeSlim CreateVector(TypeSlim elementType)
        {
            RequireNotNull(elementType, nameof(elementType));

            return new VectorArrayTypeSlim(elementType);
        }

        internal static ArrayTypeSlim CreateMultiDimensional(TypeSlim elementType, int rank)
        {
            RequireNotNull(elementType, nameof(elementType));

            if (rank <= 0)
                throw new ArgumentOutOfRangeException(nameof(rank));

            return new MultiDimensionalArrayTypeSlim(elementType, rank);
        }

        #endregion

        #region Types

        private sealed class VectorArrayTypeSlim : ArrayTypeSlim
        {
            public VectorArrayTypeSlim(TypeSlim elementType)
                : base(elementType)
            {
            }

            public override int? Rank => null;

            protected override ArrayTypeSlim Rewrite(TypeSlim elementType) => CreateVector(elementType);
        }

        private sealed class MultiDimensionalArrayTypeSlim : ArrayTypeSlim
        {
            public MultiDimensionalArrayTypeSlim(TypeSlim elementType, int rank)
                : base(elementType)
            {
                Rank = rank;
            }

            public override int? Rank { get; }

            protected override ArrayTypeSlim Rewrite(TypeSlim elementType) => CreateMultiDimensional(elementType, Rank.Value);
        }

        #endregion
    }

    public partial class TypeSlim
    {
        /// <summary>
        /// Creates a new lightweight representation of a single-dimensional array type.
        /// </summary>
        /// <param name="elementType">The element type of the array.</param>
        /// <returns>A lightweight representation of a single-dimensional array type.</returns>
        public static ArrayTypeSlim Array(TypeSlim elementType)
        {
            return ArrayTypeSlim.CreateVector(elementType);
        }

        /// <summary>
        /// Creates a new lightweight representation of a multi-dimensional array type.
        /// </summary>
        /// <param name="elementType">The element type of the array.</param>
        /// <param name="rank">The number of dimensions of the array.</param>
        /// <returns>A lightweight representation of a multi-dimensional array type.</returns>
        public static ArrayTypeSlim Array(TypeSlim elementType, int rank)
        {
            return ArrayTypeSlim.CreateMultiDimensional(elementType, rank);
        }
    }
}
