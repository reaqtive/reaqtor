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
    /// Lightweight representation of a simple type.
    /// </summary>
    public abstract class SimpleTypeSlimBase : TypeSlim
    {
        /// <summary>
        /// Creates a new generic type definition representation object.
        /// </summary>
        /// <param name="assembly">Assembly defining the type.</param>
        /// <param name="name">Name of the type.</param>
        protected SimpleTypeSlimBase(AssemblySlim assembly, string name)
        {
            Assembly = assembly;
            Name = name;
        }

        /// <summary>
        /// Gets the assembly defining the type.
        /// </summary>
        public AssemblySlim Assembly { get; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The equals method for comparing against other class instances.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><b>true</b> if the given instance equals this instance, <b>false</b> otherwise.</returns>
        public override bool Equals(TypeSlim other)
        {
            if (other is SimpleTypeSlimBase s)
            {
                return Kind == s.Kind
                    && Name == s.Name
                    && Assembly?.Name == s.Assembly?.Name;
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if (_hashCode == 0)
            {
#if NET5_0
                var hash = Assembly?.Name?.GetHashCode(StringComparison.Ordinal) ?? 0;
                _hashCode = (int)(hash * TypeSlimEqualityComparator.Prime) + Name.GetHashCode(StringComparison.Ordinal);
#else
                var hash = Assembly?.Name?.GetHashCode() ?? 0;
                _hashCode = (int)(hash * TypeSlimEqualityComparator.Prime) + Name.GetHashCode();
#endif
            }

            return _hashCode;
        }
    }
}
