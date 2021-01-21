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
        public void FastParser_Single()
        {
            AssertParse(Parser.ParseSingle, "0", 0f);

            AssertParse(Parser.ParseSingle, "1", 1f);
            AssertParse(Parser.ParseSingle, "-1", -1f);
            AssertParse(Parser.ParseSingle, "42", 42f);
            AssertParse(Parser.ParseSingle, "-42", -42f);

            AssertParse(Parser.ParseSingle, "0.1", 0.1f);
            AssertParse(Parser.ParseSingle, "-0.1", -0.1f);
            AssertParse(Parser.ParseSingle, "98.75", 98.75f);
            AssertParse(Parser.ParseSingle, "-98.75", -98.75f);

            AssertParse(Parser.ParseSingle, "367e0", 367e0f);
            AssertParse(Parser.ParseSingle, "367e1", 367e1f);
            AssertParse(Parser.ParseSingle, "367e2", 367e2f);
            AssertParse(Parser.ParseSingle, "367E0", 367e0f);
            AssertParse(Parser.ParseSingle, "367E1", 367e1f);
            AssertParse(Parser.ParseSingle, "367E2", 367e2f);
            AssertParse(Parser.ParseSingle, "367E34", 367e34f);
            AssertParse(Parser.ParseSingle, "367e+0", 367e0f);
            AssertParse(Parser.ParseSingle, "367e+1", 367e1f);
            AssertParse(Parser.ParseSingle, "367e+2", 367e2f);
            AssertParse(Parser.ParseSingle, "367e+34", 367e34f);
            AssertParse(Parser.ParseSingle, "367E+0", 367e0f);
            AssertParse(Parser.ParseSingle, "367E+1", 367e1f);
            AssertParse(Parser.ParseSingle, "367E+2", 367e2f);
            AssertParse(Parser.ParseSingle, "367E+34", 367e34f);
            AssertParse(Parser.ParseSingle, "367e-0", 367e-0f);
            AssertParse(Parser.ParseSingle, "367e-1", 367e-1f);
            AssertParse(Parser.ParseSingle, "367e-2", 367e-2f);
            AssertParse(Parser.ParseSingle, "367E-0", 367e-0f);
            AssertParse(Parser.ParseSingle, "367E-1", 367e-1f);
            AssertParse(Parser.ParseSingle, "367E-2", 367e-2f);

            AssertParseFail(Parser.ParseSingle, "");

            AssertParseFail(Parser.ParseSingle, "-");

            AssertParseFail(Parser.ParseSingle, "0.");
            AssertParseFail(Parser.ParseSingle, "0.a");

            AssertParseFail(Parser.ParseSingle, "1e");
            AssertParseFail(Parser.ParseSingle, "1E");
            AssertParseFail(Parser.ParseSingle, "1e+");
            AssertParseFail(Parser.ParseSingle, "1E+");
            AssertParseFail(Parser.ParseSingle, "1e-");
            AssertParseFail(Parser.ParseSingle, "1E-");
            AssertParseFail(Parser.ParseSingle, "1e+a");
            AssertParseFail(Parser.ParseSingle, "1E+a");
            AssertParseFail(Parser.ParseSingle, "1e-a");
            AssertParseFail(Parser.ParseSingle, "1E-a");

            AssertParseFail(Parser.ParseSingle, "null");
        }

        [TestMethod]
        public void FastParser_NullableSingle()
        {
            AssertParse(Parser.ParseNullableSingle, "null", null);

            AssertParse(Parser.ParseNullableSingle, "0", 0f);

            AssertParse(Parser.ParseNullableSingle, "1", 1f);
            AssertParse(Parser.ParseNullableSingle, "-1", -1f);
            AssertParse(Parser.ParseNullableSingle, "42", 42f);
            AssertParse(Parser.ParseNullableSingle, "-42", -42f);

            AssertParse(Parser.ParseNullableSingle, "0.1", 0.1f);
            AssertParse(Parser.ParseNullableSingle, "-0.1", -0.1f);
            AssertParse(Parser.ParseNullableSingle, "98.75", 98.75f);
            AssertParse(Parser.ParseNullableSingle, "-98.75", -98.75f);

            AssertParse(Parser.ParseNullableSingle, "367e0", 367e0f);
            AssertParse(Parser.ParseNullableSingle, "367e1", 367e1f);
            AssertParse(Parser.ParseNullableSingle, "367e2", 367e2f);
            AssertParse(Parser.ParseNullableSingle, "367E0", 367e0f);
            AssertParse(Parser.ParseNullableSingle, "367E1", 367e1f);
            AssertParse(Parser.ParseNullableSingle, "367E2", 367e2f);
            AssertParse(Parser.ParseNullableSingle, "367E34", 367e34f);
            AssertParse(Parser.ParseNullableSingle, "367e+0", 367e0f);
            AssertParse(Parser.ParseNullableSingle, "367e+1", 367e1f);
            AssertParse(Parser.ParseNullableSingle, "367e+2", 367e2f);
            AssertParse(Parser.ParseNullableSingle, "367e+34", 367e34f);
            AssertParse(Parser.ParseNullableSingle, "367E+0", 367e0f);
            AssertParse(Parser.ParseNullableSingle, "367E+1", 367e1f);
            AssertParse(Parser.ParseNullableSingle, "367E+2", 367e2f);
            AssertParse(Parser.ParseNullableSingle, "367E+34", 367e34f);
            AssertParse(Parser.ParseNullableSingle, "367e-0", 367e-0f);
            AssertParse(Parser.ParseNullableSingle, "367e-1", 367e-1f);
            AssertParse(Parser.ParseNullableSingle, "367e-2", 367e-2f);
            AssertParse(Parser.ParseNullableSingle, "367E-0", 367e-0f);
            AssertParse(Parser.ParseNullableSingle, "367E-1", 367e-1f);
            AssertParse(Parser.ParseNullableSingle, "367E-2", 367e-2f);

            AssertParseFail(Parser.ParseNullableSingle, "");

            AssertParseFail(Parser.ParseNullableSingle, "-");

            AssertParseFail(Parser.ParseNullableSingle, "0.");
            AssertParseFail(Parser.ParseNullableSingle, "0.a");

            AssertParseFail(Parser.ParseNullableSingle, "1e");
            AssertParseFail(Parser.ParseNullableSingle, "1E");
            AssertParseFail(Parser.ParseNullableSingle, "1e+");
            AssertParseFail(Parser.ParseNullableSingle, "1E+");
            AssertParseFail(Parser.ParseNullableSingle, "1e-");
            AssertParseFail(Parser.ParseNullableSingle, "1E-");
            AssertParseFail(Parser.ParseNullableSingle, "1e+a");
            AssertParseFail(Parser.ParseNullableSingle, "1E+a");
            AssertParseFail(Parser.ParseNullableSingle, "1e-a");
            AssertParseFail(Parser.ParseNullableSingle, "1E-a");

            AssertParseFail(Parser.ParseNullableSingle, "n");
            AssertParseFail(Parser.ParseNullableSingle, "nu");
            AssertParseFail(Parser.ParseNullableSingle, "nul");

            AssertParseFail(Parser.ParseNullableSingle, "Null");
            AssertParseFail(Parser.ParseNullableSingle, "nUll");
            AssertParseFail(Parser.ParseNullableSingle, "nuLl");
            AssertParseFail(Parser.ParseNullableSingle, "nulL");
        }

        [TestMethod]
        public void FastParser_Double()
        {
            AssertParse(Parser.ParseDouble, "0", 0d);

            AssertParse(Parser.ParseDouble, "1", 1d);
            AssertParse(Parser.ParseDouble, "-1", -1d);
            AssertParse(Parser.ParseDouble, "42", 42d);
            AssertParse(Parser.ParseDouble, "-42", -42d);

            AssertParse(Parser.ParseDouble, "0.1", 0.1d);
            AssertParse(Parser.ParseDouble, "-0.1", -0.1d);
            AssertParse(Parser.ParseDouble, "98.75", 98.75d);
            AssertParse(Parser.ParseDouble, "-98.75", -98.75d);

            AssertParse(Parser.ParseDouble, "367e0", 367e0d);
            AssertParse(Parser.ParseDouble, "367e1", 367e1d);
            AssertParse(Parser.ParseDouble, "367e2", 367e2d);
            AssertParse(Parser.ParseDouble, "367E0", 367e0d);
            AssertParse(Parser.ParseDouble, "367E1", 367e1d);
            AssertParse(Parser.ParseDouble, "367E2", 367e2d);
            AssertParse(Parser.ParseDouble, "367E34", 367e34d);
            AssertParse(Parser.ParseDouble, "367e+0", 367e0d);
            AssertParse(Parser.ParseDouble, "367e+1", 367e1d);
            AssertParse(Parser.ParseDouble, "367e+2", 367e2d);
            AssertParse(Parser.ParseDouble, "367e+34", 367e34d);
            AssertParse(Parser.ParseDouble, "367E+0", 367e0d);
            AssertParse(Parser.ParseDouble, "367E+1", 367e1d);
            AssertParse(Parser.ParseDouble, "367E+2", 367e2d);
            AssertParse(Parser.ParseDouble, "367E+34", 367e34d);
            AssertParse(Parser.ParseDouble, "367e-0", 367e-0d);
            AssertParse(Parser.ParseDouble, "367e-1", 367e-1d);
            AssertParse(Parser.ParseDouble, "367e-2", 367e-2d);
            AssertParse(Parser.ParseDouble, "367E-0", 367e-0d);
            AssertParse(Parser.ParseDouble, "367E-1", 367e-1d);
            AssertParse(Parser.ParseDouble, "367E-2", 367e-2d);

            AssertParseFail(Parser.ParseDouble, "");

            AssertParseFail(Parser.ParseDouble, "-");

            AssertParseFail(Parser.ParseDouble, "0.");
            AssertParseFail(Parser.ParseDouble, "0.a");

            AssertParseFail(Parser.ParseDouble, "1e");
            AssertParseFail(Parser.ParseDouble, "1E");
            AssertParseFail(Parser.ParseDouble, "1e+");
            AssertParseFail(Parser.ParseDouble, "1E+");
            AssertParseFail(Parser.ParseDouble, "1e-");
            AssertParseFail(Parser.ParseDouble, "1E-");
            AssertParseFail(Parser.ParseDouble, "1e+a");
            AssertParseFail(Parser.ParseDouble, "1E+a");
            AssertParseFail(Parser.ParseDouble, "1e-a");
            AssertParseFail(Parser.ParseDouble, "1E-a");

            AssertParseFail(Parser.ParseDouble, "null");
        }

        [TestMethod]
        public void FastParser_NullableDouble()
        {
            AssertParse(Parser.ParseNullableDouble, "null", null);

            AssertParse(Parser.ParseNullableDouble, "0", 0d);

            AssertParse(Parser.ParseNullableDouble, "1", 1d);
            AssertParse(Parser.ParseNullableDouble, "-1", -1d);
            AssertParse(Parser.ParseNullableDouble, "42", 42d);
            AssertParse(Parser.ParseNullableDouble, "-42", -42d);

            AssertParse(Parser.ParseNullableDouble, "0.1", 0.1d);
            AssertParse(Parser.ParseNullableDouble, "-0.1", -0.1d);
            AssertParse(Parser.ParseNullableDouble, "98.75", 98.75d);
            AssertParse(Parser.ParseNullableDouble, "-98.75", -98.75d);

            AssertParse(Parser.ParseNullableDouble, "367e0", 367e0d);
            AssertParse(Parser.ParseNullableDouble, "367e1", 367e1d);
            AssertParse(Parser.ParseNullableDouble, "367e2", 367e2d);
            AssertParse(Parser.ParseNullableDouble, "367E0", 367e0d);
            AssertParse(Parser.ParseNullableDouble, "367E1", 367e1d);
            AssertParse(Parser.ParseNullableDouble, "367E2", 367e2d);
            AssertParse(Parser.ParseNullableDouble, "367E34", 367e34d);
            AssertParse(Parser.ParseNullableDouble, "367e+0", 367e0d);
            AssertParse(Parser.ParseNullableDouble, "367e+1", 367e1d);
            AssertParse(Parser.ParseNullableDouble, "367e+2", 367e2d);
            AssertParse(Parser.ParseNullableDouble, "367e+34", 367e34d);
            AssertParse(Parser.ParseNullableDouble, "367E+0", 367e0d);
            AssertParse(Parser.ParseNullableDouble, "367E+1", 367e1d);
            AssertParse(Parser.ParseNullableDouble, "367E+2", 367e2d);
            AssertParse(Parser.ParseNullableDouble, "367E+34", 367e34d);
            AssertParse(Parser.ParseNullableDouble, "367e-0", 367e-0d);
            AssertParse(Parser.ParseNullableDouble, "367e-1", 367e-1d);
            AssertParse(Parser.ParseNullableDouble, "367e-2", 367e-2d);
            AssertParse(Parser.ParseNullableDouble, "367E-0", 367e-0d);
            AssertParse(Parser.ParseNullableDouble, "367E-1", 367e-1d);
            AssertParse(Parser.ParseNullableDouble, "367E-2", 367e-2d);

            AssertParseFail(Parser.ParseNullableDouble, "");

            AssertParseFail(Parser.ParseNullableDouble, "-");

            AssertParseFail(Parser.ParseNullableDouble, "0.");
            AssertParseFail(Parser.ParseNullableDouble, "0.a");

            AssertParseFail(Parser.ParseNullableDouble, "1e");
            AssertParseFail(Parser.ParseNullableDouble, "1E");
            AssertParseFail(Parser.ParseNullableDouble, "1e+");
            AssertParseFail(Parser.ParseNullableDouble, "1E+");
            AssertParseFail(Parser.ParseNullableDouble, "1e-");
            AssertParseFail(Parser.ParseNullableDouble, "1E-");
            AssertParseFail(Parser.ParseNullableDouble, "1e+a");
            AssertParseFail(Parser.ParseNullableDouble, "1E+a");
            AssertParseFail(Parser.ParseNullableDouble, "1e-a");
            AssertParseFail(Parser.ParseNullableDouble, "1E-a");

            AssertParseFail(Parser.ParseNullableDouble, "n");
            AssertParseFail(Parser.ParseNullableDouble, "nu");
            AssertParseFail(Parser.ParseNullableDouble, "nul");

            AssertParseFail(Parser.ParseNullableDouble, "Null");
            AssertParseFail(Parser.ParseNullableDouble, "nUll");
            AssertParseFail(Parser.ParseNullableDouble, "nuLl");
            AssertParseFail(Parser.ParseNullableDouble, "nulL");
        }

        [TestMethod]
        public void FastParser_Decimal()
        {
            AssertParse(Parser.ParseDecimal, "0", 0m);

            AssertParse(Parser.ParseDecimal, "1", 1m);
            AssertParse(Parser.ParseDecimal, "-1", -1m);
            AssertParse(Parser.ParseDecimal, "42", 42m);
            AssertParse(Parser.ParseDecimal, "-42", -42m);

            AssertParse(Parser.ParseDecimal, "0.1", 0.1m);
            AssertParse(Parser.ParseDecimal, "-0.1", -0.1m);
            AssertParse(Parser.ParseDecimal, "98.75", 98.75m);
            AssertParse(Parser.ParseDecimal, "-98.75", -98.75m);

            AssertParse(Parser.ParseDecimal, "367e0", 367e0m);
            AssertParse(Parser.ParseDecimal, "367e1", 367e1m);
            AssertParse(Parser.ParseDecimal, "367e2", 367e2m);
            AssertParse(Parser.ParseDecimal, "367E0", 367e0m);
            AssertParse(Parser.ParseDecimal, "367E1", 367e1m);
            AssertParse(Parser.ParseDecimal, "367E2", 367e2m);
            AssertParse(Parser.ParseDecimal, "367E21", 367e21m);
            AssertParse(Parser.ParseDecimal, "367e+0", 367e0m);
            AssertParse(Parser.ParseDecimal, "367e+1", 367e1m);
            AssertParse(Parser.ParseDecimal, "367e+2", 367e2m);
            AssertParse(Parser.ParseDecimal, "367e+21", 367e21m);
            AssertParse(Parser.ParseDecimal, "367E+0", 367e0m);
            AssertParse(Parser.ParseDecimal, "367E+1", 367e1m);
            AssertParse(Parser.ParseDecimal, "367E+2", 367e2m);
            AssertParse(Parser.ParseDecimal, "367E+21", 367e21m);
            AssertParse(Parser.ParseDecimal, "367e-0", 367e-0m);
            AssertParse(Parser.ParseDecimal, "367e-1", 367e-1m);
            AssertParse(Parser.ParseDecimal, "367e-2", 367e-2m);
            AssertParse(Parser.ParseDecimal, "367E-0", 367e-0m);
            AssertParse(Parser.ParseDecimal, "367E-1", 367e-1m);
            AssertParse(Parser.ParseDecimal, "367E-2", 367e-2m);

            AssertParseFail(Parser.ParseDecimal, "");

            AssertParseFail(Parser.ParseDecimal, "-");

            AssertParseFail(Parser.ParseDecimal, "0.");
            AssertParseFail(Parser.ParseDecimal, "0.a");

            AssertParseFail(Parser.ParseDecimal, "1e");
            AssertParseFail(Parser.ParseDecimal, "1E");
            AssertParseFail(Parser.ParseDecimal, "1e+");
            AssertParseFail(Parser.ParseDecimal, "1E+");
            AssertParseFail(Parser.ParseDecimal, "1e-");
            AssertParseFail(Parser.ParseDecimal, "1E-");
            AssertParseFail(Parser.ParseDecimal, "1e+a");
            AssertParseFail(Parser.ParseDecimal, "1E+a");
            AssertParseFail(Parser.ParseDecimal, "1e-a");
            AssertParseFail(Parser.ParseDecimal, "1E-a");

            AssertParseFail(Parser.ParseDecimal, "null");
        }

        [TestMethod]
        public void FastParser_NullableDecimal()
        {
            AssertParse(Parser.ParseNullableDecimal, "null", null);

            AssertParse(Parser.ParseNullableDecimal, "0", 0m);

            AssertParse(Parser.ParseNullableDecimal, "1", 1m);
            AssertParse(Parser.ParseNullableDecimal, "-1", -1m);
            AssertParse(Parser.ParseNullableDecimal, "42", 42m);
            AssertParse(Parser.ParseNullableDecimal, "-42", -42m);

            AssertParse(Parser.ParseNullableDecimal, "0.1", 0.1m);
            AssertParse(Parser.ParseNullableDecimal, "-0.1", -0.1m);
            AssertParse(Parser.ParseNullableDecimal, "98.75", 98.75m);
            AssertParse(Parser.ParseNullableDecimal, "-98.75", -98.75m);

            AssertParse(Parser.ParseNullableDecimal, "367e0", 367e0m);
            AssertParse(Parser.ParseNullableDecimal, "367e1", 367e1m);
            AssertParse(Parser.ParseNullableDecimal, "367e2", 367e2m);
            AssertParse(Parser.ParseNullableDecimal, "367E0", 367e0m);
            AssertParse(Parser.ParseNullableDecimal, "367E1", 367e1m);
            AssertParse(Parser.ParseNullableDecimal, "367E2", 367e2m);
            AssertParse(Parser.ParseNullableDecimal, "367E21", 367e21m);
            AssertParse(Parser.ParseNullableDecimal, "367e+0", 367e0m);
            AssertParse(Parser.ParseNullableDecimal, "367e+1", 367e1m);
            AssertParse(Parser.ParseNullableDecimal, "367e+2", 367e2m);
            AssertParse(Parser.ParseNullableDecimal, "367e+21", 367e21m);
            AssertParse(Parser.ParseNullableDecimal, "367E+0", 367e0m);
            AssertParse(Parser.ParseNullableDecimal, "367E+1", 367e1m);
            AssertParse(Parser.ParseNullableDecimal, "367E+2", 367e2m);
            AssertParse(Parser.ParseNullableDecimal, "367E+21", 367e21m);
            AssertParse(Parser.ParseNullableDecimal, "367e-0", 367e-0m);
            AssertParse(Parser.ParseNullableDecimal, "367e-1", 367e-1m);
            AssertParse(Parser.ParseNullableDecimal, "367e-2", 367e-2m);
            AssertParse(Parser.ParseNullableDecimal, "367E-0", 367e-0m);
            AssertParse(Parser.ParseNullableDecimal, "367E-1", 367e-1m);
            AssertParse(Parser.ParseNullableDecimal, "367E-2", 367e-2m);

            AssertParseFail(Parser.ParseNullableDecimal, "");

            AssertParseFail(Parser.ParseNullableDecimal, "-");

            AssertParseFail(Parser.ParseNullableDecimal, "0.");
            AssertParseFail(Parser.ParseNullableDecimal, "0.a");

            AssertParseFail(Parser.ParseNullableDecimal, "1e");
            AssertParseFail(Parser.ParseNullableDecimal, "1E");
            AssertParseFail(Parser.ParseNullableDecimal, "1e+");
            AssertParseFail(Parser.ParseNullableDecimal, "1E+");
            AssertParseFail(Parser.ParseNullableDecimal, "1e-");
            AssertParseFail(Parser.ParseNullableDecimal, "1E-");
            AssertParseFail(Parser.ParseNullableDecimal, "1e+a");
            AssertParseFail(Parser.ParseNullableDecimal, "1E+a");
            AssertParseFail(Parser.ParseNullableDecimal, "1e-a");
            AssertParseFail(Parser.ParseNullableDecimal, "1E-a");

            AssertParseFail(Parser.ParseNullableDecimal, "n");
            AssertParseFail(Parser.ParseNullableDecimal, "nu");
            AssertParseFail(Parser.ParseNullableDecimal, "nul");

            AssertParseFail(Parser.ParseNullableDecimal, "Null");
            AssertParseFail(Parser.ParseNullableDecimal, "nUll");
            AssertParseFail(Parser.ParseNullableDecimal, "nuLl");
            AssertParseFail(Parser.ParseNullableDecimal, "nulL");
        }
    }
}
