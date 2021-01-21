// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public class VariableDefaultValueSubstitutorTests
    {
        [TestMethod]
        public void VariableDefaultValueSubstitutor_Assign()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Block(
                    Expression.TryCatch(
                        Expression.Block(
                            typeof(void),
                            Expression.Lambda(
                                x
                            )
                        ),
                        Expression.Catch(
                            ex,
                            Expression.Block(
                                typeof(void),
                                ex
                            )
                        )
                    ),
                    Expression.Block(
                        new[] { x },
                        x
                    ),
                    Expression.Lambda(
                        Expression.Add(
                            x,
                            y
                        ),
                        x
                    )
                );

            var subst = new VariableDefaultValueSubstitutor(new HashSet<ParameterExpression> { x, ex });

            var res = subst.Visit(e);

            var r =
                Expression.Block(
                    Expression.TryCatch(
                        Expression.Block(
                            typeof(void),
                            Expression.Lambda(
                                Expression.Default(typeof(int))
                            )
                        ),
                        Expression.Catch(
                            ex,
                            Expression.Block(
                                typeof(void),
                                ex
                            )
                        )
                    ),
                    Expression.Block(
                        new[] { x },
                        x
                    ),
                    Expression.Lambda(
                        Expression.Add(
                            x,
                            y
                        ),
                        x
                    )
                );

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(r, res));
        }
    }
}
