// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.CodeDom.Compiler;

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to perform type system checks.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IReflectionTypeSystemProvider
    {
        /// <summary>
        /// Determines whether the class represented by the specified <paramref name="type" /> derives from the class represented by the specified type <paramref name="c"/>.
        /// </summary>
        /// <param name="type">The type to check for being a subclass.</param>
        /// <param name="c">The type to compare with the specified type.</param>
        /// <returns>true if the type represented by the <paramref name="c" /> parameter and the specified <paramref name="type"/> represent classes, and the class represented by the specified <paramref name="type"/> derives from the class represented by <paramref name="c" />; otherwise, false. This method also returns false if <paramref name="c" /> and the specified <paramref name="type"/> represent the same class.</returns>
        bool IsSubclassOf(Type type, Type c);

        /// <summary>
        /// Determines whether the specified object is an instance of the specified <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type to compare the object against.</param>
        /// <param name="o">The object to compare with the specified type.</param>
        /// <returns>true if the specified <paramref name="type"/> is in the inheritance hierarchy of the object represented by <paramref name="o" />, or if the specified <paramref name="type"/> is an interface that <paramref name="o" /> supports. false if neither of these conditions is the case, or if <paramref name="o" /> is null, or if the specified <paramref name="type"/> is an open generic type (that is, <see cref="Type.ContainsGenericParameters" /> returns true).</returns>
        bool IsInstanceOfType(Type type, object o);

        /// <summary>
        /// Determines whether two COM types have the same identity and are eligible for type equivalence.
        /// </summary>
        /// <param name="type">The COM type that is tested for equivalence with the other type.</param>
        /// <param name="other">The COM type that is tested for equivalence with the specified type.</param>
        /// <returns>true if the COM types are equivalent; otherwise, false. This method also returns false if one type is in an assembly that is loaded for execution, and the other is in an assembly that is loaded into the reflection-only context.</returns>
        bool IsEquivalentTo(Type type, Type other);

        /// <summary>
        /// Determines whether an instance of the specified <paramref name="type"/> can be assigned from an instance of the specified type <paramref name="c"/>.
        /// </summary>
        /// <param name="type">The type to check for assignment compatibility.</param>
        /// <param name="c">The type to compare with the specified <paramref name="type"/>.</param>
        /// <returns>true if <paramref name="c" /> and the specified <paramref name="type"/> represent the same type, or if the specified <paramref name="type"/> is in the inheritance hierarchy of <paramref name="c" />, or if the specified <paramref name="type"/> is an interface that <paramref name="c" /> implements, or if <paramref name="c" /> is a generic type parameter and the specified <paramref name="type"/> represents one of the constraints of <paramref name="c" />. false if none of these conditions are true, or if <paramref name="c" /> is null.</returns>
        bool IsAssignableFrom(Type type, Type c);
    }
}
