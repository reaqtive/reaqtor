// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Base class for operators with state versioning capabilities.
    /// </summary>
    /// <typeparam name="TParam">Type of the parameter bundle passed to the operator.</typeparam>
    /// <typeparam name="TResult">Result of the elements produced by the operator.</typeparam>
    public abstract class VersionedOperator<TParam, TResult> : Operator<TParam, TResult>, IVersioned
    {
        /// <summary>
        /// Creates a new versioned operator with the specified parameters and output observer.
        /// </summary>
        /// <param name="parent">Parameters passed to the operator instance.</param>
        /// <param name="observer">Observer to write operator output to.</param>
        protected VersionedOperator(TParam parent, IObserver<TResult> observer)
            : base(parent, observer)
        {
        }

        /// <summary>
        /// Gets the name tag of the operator, used to persist state headers.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the version of the operator.
        /// </summary>
        public abstract Version Version
        {
            get;
        }
    }
}
