// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nuqleon.Json.Serialization;

namespace Tests
{
    public partial class ParserTests
    {
        [TestMethod]
        public void FastParser_Boolean()
        {
            AssertParse(Parser.ParseBoolean, "true", true);
            AssertParse(Parser.ParseBoolean, "false", false);

            AssertParseFail(Parser.ParseBoolean, "");
            AssertParseFail(Parser.ParseBoolean, " ");

            AssertParseFail(Parser.ParseBoolean, "t");
            AssertParseFail(Parser.ParseBoolean, "tr");
            AssertParseFail(Parser.ParseBoolean, "tru");

            AssertParseFail(Parser.ParseBoolean, "True");
            AssertParseFail(Parser.ParseBoolean, "tRue");
            AssertParseFail(Parser.ParseBoolean, "trUe");
            AssertParseFail(Parser.ParseBoolean, "truE");

            AssertParseFail(Parser.ParseBoolean, "f");
            AssertParseFail(Parser.ParseBoolean, "fa");
            AssertParseFail(Parser.ParseBoolean, "fal");
            AssertParseFail(Parser.ParseBoolean, "fals");

            AssertParseFail(Parser.ParseBoolean, "False");
            AssertParseFail(Parser.ParseBoolean, "fAlse");
            AssertParseFail(Parser.ParseBoolean, "faLse");
            AssertParseFail(Parser.ParseBoolean, "falSe");
            AssertParseFail(Parser.ParseBoolean, "falsE");

            AssertParseFail(Parser.ParseBoolean, "42");
            AssertParseFail(Parser.ParseBoolean, "-42");

            AssertParseFail(Parser.ParseBoolean, "[");
            AssertParseFail(Parser.ParseBoolean, "]");
            AssertParseFail(Parser.ParseBoolean, "{");
            AssertParseFail(Parser.ParseBoolean, "}");
            AssertParseFail(Parser.ParseBoolean, ",");
            AssertParseFail(Parser.ParseBoolean, ":");

            AssertParseFail(Parser.ParseBoolean, "null");
        }

        [TestMethod]
        public void FastParser_NullableBoolean()
        {
            AssertParse(Parser.ParseNullableBoolean, "null", null);
            AssertParse(Parser.ParseNullableBoolean, "true", true);
            AssertParse(Parser.ParseNullableBoolean, "false", false);

            AssertParseFail(Parser.ParseNullableBoolean, "");
            AssertParseFail(Parser.ParseNullableBoolean, " ");

            AssertParseFail(Parser.ParseNullableBoolean, "t");
            AssertParseFail(Parser.ParseNullableBoolean, "tr");
            AssertParseFail(Parser.ParseNullableBoolean, "tru");

            AssertParseFail(Parser.ParseNullableBoolean, "tRue");
            AssertParseFail(Parser.ParseNullableBoolean, "trUe");
            AssertParseFail(Parser.ParseNullableBoolean, "truE");

            AssertParseFail(Parser.ParseNullableBoolean, "f");
            AssertParseFail(Parser.ParseNullableBoolean, "fa");
            AssertParseFail(Parser.ParseNullableBoolean, "fal");
            AssertParseFail(Parser.ParseNullableBoolean, "fals");

            AssertParseFail(Parser.ParseNullableBoolean, "False");
            AssertParseFail(Parser.ParseNullableBoolean, "fAlse");
            AssertParseFail(Parser.ParseNullableBoolean, "faLse");
            AssertParseFail(Parser.ParseNullableBoolean, "falSe");
            AssertParseFail(Parser.ParseNullableBoolean, "falsE");

            AssertParseFail(Parser.ParseNullableBoolean, "n");
            AssertParseFail(Parser.ParseNullableBoolean, "nu");
            AssertParseFail(Parser.ParseNullableBoolean, "nul");

            AssertParseFail(Parser.ParseNullableBoolean, "Null");
            AssertParseFail(Parser.ParseNullableBoolean, "nUll");
            AssertParseFail(Parser.ParseNullableBoolean, "nuLl");
            AssertParseFail(Parser.ParseNullableBoolean, "nulL");

            AssertParseFail(Parser.ParseNullableBoolean, "42");
            AssertParseFail(Parser.ParseNullableBoolean, "-42");

            AssertParseFail(Parser.ParseNullableBoolean, "[");
            AssertParseFail(Parser.ParseNullableBoolean, "]");
            AssertParseFail(Parser.ParseNullableBoolean, "{");
            AssertParseFail(Parser.ParseNullableBoolean, "}");
            AssertParseFail(Parser.ParseNullableBoolean, ",");
            AssertParseFail(Parser.ParseNullableBoolean, ":");
        }
    }
}
