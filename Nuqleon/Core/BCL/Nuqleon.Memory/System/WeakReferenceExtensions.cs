// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Created WeakReferenceExtensions functionality.
//

namespace System
{
    /// <summary>
    /// Provides a set of extension methods of <see cref="WeakReference{T}"/>.
    /// </summary>
    public static class WeakReferenceExtensions
    {
        /// <summary>
        /// Creates a new weak reference to the specified target object.
        /// </summary>
        /// <typeparam name="T">Type of the object referred to by the weak reference. This type has to be a reference type.</typeparam>
        /// <param name="target">The target object referred to by the weak reference.</param>
        /// <returns>A new weak reference referring to the specified target object.</returns>
        /// <remarks>
        /// Weak reference instances created using this method use a singleton instance for a weak reference containing a null reference.
        /// The <see cref="GetTarget{T}"/> and <see cref="GetOrSetTarget{T}"/> methods use this singleton instance to differentiate between an intentional (weak) null reference and a weak reference whose target has been collected.
        /// </remarks>
        public static WeakReference<T> Create<T>(T target)
            where T : class
        {
            return Create(target, trackResurrection: false);
        }

        /// <summary>
        /// Creates a new weak reference to the specified target object, using the specified resurrection tracking.
        /// </summary>
        /// <typeparam name="T">Type of the object referred to by the weak reference. This type has to be a reference type.</typeparam>
        /// <param name="target">The target object referred to by the weak reference.</param>
        /// <param name="trackResurrection">true to track the object after finalization; false to track the object only until finalization.</param>
        /// <returns>A new weak reference referring to the specified target object.</returns>
        /// <remarks>
        /// Weak reference instances created using this method use a singleton instance for a weak reference containing a null reference.
        /// The <see cref="GetTarget{T}"/> and <see cref="GetOrSetTarget{T}"/> methods use this singleton instance to differentiate between an intentional (weak) null reference and a weak reference whose target has been collected.
        /// </remarks>
        public static WeakReference<T> Create<T>(T target, bool trackResurrection)
            where T : class
        {
            if (target == null)
            {
                return Holder<T>.Null;
            }
            else
            {
                return new WeakReference<T>(target, trackResurrection);
            }
        }

        /// <summary>
        /// Gets the target object referred to by the specified weak reference. If the weak reference no longer has a reference to the underlying object, a <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <typeparam name="T">Type of the object referred to by the weak reference. This type has to be a reference type.</typeparam>
        /// <param name="weakReference">The weak reference whose referred target object to obtain.</param>
        /// <returns>The target object referred to by the weak reference.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the weak reference no longer holds a reference to the original target object.</exception>
        /// <remarks>
        /// If the weak reference intentionally contains a null reference (see <see cref="Create{T}(T)"/>), a null reference will be returned.
        /// </remarks>
        public static T GetTarget<T>(this WeakReference<T> weakReference)
            where T : class
        {
            if (weakReference == null)
                throw new ArgumentNullException(nameof(weakReference));

            if (weakReference == Holder<T>.Null)
            {
                return null;
            }

            if (!weakReference.TryGetTarget(out T res))
            {
                throw new InvalidOperationException("Weak reference no longer alive.");
            }

            return res;
        }

        /// <summary>
        /// Gets the target object referred to by the specified weak reference. If the weak reference no longer has a reference to the underlying object, the specified factory delegate is invoked to create and set a new target.
        /// </summary>
        /// <typeparam name="T">Type of the object referred to by the weak reference. This type has to be a reference type.</typeparam>
        /// <param name="weakReference">The weak reference whose referred target object to obtain.</param>
        /// <param name="targetFactory">The target factory delegate to invoke in case the weak reference no longer has a reference to the underlying object.</param>
        /// <returns>The target object referred to by the weak reference.</returns>
        /// <remarks>
        /// If the weak reference intentionally contains a null reference (see <see cref="Create{T}(T)"/>), the factory delegate won't get invoked and a null reference will be returned.
        /// </remarks>
        public static T GetOrSetTarget<T>(this WeakReference<T> weakReference, Func<T> targetFactory)
            where T : class
        {
            if (weakReference == null)
                throw new ArgumentNullException(nameof(weakReference));
            if (targetFactory == null)
                throw new ArgumentNullException(nameof(targetFactory));

            if (weakReference == Holder<T>.Null)
            {
                return null;
            }

            if (!weakReference.TryGetTarget(out T res))
            {
                res = targetFactory();
                weakReference.SetTarget(res);
            }

            return res;
        }

        private static class Holder<T>
            where T : class
        {
            public static readonly WeakReference<T> Null = new(target: null);
        }
    }
}
