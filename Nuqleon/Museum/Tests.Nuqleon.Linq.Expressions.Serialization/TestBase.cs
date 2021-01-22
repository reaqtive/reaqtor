// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2013
//

namespace Tests
{
    public class TestBase
    {
        protected class Person
        {
            public int Age;
            public string Name { get; set; }
        }

        protected class Book
        {
            public int YearPublished;
            public string Title { get; set; }
        }
    }
}
