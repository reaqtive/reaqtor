// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices.Tools.BURS
{
    internal sealed class Bar
    {
    }

    internal sealed class Foo
    {
    }

    internal sealed class BarWildcards : IWildcardFactory<ITree<Bar>>
    {
        public ITree<Bar> CreateWildcard(global::System.Linq.Expressions.ParameterExpression hole)
        {
            throw new NotImplementedException();
        }
    }
}
