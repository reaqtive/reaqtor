// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Json = Nuqleon.Json.Expressions;

namespace Tests
{
    [TestClass]
    public class JsonTests
    {
#pragma warning disable IDE0100 // Remove redundant equality (clarity for Boolean asserts)

        [TestMethod]
        public void Constant_Null()
        {
            var t = Json.Expression.Null();
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Null);
        }

        [TestMethod]
        public void Constant_Bool_True()
        {
            var t = Json.Expression.Boolean(true);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)o).Value);
        }

        [TestMethod]
        public void Constant_Bool_False()
        {
            var t = Json.Expression.Boolean(false);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Boolean && !(bool)((Json.ConstantExpression)o).Value);
        }

        [TestMethod]
        public void Constant_Number_Int()
        {
            var t = Json.Expression.Number("42");
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)o).Value == "42");
        }

        [TestMethod]
        public void Constant_String()
        {
            var s = "Hello, \t \"World\"! \r\n \b \\ \f";

            var t = Json.Expression.String(s);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)o).Value == s);
        }

        [TestMethod]
        public void Constant_String_Null()
        {
            var s = default(string);

            var t = Json.Expression.String(s);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Null);
        }

        [TestMethod]
        public void Array_Empty()
        {
            var t = Json.Expression.Array();
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array && ((Json.ArrayExpression)o).Elements.Count == 0);
        }

        [TestMethod]
        public void Array_One()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"));
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);
            var arr = (Json.ArrayExpression)o;
            Assert.IsTrue(arr.Elements.Count == 1);
            Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");
        }

        [TestMethod]
        public void Array_Many()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"), Json.Expression.Boolean(true), Json.Expression.String("Hello"));
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);
            var arr = (Json.ArrayExpression)o;
            Assert.IsTrue(arr.Elements.Count == 3);
            Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");
            Assert.IsTrue(arr.Elements[1].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[1]).Value == true);
            Assert.IsTrue(arr.Elements[2].NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)arr.Elements[2]).Value == "Hello");
        }

        [TestMethod]
        public void Array_Nested()
        {
            var t = Json.Expression.Array(Json.Expression.Array(Json.Expression.Boolean(true)));
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);
            var arr = (Json.ArrayExpression)o;
            Assert.IsTrue(arr.Elements.Count == 1);
            Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Array);
            var nst = (Json.ArrayExpression)arr.Elements[0];
            Assert.IsTrue(nst.Elements.Count == 1);
            Assert.IsTrue(nst.Elements[0].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)nst.Elements[0]).Value == true);
        }

        [TestMethod]
        public void Object()
        {
            var t = Json.Expression.Object(new Dictionary<string, Json.Expression>
            {
                { "Name", Json.Expression.String("Bart") },
                { "Age", Json.Expression.Number("21") },
                { "Weird \"Property\" \t\r\n", Json.Expression.Boolean(true) },
            });
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Object);
            var obj = (Json.ObjectExpression)o;

            Assert.IsTrue(obj.Members.Keys.Count() == 3);
            Assert.IsTrue((string)((Json.ConstantExpression)obj.Members["Name"]).Value == "Bart");
            Assert.IsTrue((string)((Json.ConstantExpression)obj.Members["Age"]).Value == "21");
            Assert.IsTrue((bool)((Json.ConstantExpression)obj.Members["Weird \"Property\" \t\r\n"]).Value == true);
        }

#pragma warning restore IDE0100 // Remove redundant equality
    }
}
