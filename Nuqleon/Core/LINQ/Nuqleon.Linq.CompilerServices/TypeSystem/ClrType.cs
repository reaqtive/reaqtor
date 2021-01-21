// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011
//

namespace System.Linq.CompilerServices
{
    internal class ClrType : IType
    {
        public static readonly IType Void = new ClrType(typeof(void));

        private readonly Type _type;

        public ClrType(Type type) => _type = type;

        public bool IsAssignableTo(IType type)
        {
            if (type is not ClrType clrType)
                throw new InvalidOperationException("Type check across type systems.");

            return IsAssignableTo(_type, clrType._type);
        }

        private static bool IsAssignableTo(Type fromType, Type toType) => toType.IsAssignableFrom(fromType); // TODO: generic wildcards

        public bool Equals(IType other)
        {
            if (other is not ClrType clrType)
                throw new InvalidOperationException("Type check across type systems.");

            return clrType._type.Equals(_type);
        }

        public override bool Equals(object obj) => obj is IType type ? Equals(type) : base.Equals(obj);

        public override int GetHashCode() => _type.GetHashCode();

        public override string ToString() => _type.ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false);
    }
}
