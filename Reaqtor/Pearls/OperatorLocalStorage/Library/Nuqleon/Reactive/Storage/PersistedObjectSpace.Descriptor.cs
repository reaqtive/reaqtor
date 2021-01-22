// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using Nuqleon.DataModel;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Entity type used to store the descriptor of a persisted entity.
        /// </summary>
        private sealed class Descriptor
        {
            /// <summary>
            /// Gets or sets the string representation of the persisted entity kind, see <see cref="PersistableKind"/>.
            /// </summary>
            [Mapping("kind")]
            public string Kind { get; set; }

            //
            // NB: Additional fields can be added here to represent additional state required for instantiation of persisted entities.
            //
        }
    }
}
