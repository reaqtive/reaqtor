// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1040 // Avoid empty interfaces.

namespace Reaqtive
{
    /// <summary>
    /// Annotate operators with this interface to denote that existing
    /// subscriptions with this operator may need to transition from a
    /// stateless variant to a stateful one.
    /// </summary>
    public interface ITransitioningOperator
    {
    }
}
