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
    public sealed class DeleteStateWriterOperation : StateWriterOperation
    {
        public DeleteStateWriterOperation(string category, string key) : base(category, key)
        {
        }

        public override StateWriterOperationKind Kind => StateWriterOperationKind.Delete;

        public override void Apply(Store store)
        {
            var success = false;

            if (store.Data.TryGetValue(Category, out var table))
            {
                success = table.Remove(Key);
            }

            if (!success)
            {
                // TODO: Implement ACID behavior.
            }
        }
    }
}
