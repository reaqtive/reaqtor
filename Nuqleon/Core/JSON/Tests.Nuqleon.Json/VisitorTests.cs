// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Expressions;
using Nuqleon.Json.Parser;

namespace Tests.Nuqleon.Json
{
    [TestClass]
    public class VisitorTests
    {
        [TestMethod]
        public void Visitor_NoChange()
        {
            var visitor = new MyVisitor();

            foreach (var json in new[] {
                JsonParser.Parse("17", ensureTopLevelObjectOrArray: false),
                JsonParser.Parse("\"foo\"", ensureTopLevelObjectOrArray: false),
                JsonParser.Parse("[1, 3, [5, 7], 9]"),
                JsonParser.Parse("""{ "foo": 123, "bar": [-1, -3, -5] }"""),
            })
            {
                var res = visitor.Visit(json);
                Assert.AreSame(json, res);
            }
        }

        [TestMethod]
        public void Visitor_SomeChange()
        {
            var visitor = new MyVisitor();

            var json = """{ "foo":   123, "bar": [  1, 2, 3 ],    "qux": 8}""";

            var res = visitor.Visit(JsonParser.Parse(json));

            Assert.AreEqual(@"{""foo"":123,""bar"":[1,1,3],""qux"":4}", res.ToString());
        }

        private sealed class MyVisitor : ExpressionVisitor
        {
            public override Expression VisitConstant(ConstantExpression node)
            {
                if (node.NodeType == ExpressionType.Number)
                {
                    var value = int.Parse((string)node.Value);

                    if (value % 2 == 0)
                    {
                        return Expression.Number((value / 2).ToString());
                    }
                }

                return base.VisitConstant(node);
            }
        }
    }
}
