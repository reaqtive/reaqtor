// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Provides commonly used types.
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Represents the top type of a type system. Every type is assignable to Top.
        /// </summary>
        /// <example>The root of a type hierarchy, such as System.Object in the CLR, is typically implemented as Top.</example>
        public static IType Top { get; } = new TopImpl();

        /// <summary>
        /// Represents the bottom type of a type system. Every type is assignable from Bottom.
        /// </summary>
        /// <example>A type representing the empty data type or the Nil value is typically implemented as Bottom.</example>
        public static IType Bottom { get; } = new BotImpl();

        private sealed class TopImpl : IType
        {
            public bool IsAssignableTo(IType type) => object.ReferenceEquals(this, type);

            public bool Equals(IType other) => object.ReferenceEquals(this, other);

            public override string ToString() => "Top";
        }

        private sealed class BotImpl : IType
        {
            public bool IsAssignableTo(IType type) => true;

            public bool Equals(IType other) => object.ReferenceEquals(this, other);

            public override string ToString() => "Bot";
        }
    }
}
