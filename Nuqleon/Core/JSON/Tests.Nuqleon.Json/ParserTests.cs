// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Reflection;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Expressions;
using Nuqleon.Json.Parser;

namespace Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Parser_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => JsonParser.Parse(input: null));
            Assert.ThrowsException<ArgumentNullException>(() => JsonParser.Parse(input: null, ensureTopLevelObjectOrArray: false));
        }

        [TestMethod]
        public void Parser_Roundtrip()
        {
            foreach (var json in new[] {
                @"0",
                @"1",
                @"-321",
                @"-0.98",
                @"12345.67890",
                @"1.0",
                @"1.0e123",
                @"1.0e-123",
                @"1.0e1",
                @"1.0e-1",
                @"1.0e123",
                @"1.0E123",
                @"1.0E-123",
                @"1.0E1",
                @"1.0E-1",
                @"1.0E123",
                @"-1.0",
                @"-1.0e123",
                @"-1.0e-123",
                @"-1.0e1",
                @"-1.0e-1",
                @"-1.0e123",
                @"-1.0E123",
                @"-1.0E-123",
                @"-1.0E1",
                @"-1.0E-1",
                @"-1.0E123",
                @"0e123",
                @"-0e123",
                @"0E123",
                @"-0E123",

                @"""""",
                @"""bar""",
                @"""foo bar""",
                @"""this tab \t doesn't return \r the \""quoted\"" carriage \t but may space \b back the / solidus and its reverse \\ brother to feed \f the form""",

                @"true",
                @"false",
                @"null",

                @"[]",
                @"[1]",
                @"[1,2,3]",
                @"[1,[2,3]]",
                @"[1,[2],3]",

                @"{}",
                @"{""bar"":42,""qux"":""foo""}",
            })
            {
                var res = JsonParser.Parse(json, ensureTopLevelObjectOrArray: false).ToString();
                Assert.AreEqual(json, res, "Failed for \"" + json + "\".");
            }

            var str1 = @"""This \/ should not \u0123 fail \u12ab to parse \uAB98 back and forth!""";
            var res1 = JsonParser.Parse(str1, ensureTopLevelObjectOrArray: false);
            var str2 = res1.ToString();
            var res2 = JsonParser.Parse(str2, ensureTopLevelObjectOrArray: false);

            var exp = "\"This / should not \u0123 fail \u12ab to parse \uAB98 back and forth!\"";

            Assert.AreEqual(exp, str2);
            Assert.AreEqual(exp, res2.ToString());
        }

        [TestMethod]
        public void Parser_Assembly()
        {
            Type type = typeof(JsonParser);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Json", assembly);
        }

        [TestMethod]
        public void Parser_Errors()
        {
            Assert.ThrowsException<ParseException>(() => JsonParser.Parse(@"123", ensureTopLevelObjectOrArray: true));
            Assert.ThrowsException<ParseException>(() => JsonParser.Parse(@"""foo""", ensureTopLevelObjectOrArray: true));
            Assert.ThrowsException<ParseException>(() => JsonParser.Parse(@"false", ensureTopLevelObjectOrArray: true));
            Assert.ThrowsException<ParseException>(() => JsonParser.Parse(@"true", ensureTopLevelObjectOrArray: true));
            Assert.ThrowsException<ParseException>(() => JsonParser.Parse(@"null", ensureTopLevelObjectOrArray: true));

            var assert = new Action<string>(json =>
            {
                Assert.ThrowsException<ParseException>(() => JsonParser.Parse(json, ensureTopLevelObjectOrArray: false), json);
            });

            assert(@"n");
            assert(@"nu");
            assert(@"nul");
            assert(@"nulll");
            assert(@"nuel");

            assert(@"t");
            assert(@"tr");
            assert(@"tru");
            assert(@"truee");
            assert(@"treu");

            assert(@"f");
            assert(@"fa");
            assert(@"fal");
            assert(@"fals");
            assert(@"falsee");
            assert(@"fasle");

            assert(@"");
            assert(@"""Hello, this is wrong!");
            assert(@"Hello, ""this"" is wrong!");
            assert(@"""Hello, this is wrong""!");
            assert(@"""Thou shalt escape completely\");

            assert(@"""Invalid Unicode coming up: \u1");
            assert(@"""Invalid Unicode coming up: \u12");
            assert(@"""Invalid Unicode coming up: \u123");
            assert(@"""Invalid Unicode coming up: \u12345"); // NOK - string not terminated

            assert(@"""Invalid Unicode coming up: \u1 .""");
            assert(@"""Invalid Unicode coming up: \u12 .""");
            assert(@"""Invalid Unicode coming up: \u123 .""");
            //assert(@"""Invalid Unicode coming up: \u12345 ."""); // OK - 5 is the next character

            assert(@"""Invalid Unicode coming up: \u1.""");
            assert(@"""Invalid Unicode coming up: \u12.""");
            assert(@"""Invalid Unicode coming up: \u123.""");
            //assert(@"""Invalid Unicode coming up: \u12345."""); // OK - 5 is the next character

            assert(@"""Invalid Unicode coming up: \ug239");
            assert(@"""Invalid Unicode coming up: \u1g39");
            assert(@"""Invalid Unicode coming up: \u12g9");
            assert(@"""Invalid Unicode coming up: \u123g");

            assert(@"\u1");
            assert(@"\u12");
            assert(@"\u123");
            assert(@"\u12345");
            assert(@"\ug239");
            assert(@"\u1g39");
            assert(@"\u12g9");
            assert(@"\u123g");

            assert(@"""Not a valid escape: \a""");
            assert(@"""Not a valid escape: \d""");
            assert(@"""Not a valid escape: \j""");
            assert(@"""Not a valid escape: \z""");

            assert("\"Control character \b not expected.\"");

            assert(@"-");
            assert(@"+");
            assert(@"01");
            assert(@"-01");
            assert(@"42.");
            assert(@"42..");
            assert(@".42");
            assert(@"..42");
            assert(@"42..42");
            assert(@"--42");
            assert(@"42e");
            assert(@"42e+");
            assert(@"42e-");

            assert(@"[");
            assert(@"]");
            assert(@"[1");
            assert(@"[1,2,]");
            assert(@"[1,,2]");
            assert(@"[,1,2]");
            assert(@"[1 2]");

            assert(@"{");
            assert(@"}");
            assert(@"{a");
            assert(@"{1");
            assert(@"{ a");
            assert(@"{ 1");
            assert(@"{ ""a");
            assert(@"{ ""a""");
            assert(@"{ ""a"":");
            assert(@"{ ""a"": 123");
            assert(@"{ ""a"" 123");
            assert(@"{ ""a"": 123,");
            assert(@"{ ""a"": 123, }");
            assert(@"{ ""a: 123, }");
            assert(@"{ ""a"": 123 ""b"": 321 }");
            assert(@"{ ""a"": 123,, ""b"": 321 }");
            assert(@"{ , ""a"": 123, ""b"": 321 }");
            assert(@"{ ""a"": 123, }");
        }

        [TestMethod]
        public void Parser_E2E()
        {
            var asm = Assembly.GetExecutingAssembly();
            var stream = asm.GetManifestResourceStream("Tests.Nuqleon.Json.JsonFragments.xml");

            using (stream)
            {
                var doc = XDocument.Load(stream);

                var templates = doc.Root.Descendants("Template");

                foreach (var template in templates)
                {
                    var json = template.Value;
                    var res = JsonParser.Parse(json);

                    Assert.AreEqual(ExpressionType.Object, res.NodeType);

                    var obj = (ObjectExpression)res;
                    Assert.IsTrue(obj.Members.ContainsKey("Context"));
                    Assert.IsTrue(obj.Members.ContainsKey("Expression"));
                }
            }
        }
    }
}
