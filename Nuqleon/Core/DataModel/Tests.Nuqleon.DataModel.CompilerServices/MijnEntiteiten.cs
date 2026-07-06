// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

// NB: Entity types used by ExpressionEntityTypeRecordizerTests. Split out of that file so it can
//     use a file-scoped namespace (a file can hold only one namespace in that style).

using Nuqleon.DataModel;

namespace MijnEntiteiten;

public class Persoon1
{
    [Mapping("contoso://entities/person/name")]
    public string Naam { get; set; }

    [Mapping("contoso://entities/person/age")]
    public int Leeftijd { get; set; }

    [Mapping("contoso://entities/person/sex")]
    public Sex Geslacht { get; set; }
}

public class Persoon2
{
    [Mapping("contoso://entities/person/name")]
    public string Naam { get; set; }

    [Mapping("contoso://entities/person/age")]
    public int Leeftijd { get; set; }

    [Mapping("contoso://entities/person/sex")]
    public Sex? Geslacht { get; set; }
}

public class Persoon3
{
    [Mapping("contoso://entities/person/name")]
    public string Naam { get; set; }

    [Mapping("contoso://entities/person/age")]
    public int Leeftijd { get; set; }

    [Mapping("contoso://entities/person/sex")]
    public Sex Geslacht { get; set; }

    [Mapping("contoso://entities/person/color")]
    public ConsoleColor Color { get; set; }
}

public enum Sex
{
    [Mapping("contoso://gender/male")]
    Male = 1,

    [Mapping("contoso://gender/female")]
    Female = 2,
}
