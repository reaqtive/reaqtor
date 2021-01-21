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
    public abstract class StateWriterOperation
    {
        protected StateWriterOperation(string category, string key)
        {
            Category = category;
            Key = key;
        }

        public abstract StateWriterOperationKind Kind { get; }

        public string Category { get; }
        public string Key { get; }

        public abstract void Apply(Store store);
    }
}
