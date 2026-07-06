// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

// NB: The short top-level namespace keeps the type names compact in the expected ToString
//     output asserted by Tests.cs. Split out of Tests.cs so that file can use a file-scoped
//     namespace (a file can hold only one namespace in that style).

#pragma warning disable 0649

namespace Types2;

internal class Bar
{
    public int Foo;
    public List<int> Quxs;
    public Baz Baz;
}

internal class Baz
{
    public int Qux;
}
