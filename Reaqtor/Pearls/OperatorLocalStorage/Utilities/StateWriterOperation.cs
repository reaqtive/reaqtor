// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace Utilities
{
    public abstract class StateWriterOperation(string category, string key)
    {
        public abstract StateWriterOperationKind Kind { get; }

        public string Category { get; } = category;
        public string Key { get; } = key;

        public abstract void Apply(Store store);
    }
}
