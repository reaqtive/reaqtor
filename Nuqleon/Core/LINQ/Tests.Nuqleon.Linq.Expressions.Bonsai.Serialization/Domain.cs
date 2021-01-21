// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions.Bonsai.Serialization;
using Json = Nuqleon.Json.Expressions;

namespace Tests
{
    [TestClass]
    public class DomainTests
    {
        [TestMethod]
        public void Domain_Constructor_ThrowsInvalidOperation()
        {
            var exprs = new[]
            {

                #region Invalid Input

                @"null",

                #endregion

                #region Invalid Version

                @"{""Version"": null, ""Assemblies"": [], ""Types"": []}",

                #endregion

                #region Invalid Assemblies

                @"{""Types"": []}",
                @"{""Assemblies"": null, ""Types"": []}",
                @"{""Assemblies"": [null], ""Types"": []}",

                #endregion

                #region Invalid Types

                #region Invalid Type Input

                @"{""Assemblies"": []}",
                @"{""Assemblies"": [], ""Types"": null}",
                @"{""Assemblies"": [], ""Types"": [null]}",
                @"{""Assemblies"": [], ""Types"": [[]]}",
                @"{""Assemblies"": [], ""Types"": [[null]]}",
                @"{""Assemblies"": [], ""Types"": [[""foo""]]}",

                #endregion

                #region Invalid Simple Types

                @"{""Assemblies"": [], ""Types"": [[""::""]]}",
                @"{""Assemblies"": [], ""Types"": [[""::"", null, 0]]}",
                @"{""Assemblies"": [], ""Types"": [[""::"", ""Foo"", null]]}",

                #endregion

                #region Invalid Generic Types

                @"{""Assemblies"": [], ""Types"": [[""<>""]]}",
                @"{""Assemblies"": [], ""Types"": [[""<>"", null, [0]]]}",
                @"{""Assemblies"": [], ""Types"": [[""<>"", 0, null]]}",
                @"{""Assemblies"": [], ""Types"": [[""<>"", 0, []]]}",

                #endregion

                #region Invalid Array Types

                @"{""Assemblies"": [], ""Types"": [[""[]""]]}",
                @"{""Assemblies"": [], ""Types"": [[""[]"", 0, []]]}",
                @"{""Assemblies"": [], ""Types"": [[""[]"", 0, ""foo""]]}",

                #endregion

                #region Invalid Structural Types

                @"{""Assemblies"": [], ""Types"": [[""{}"", ""0""]]}",
                @"{""Assemblies"": [], ""Types"": [[""{}"", []]]}",
                @"{""Assemblies"": [], ""Types"": [[""{}"", [0,1]]]}",
                @"{""Assemblies"": [], ""Types"": [[""{}"", [""foo"",1,0]]]}",
                @"{""Version"": ""0.9.0.0"", ""Assemblies"": [], ""Types"": [[""{;}""]]}",
                @"{""Version"": ""0.9.0.0"", ""Assemblies"": [], ""Types"": [[""{;}"", 0]]}",
                @"{""Version"": ""0.9.0.0"", ""Assemblies"": [], ""Types"": [[""{;}"", [0]]]}",
                @"{""Version"": ""0.9.0.0"", ""Assemblies"": [], ""Types"": [[""{;}"", [[]]]]}",
                @"{""Version"": ""0.9.0.0"", ""Assemblies"": [], ""Types"": [[""{;}"", [], 0]]}",

                #endregion

                #endregion

                #region Invalid Members

                #region Invalid Member Input

                @"{""Assemblies"": [], ""Types"": [], ""Members"" : 0}",
                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [0]}",
                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [[]]}",
                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [[0, null]]}",
                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [[""foo"", null]]}",

                #endregion

                #region Invalid Constructor

                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [[""C""]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""C"", 0, 0]]}",

                #endregion

                #region Invalid Field

                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [[""F""]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""F"", 0, 0]]}",

                #endregion

                #region Invalid Property

                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [[""P""]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""P"", 0, 0]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""P"", 0, ""Foo"", 0]]}",

                #endregion

                #region Invalid Simple Method

                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [[""M""]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""M"", 0, 0, []]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""M"", 0, ""Foo"", 0]]}",

                #endregion

                #region Invalid Open Generic Method

                @"{""Assemblies"": [], ""Types"": [], ""Members"" : [[""M`""]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""M`"", 0, 0, 0, []]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""M`"", 0, ""Foo"", ""0"", []]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""M`"", 0, ""Foo"", 0, 0]]}",

                #endregion

                #region Invalid Closed Generic Method

                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""M<>""]]}",
                @"{""Assemblies"": [""Foo""], ""Types"": [[""::"", ""Foo"", 0]], ""Members"" : [[""M<>"", 0, 0]]}",

                #endregion

                #endregion
            };

            ConstructDomainAndThrow<BonsaiParseException>(exprs);
        }

        [TestMethod]
        public void Domain_Constructor_ThrowsNotSupported()
        {
            var exprs = new[]
            {
                @"{""Assemblies"": [], ""Types"": [[""{;}"", [], true]]}",
            };

            ConstructDomainAndThrow<NotSupportedException>(exprs);
        }

        [TestMethod]
        public void Domain_AddMember_Exceptions()
        {
            var domain = new SerializationDomain(BonsaiVersion.Default);
            AssertEx.ThrowsException<ArgumentNullException>(() => domain.AddMember(member: null), ex => Assert.AreEqual(ex.ParamName, "member"));
        }

        [TestMethod]
        public void Domain_GetMember_Exceptions()
        {
            var domain = GetDomain();
            AssertEx.ThrowsException<ArgumentNullException>(() => domain.GetMember(expression: null), ex => Assert.AreEqual(ex.ParamName, "expression"));
            Assert.ThrowsException<BonsaiParseException>(() => domain.GetMember(Json.Expression.Null()));
            Assert.ThrowsException<BonsaiParseException>(() => domain.GetMember(Json.Expression.Number("-1")));
            Assert.ThrowsException<BonsaiParseException>(() => domain.GetMember(Json.Expression.Number("99")));
        }

        [TestMethod]
        public void Domain_GetType_Exceptions()
        {
            var domain = GetDomain();
            AssertEx.ThrowsException<ArgumentNullException>(() => domain.GetType((Json.Expression)null), ex => Assert.AreEqual(ex.ParamName, "expression"));
            AssertEx.ThrowsException<ArgumentNullException>(() => domain.GetType((TypeRef)null), ex => Assert.AreEqual(ex.ParamName, "typeRef"));
            Assert.ThrowsException<BonsaiParseException>(() => domain.GetType(Json.Expression.Null()));
            Assert.ThrowsException<BonsaiParseException>(() => domain.GetType(Json.Expression.Number("99")));
        }

        private static DeserializationDomain GetDomain()
        {
            var json = (Json.ObjectExpression)Json.Expression.Parse(@"{""Context"": {""Types"": [[""::"", ""System.Int32"", 0], [""::"", ""System.Func`2"", 0], [""<>"", 1, [0, 0]]], ""Assemblies"": [""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""], ""Version"": ""0.9.0.0""}, ""Expression"": [""=>"", 2, [""*"", [""$"", 0, 0], ["":"", 2, 0]], [[0, ""x""]]]}");
            var context = json.Members["Context"];
            return new DeserializationDomain(context);
        }

        [TestMethod]
        public void Domain_SupportsVersion()
        {
            var domain = new SerializationDomain(new Version(1, 0, 0, 0));
            Assert.IsTrue(domain.SupportsVersion(new Version(1, 0)));
            Assert.IsTrue(domain.SupportsVersion(new Version(0, 9)));
            Assert.IsFalse(domain.SupportsVersion(new Version(2, 0)));
        }

        private static void ConstructDomainAndThrow<T>(IEnumerable<string> jsonExpressions)
            where T : Exception
        {
            foreach (var expr in jsonExpressions)
            {
                var json = Json.Expression.Parse(expr, ensureTopLevelObjectOrArray: false);
                Assert.ThrowsException<T>(() => new DeserializationDomain(json));
            }
        }
    }
}
