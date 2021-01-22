// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Linq.Expressions.Jit;
using System.Runtime.CompilerServices;

namespace Tests
{
    [TestClass]
    public class ClosureGeneratorTests
    {
        [TestMethod]
        public void ClosureGeneratorAll()
        {
            var eq = new ExpressionEqualityComparer();

            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(string));
            var z = Expression.Parameter(typeof(bool));

            var variables = new[]
            {
                new KeyValuePair<ParameterExpression, StorageKind>(x, StorageKind.Hoisted),
                new KeyValuePair<ParameterExpression, StorageKind>(y, StorageKind.Hoisted | StorageKind.Boxed),
                new KeyValuePair<ParameterExpression, StorageKind>(z, StorageKind.Hoisted),
            };

            var res = ClosureGenerator.Create(variables);

            var closureType = res.ClosureType;

            var fx = closureType.GetField("Item1");
            Assert.IsNotNull(fx);
            Assert.AreEqual(typeof(int), fx.FieldType);
            Assert.AreEqual(fx, res.FieldMap[x].Field);
            Assert.AreEqual(StorageKind.Hoisted, res.FieldMap[x].Kind);

            var fy = closureType.GetField("Item2");
            Assert.IsNotNull(fy);
            Assert.AreEqual(typeof(StrongBox<string>), fy.FieldType);
            Assert.AreEqual(fy, res.FieldMap[y].Field);
            Assert.AreEqual(StorageKind.Hoisted | StorageKind.Boxed, res.FieldMap[y].Kind);

            var fz = closureType.GetField("Item3");
            Assert.IsNotNull(fz);
            Assert.AreEqual(typeof(bool), fz.FieldType);
            Assert.AreEqual(fz, res.FieldMap[z].Field);
            Assert.AreEqual(StorageKind.Hoisted, res.FieldMap[z].Kind);

            var closure = Expression.Parameter(closureType);

            var ax = res.Access(closure, x);
            Assert.IsTrue(
                eq.Equals(ax,
                    Expression.Field(
                        closure,
                        fx
                    )
                )
            );

            var sx = res.Assign(closure, x, Expression.Constant(1));
            Assert.IsTrue(
                eq.Equals(sx,
                    Expression.Assign(
                        Expression.Field(
                            closure,
                            fx
                        ),
                        Expression.Constant(1)
                    )
                )
            );

            var ay = res.Access(closure, y);
            Assert.IsTrue(
                eq.Equals(ay,
                    Expression.Field(
                        Expression.Field(
                            closure,
                            fy
                        ),
                        typeof(StrongBox<string>).GetField("Value")
                    )
                )
            );

            var sy = res.Assign(closure, y, Expression.Constant("bar"));
            Assert.IsTrue(
                eq.Equals(sy,
                    Expression.Assign(
                        Expression.Field(
                            closure,
                            fy
                        ),
                        Expression.New(
                            typeof(StrongBox<string>).GetConstructor(new[] { typeof(string) }),
                            Expression.Constant("bar")
                        )
                    )
                )
            );

            var az = res.Access(closure, z);
            Assert.IsTrue(
                eq.Equals(az,
                    Expression.Field(
                        closure,
                        fz
                    )
                )
            );

            var sz = res.Assign(closure, z, Expression.Constant(true));
            Assert.IsTrue(
                eq.Equals(sz,
                    Expression.Assign(
                        Expression.Field(
                            closure,
                            fz
                        ),
                        Expression.Constant(true)
                    )
                )
            );
        }
    }
}
