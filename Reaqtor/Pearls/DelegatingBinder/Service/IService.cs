// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace DelegatingBinder
{
    internal interface IService
    {
        void Evaluate(Expression expression); // NOTE: This is more elegant than IRP but has more overhead ("everything is an expression" mantra)
    }
}
