// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive
{
    /// <summary>
    /// Interface for operators that support unloading.
    /// </summary>
    public interface IUnloadableOperator : IOperator
    {
        /// <summary>
        /// Unloads the operator.
        /// </summary>
        void Unload();
    }
}
