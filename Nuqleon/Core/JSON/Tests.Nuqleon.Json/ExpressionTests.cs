// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Json = Nuqleon.Json.Expressions;

namespace Tests
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void Expression_ArgumentChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(default(Json.Expression[])));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(default(IEnumerable<Json.Expression>)));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(default(IList<Json.Expression>)));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Object(members: null));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Parse(input: null));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Parse(input: null, ensureTopLevelObjectOrArray: false));

            var n = default(Json.Expression);
            var e = Json.Expression.Null();

            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(n));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(n, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, n));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(n, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, n, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, n));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(n, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, n, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, n, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, e, n));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(n, e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, n, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, n, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, e, n, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, e, e, n));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(n, e, e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, n, e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, n, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, e, n, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, e, e, n, e));
            Assert.ThrowsException<ArgumentNullException>(() => Json.Expression.Array(e, e, e, e, e, n));
        }

        [TestMethod]
        public void Expression_Constant_Null()
        {
            var t = Json.Expression.Null();
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Null);

            Assert.AreSame(Json.Expression.Null(), Json.Expression.Null());
        }

        [TestMethod]
        public void Expression_Constant_Bool_True()
        {
            var t = Json.Expression.Boolean(true);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)o).Value);

            Assert.AreSame(Json.Expression.Boolean(true), Json.Expression.Boolean(true));
        }

        [TestMethod]
        public void Expression_Constant_Bool_False()
        {
            var t = Json.Expression.Boolean(false);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Boolean && !(bool)((Json.ConstantExpression)o).Value);

            Assert.AreSame(Json.Expression.Boolean(false), Json.Expression.Boolean(false));
        }

        [TestMethod]
        public void Expression_Constant_Number_Null()
        {
            var t = Json.Expression.Number(value: null);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Null);
        }

        [TestMethod]
        public void Expression_Constant_Number_Int()
        {
            var t = Json.Expression.Number("42");
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)o).Value == "42");
        }

        [TestMethod]
        public void Expression_Constant_Number_Int_Range()
        {
            foreach (var x in Enumerable.Range(-10, 100))
            {
                var t = Json.Expression.Number(x.ToString());
                var j = t.ToString();
                var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
                Assert.IsTrue(o.NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)o).Value == x.ToString());
            }
        }

        [TestMethod]
        public void Expression_Constant_Number_Int_Optimized()
        {
            for (var i = 0; i <= 9; i++)
            {
                Assert.AreSame(Json.Expression.Number(i.ToString()), Json.Expression.Number(i.ToString()));
            }
        }

        [TestMethod]
        public void Expression_Constant_String()
        {
            var s = "Hello, \t \"World\"! \r\n \b \\ \f";

            var t = Json.Expression.String(s);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)o).Value == s);
        }

        [TestMethod]
        public void Expression_Constant_String_Null()
        {
            var s = default(string);

            var t = Json.Expression.String(s);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Null);
        }

        [TestMethod]
        public void Expression_Constant_String_Empty()
        {
            var s = "";

            var t = Json.Expression.String(s);
            var j = t.ToString();
            var o = Json.Expression.Parse(j, ensureTopLevelObjectOrArray: false);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)o).Value == s);

            Assert.AreSame(Json.Expression.String(""), Json.Expression.String(" ".Trim(' ')));
        }

        [TestMethod]
        public void Expression_Array_Empty()
        {
            var t = Json.Expression.Array();
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);

            foreach (var arr in new[] { t, (Json.ArrayExpression)o })
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(0));

                Assert.IsTrue(arr.ElementCount == 0);
                Assert.IsTrue(arr.Elements.Count == 0);
            }
        }

        [TestMethod]
        public void Expression_Array_One()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"));
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);

            foreach (var arr in new[] { t, (Json.ArrayExpression)o })
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(1));

                for (var k = 0; k < 2; k++) // Twice to check before/after Elements allocation.
                {
                    Assert.IsTrue(arr.ElementCount == 1);
                    var elements = Enumerable.Range(0, 1).Select(arr.GetElement).ToArray();
                    Assert.IsTrue(elements.SequenceEqual(arr.Elements /* will allocate */));
                }

                Assert.IsTrue(arr.Elements.Count == 1);
                Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");

                var i = arr.ToString();
                Assert.AreEqual(j, i);
            }
        }

        [TestMethod]
        public void Expression_Array_Two()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"), Json.Expression.Boolean(true));
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);

            foreach (var arr in new[] { t, (Json.ArrayExpression)o })
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(2));

                for (var k = 0; k < 2; k++) // Twice to check before/after Elements allocation.
                {
                    Assert.IsTrue(arr.ElementCount == 2);
                    var elements = Enumerable.Range(0, 2).Select(arr.GetElement).ToArray();
                    Assert.IsTrue(elements.SequenceEqual(arr.Elements /* will allocate */));
                }

                Assert.IsTrue(arr.Elements.Count == 2);
                Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");
                Assert.IsTrue(arr.Elements[1].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[1]).Value == true);

                var i = arr.ToString();
                Assert.AreEqual(j, i);
            }
        }

        [TestMethod]
        public void Expression_Array_Three()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"), Json.Expression.Boolean(true), Json.Expression.String("Hello"));
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);

            foreach (var arr in new[] { t, (Json.ArrayExpression)o })
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(3));

                for (var k = 0; k < 2; k++) // Twice to check before/after Elements allocation.
                {
                    Assert.IsTrue(arr.ElementCount == 3);
                    var elements = Enumerable.Range(0, 3).Select(arr.GetElement).ToArray();
                    Assert.IsTrue(elements.SequenceEqual(arr.Elements /* will allocate */));
                }

                Assert.IsTrue(arr.Elements.Count == 3);
                Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");
                Assert.IsTrue(arr.Elements[1].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[1]).Value == true);
                Assert.IsTrue(arr.Elements[2].NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)arr.Elements[2]).Value == "Hello");

                var i = arr.ToString();
                Assert.AreEqual(j, i);
            }
        }

        [TestMethod]
        public void Expression_Array_Four()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"), Json.Expression.Boolean(true), Json.Expression.String("Hello"), Json.Expression.Null());
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);

            foreach (var arr in new[] { t, (Json.ArrayExpression)o })
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(4));

                for (var k = 0; k < 2; k++) // Twice to check before/after Elements allocation.
                {
                    Assert.IsTrue(arr.ElementCount == 4);
                    var elements = Enumerable.Range(0, 4).Select(arr.GetElement).ToArray();
                    Assert.IsTrue(elements.SequenceEqual(arr.Elements /* will allocate */));
                }

                Assert.IsTrue(arr.Elements.Count == 4);
                Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");
                Assert.IsTrue(arr.Elements[1].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[1]).Value == true);
                Assert.IsTrue(arr.Elements[2].NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)arr.Elements[2]).Value == "Hello");
                Assert.IsTrue(arr.Elements[3].NodeType == Json.ExpressionType.Null);

                var i = arr.ToString();
                Assert.AreEqual(j, i);
            }
        }

        [TestMethod]
        public void Expression_Array_Five()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"), Json.Expression.Boolean(true), Json.Expression.String("Hello"), Json.Expression.Null(), Json.Expression.Number("2"));
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);

            foreach (var arr in new[] { t, (Json.ArrayExpression)o })
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(5));

                for (var k = 0; k < 2; k++) // Twice to check before/after Elements allocation.
                {
                    Assert.IsTrue(arr.ElementCount == 5);
                    var elements = Enumerable.Range(0, 5).Select(arr.GetElement).ToArray();
                    Assert.IsTrue(elements.SequenceEqual(arr.Elements /* will allocate */));
                }

                Assert.IsTrue(arr.Elements.Count == 5);
                Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");
                Assert.IsTrue(arr.Elements[1].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[1]).Value == true);
                Assert.IsTrue(arr.Elements[2].NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)arr.Elements[2]).Value == "Hello");
                Assert.IsTrue(arr.Elements[3].NodeType == Json.ExpressionType.Null);
                Assert.IsTrue(arr.Elements[4].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[4]).Value == "2");

                var i = arr.ToString();
                Assert.AreEqual(j, i);
            }
        }

        [TestMethod]
        public void Expression_Array_Six()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"), Json.Expression.Boolean(true), Json.Expression.String("Hello"), Json.Expression.Null(), Json.Expression.Number("2"), Json.Expression.Boolean(false));
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);

            foreach (var arr in new[] { t, (Json.ArrayExpression)o })
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(6));

                for (var k = 0; k < 2; k++) // Twice to check before/after Elements allocation.
                {
                    Assert.IsTrue(arr.ElementCount == 6);
                    var elements = Enumerable.Range(0, 6).Select(arr.GetElement).ToArray();
                    Assert.IsTrue(elements.SequenceEqual(arr.Elements /* will allocate */));
                }

                Assert.IsTrue(arr.Elements.Count == 6);
                Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");
                Assert.IsTrue(arr.Elements[1].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[1]).Value == true);
                Assert.IsTrue(arr.Elements[2].NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)arr.Elements[2]).Value == "Hello");
                Assert.IsTrue(arr.Elements[3].NodeType == Json.ExpressionType.Null);
                Assert.IsTrue(arr.Elements[4].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[4]).Value == "2");
                Assert.IsTrue(arr.Elements[5].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[5]).Value == false);

                var i = arr.ToString();
                Assert.AreEqual(j, i);
            }
        }

        [TestMethod]
        public void Expression_Array_More()
        {
            var t = Json.Expression.Array(Json.Expression.Number("1"), Json.Expression.Boolean(true), Json.Expression.String("Hello"), Json.Expression.Null(), Json.Expression.Number("2"), Json.Expression.Boolean(false), Json.Expression.Null());
            var j = t.ToString();
            var o = Json.Expression.Parse(j);
            Assert.IsTrue(o.NodeType == Json.ExpressionType.Array);

            foreach (var arr in new[] { t, (Json.ArrayExpression)o })
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(-1));
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => arr.GetElement(7));

                for (var k = 0; k < 2; k++) // Twice for consistency with other tests above.
                {
                    Assert.IsTrue(arr.ElementCount == 7);
                    var elements = Enumerable.Range(0, 7).Select(arr.GetElement).ToArray();
                    Assert.IsTrue(elements.SequenceEqual(arr.Elements));
                }

                Assert.IsTrue(arr.Elements.Count == 7);
                Assert.IsTrue(arr.Elements[0].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[0]).Value == "1");
                Assert.IsTrue(arr.Elements[1].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[1]).Value == true);
                Assert.IsTrue(arr.Elements[2].NodeType == Json.ExpressionType.String && (string)((Json.ConstantExpression)arr.Elements[2]).Value == "Hello");
                Assert.IsTrue(arr.Elements[3].NodeType == Json.ExpressionType.Null);
                Assert.IsTrue(arr.Elements[4].NodeType == Json.ExpressionType.Number && (string)((Json.ConstantExpression)arr.Elements[4]).Value == "2");
                Assert.IsTrue(arr.Elements[5].NodeType == Json.ExpressionType.Boolean && (bool)((Json.ConstantExpression)arr.Elements[5]).Value == false);
                Assert.IsTrue(arr.Elements[6].NodeType == Json.ExpressionType.Null);

                var i = arr.ToString();
                Assert.AreEqual(j, i);
            }
        }

        [TestMethod]
        public void Expression_Array_Nested()
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
        public void Expression_Array_Optimize0()
        {
            var arr0 = Json.Expression.Array();
            var arr1 = Json.Expression.Array(Array.Empty<Json.Expression>());
            var arr2 = Json.Expression.Array(new List<Json.Expression>());
            var arr3 = Json.Expression.Array(Enumerable.Empty<Json.Expression>());

            Assert.IsTrue(arr0.ElementCount == 0);
            Assert.IsTrue(arr0.Elements.Count == 0);

            Assert.AreSame(arr0, arr1);
            Assert.AreSame(arr0, arr2);
            Assert.AreSame(arr0, arr3);
        }

        [TestMethod]
        public void Expression_Array_Optimize1() => Expression_Array_Optimize(1);

        [TestMethod]
        public void Expression_Array_Optimize2() => Expression_Array_Optimize(2);

        [TestMethod]
        public void Expression_Array_Optimize3() => Expression_Array_Optimize(3);

        [TestMethod]
        public void Expression_Array_Optimize4() => Expression_Array_Optimize(4);

        [TestMethod]
        public void Expression_Array_Optimize5() => Expression_Array_Optimize(5);

        [TestMethod]
        public void Expression_Array_Optimize6() => Expression_Array_Optimize(6);

        private static void Expression_Array_Optimize(int n)
        {
            var exprs = Enumerable.Range(0, n).Select(i => Json.Expression.Number(n.ToString())).Cast<Json.Expression>().ToArray();

            var arr1 = Json.Expression.Array(exprs.Select(e => e));
            var arr2 = Json.Expression.Array(exprs.ToArray());
            var arr3 = Json.Expression.Array(exprs.ToList());

            foreach (var arr in new[] { arr1, arr2, arr3 })
            {
                Assert.IsTrue(arr.ElementCount == n);
                Assert.IsTrue(exprs.SequenceEqual(Enumerable.Range(0, n).Select(i => arr.GetElement(i))));

                Assert.IsTrue(arr.Elements.Count == n);
                Assert.IsTrue(exprs.SequenceEqual(arr.Elements));

                Assert.IsTrue(arr.GetType().Name.Contains("ArrayExpression" + n));
            }
        }

        [TestMethod]
        public void Expression_Object()
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

        [TestMethod]
        public void Expression_ToString()
        {
            var t = Json.Expression.Object(new Dictionary<string, Json.Expression>
            {
                { "Name", Json.Expression.String("Bart") },
                { "Age", Json.Expression.Number("21") },
            });

            Assert.ThrowsException<ArgumentNullException>(() => t.ToString(builder: null));

            var s1 = t.ToString();

            var sb = new StringBuilder();
            t.ToString(sb);
            var s2 = sb.ToString();

            Assert.AreEqual(s1, s2);
        }
    }
}
