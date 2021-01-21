// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Testable state container.
    /// </summary>
    public interface IOperatorStateContainer
    {
        // TODO: Think of keeping track of recorded onload/onsave events.

        /// <summary>
        /// Creates a new reader of the current state.
        /// </summary>
        IOperatorStateReaderFactory CreateReader();

        /// <summary>
        /// Creates a new writer. The state is reset.
        /// </summary>
        IOperatorStateWriterFactory CreateWriter();
    }
}
