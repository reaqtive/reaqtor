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
    /// Lightweight representation of an assembly, i.e. a container for types.
    /// </summary>
    public sealed class AssemblySlim : IEquatable<AssemblySlim>
    {
        /// <summary>
        /// Creates a new assembly representation object.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        public AssemblySlim(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Assembly name can not be null or empty.", nameof(name));

            Name = name;
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Checks if two assemblies are equal.
        /// </summary>
        /// <param name="first">The first assembly to compare.</param>
        /// <param name="second">The second assembly to compare.</param>
        /// <returns><c>true</c> if both assemblies are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(AssemblySlim first, AssemblySlim second) => first is null ? second is null : first.Equals(second);

        /// <summary>
        /// Checks if two assemblies are not equal.
        /// </summary>
        /// <param name="first">The first assembly to compare.</param>
        /// <param name="second">The second assembly to compare.</param>
        /// <returns><c>true</c> if both assemblies are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(AssemblySlim first, AssemblySlim second) => !(first == second);

        /// <summary>
        /// Checks whether the specified assembly is equal to the current instance.
        /// </summary>
        /// <param name="other">The assembly to compare.</param>
        /// <returns><c>true</c> if the specified assembly is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(AssemblySlim other) => other != null && Name == other.Name;

        /// <summary>
        /// Checks whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as AssemblySlim);

        /// <summary>
        /// Gets a hash code for the current instance.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode() =>
#if NET5_0 || NETSTANDARD2_1
            Name.GetHashCode(StringComparison.Ordinal);
#else
            Name.GetHashCode();
#endif

        /// <summary>
        /// Returns a friendly string representation of the assembly.
        /// </summary>
        /// <returns>Friendly string representation of the assembly.</returns>
        public override string ToString() => Name;
    }
}
