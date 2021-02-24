// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Base class for observers with state versioning capabilities.
    /// </summary>
    /// <typeparam name="T">Type of the elements received by the observer.</typeparam>
    public abstract class VersionedObserver<T> : Observer<T>, IVersioned
    {
        /// <summary>
        /// Gets the name tag of the observer, used to persist state headers.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the version of the observer.
        /// </summary>
        public abstract Version Version
        {
            get;
        }
    }
}
