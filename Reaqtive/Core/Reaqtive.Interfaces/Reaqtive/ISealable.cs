// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace Reaqtive
{
    /// <summary>
    /// Infrastructure.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISealable
    {
        /// <summary>
        /// Seals the object.
        /// </summary>
        void Seal();
    }
}
