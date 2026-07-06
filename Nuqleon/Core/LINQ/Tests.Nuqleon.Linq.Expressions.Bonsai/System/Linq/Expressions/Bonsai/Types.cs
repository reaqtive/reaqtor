// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

// NB: The short top-level namespace keeps the type names compact in the expected C# printer
//     output asserted by ExpressionSlimCSharpPrinterTest.cs. Split out of that file so it can
//     use a file-scoped namespace (a file can hold only one namespace in that style).

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable 0649

namespace Types;

internal sealed class Bar
{
    public Bar()
    {
    }

    public Bar(int x)
    {
    }

    public int Qux { get; set; }
    public Foo Foo;
    public List<int> Ys { get; set; }
}

internal sealed class Foo
{
    public int Baz;
}
