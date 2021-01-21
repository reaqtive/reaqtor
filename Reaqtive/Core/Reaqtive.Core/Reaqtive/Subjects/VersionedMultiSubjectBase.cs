// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Base implementation of the non-generic multi-subject, with support for versioning.
    /// </summary>
    public abstract class VersionedMultiSubjectBase : MultiSubjectBase, IVersioned
    {
        /// <summary>
        /// The name of the multi-subject.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// The version of the multi-subject.
        /// </summary>
        public abstract Version Version
        {
            get;
        }
    }
}
