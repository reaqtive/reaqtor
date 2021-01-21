// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System;
using System.Linq.CompilerServices;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public class StrictBetaReducerTests
    {
        private static readonly Expression B = Expression.Call(typeof(StrictBetaReducerTests).GetMethod(nameof(MB)));
        private static readonly Expression D = Expression.Call(typeof(StrictBetaReducerTests).GetMethod(nameof(MD)));
        private static readonly Expression F = Expression.Call(typeof(StrictBetaReducerTests).GetMethod(nameof(MF)));
        private static readonly Expression G = Expression.Call(typeof(StrictBetaReducerTests).GetMethod(nameof(MG)));
        private static readonly Expression S = Expression.Call(typeof(StrictBetaReducerTests).GetMethod(nameof(MS)));
        private static readonly Expression T = Expression.Call(typeof(StrictBetaReducerTests).GetMethod(nameof(MT)));

        [TestMethod]
        public void StrictBetaReducer_NoArgs1()
        {
            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Empty()
                    )
                );

            var r =
                Expression.Empty();

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_NoArgs2()
        {
            var b =
                (Expression<Action>)(() => Console.WriteLine("Foo"));

            var e =
                Expression.Invoke(
                    b
                );

            var r =
                b.Body;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Identity_Default()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        x,
                        x
                    ),
                    Expression.Default(typeof(int))
                );

            var r =
                Expression.Default(typeof(int));

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Identity_Constant()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        x,
                        x
                    ),
                    Expression.Constant(42)
                );

            var r =
                Expression.Constant(42);

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Identity_GlobalParameter()
        {
            var x = Expression.Parameter(typeof(int));
            var g = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        x,
                        x
                    ),
                    g
                );

            var r =
                g;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Identity_SideEffect()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        x,
                        x
                    ),
                    F
                );

            var r =
                F;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Identity_CanReduceWhenDiscardingPure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Constant(1),
                        x
                    ),
                    Expression.Default(typeof(int))
                );

            var r =
                Expression.Constant(1);

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Identity_CanReduceWhenRepeatingPure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(x, x),
                        x
                    ),
                    PureRepeat(typeof(int))
                );

            var r =
                Expression.Add(
                    PureRepeat(typeof(int)),
                    PureRepeat(typeof(int))
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Identity_DontReduceIfSideEffectDiscarded()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Constant(1),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Identity_DontReduceIfSideEffectRepeated()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(x, x),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_DontBindNestedScope_Lambda()
        {
            var x = Expression.Parameter(typeof(int));

            var b =
                Expression.Lambda(
                    x,
                    x
                );

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        b,
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                b;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_DontBindNestedScope_Block()
        {
            var x = Expression.Parameter(typeof(int));

            var b =
                Expression.Block(
                    new[] { x },
                    x
                );

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        b,
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                b;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_BindPureInNestedLambda()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Lambda(
                            x,
                            y
                        ),
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                Expression.Lambda(
                    Expression.Constant(1),
                    y
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_BindIgnoresGlobals()
        {
            var x = Expression.Parameter(typeof(int));
            var g = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(
                            x,
                            g
                        ),
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                Expression.Add(
                    Expression.Constant(1),
                    g
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoBindIfCapture_Lambda()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Lambda(
                            x,
                            y
                        ),
                        x
                    ),
                    y
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoBindLval_Assign_Parameter()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Assign(
                            x,
                            Expression.Constant(1)
                        ),
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoBindLval_Assign_Member()
        {
            var x = Expression.Parameter(typeof(StrongBox<int>));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Assign(
                            Expression.Field(
                                x,
                                nameof(StrongBox<int>.Value)
                            ),
                            Expression.Constant(1)
                        ),
                        x
                    ),
                    Expression.Default(typeof(StrongBox<int>))
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoBindLval_Assign_Index()
        {
            var x = Expression.Parameter(typeof(List<int>));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Assign(
                            Expression.MakeIndex(
                                x,
                                typeof(List<int>).GetProperty("Item"),
                                new[]
                                {
                                    Expression.Constant(0)
                                }
                            ),
                            Expression.Constant(1)
                        ),
                        x
                    ),
                    Expression.Default(typeof(List<int>))
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoBindLval_AddAssign()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.AddAssign(
                            x,
                            Expression.Constant(1)
                        ),
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoBindLval_PostIncrementAssign()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.PostIncrementAssign(
                            x
                        ),
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoBindIfCapture_Block()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Block(
                            new[] { y },
                            x
                        ),
                        x
                    ),
                    y
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoBindInQuote()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Quote(
                            Expression.Lambda(
                                x
                            )
                        ),
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoInlineLval()
        {
            var g = Expression.Parameter(typeof(int));
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(
                            g,
                            x
                        ),
                        x
                    ),
                    Expression.Assign(
                        g,
                        Expression.Constant(1)
                    )
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_NoInlineRethrow()
        {
            var g = Expression.Parameter(typeof(int));
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(
                            g,
                            x
                        ),
                        x
                    ),
                    Expression.Rethrow(typeof(int))
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_SafeToInlineRethrow()
        {
            var g = Expression.Parameter(typeof(int));
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(
                            g,
                            x
                        ),
                        x
                    ),
                    Expression.TryCatch(
                        PureNoThrowNoNull(typeof(int)),
                        Expression.Catch(
                            typeof(Exception),
                            Expression.Rethrow(typeof(int))
                        )
                    )
                );

            var r =
                Expression.Add(
                    g,
                    Expression.TryCatch(
                        PureNoThrowNoNull(typeof(int)),
                        Expression.Catch(
                            typeof(Exception),
                            Expression.Rethrow(typeof(int))
                        )
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Unary_CanReduce()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.OnesComplement(x),
                        x
                    ),
                    F
                );

            var r =
                Expression.OnesComplement(F);

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_UnaryMayThrow_Negate_Method()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.Negate(
                    PureNoThrowNoNull(typeof(TimeSpan))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_UnaryMayThrow_ArrayLength()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.ArrayLength(
                    PureNoThrowMaybeNull(typeof(int[]))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_UnaryMayThrow_Convert()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.Convert(
                    PureNoThrowNoNull(typeof(long)),
                    typeof(int)
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_UnaryMayThrow_ConvertChecked()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.ConvertChecked(
                    PureNoThrowNoNull(typeof(long)),
                    typeof(int)
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_UnaryMayThrow_NegateChecked()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.NegateChecked(
                    PureNoThrowNoNull(typeof(int))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_UnaryMayThrow_Throw()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.Throw(
                    PureNoThrowNoNull(typeof(Exception)),
                    typeof(int)
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_UnaryMayThrow_Unbox()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.Unbox(
                    PureNoThrowMaybeNull(typeof(object)),
                    typeof(int)
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_TypeBinary_CanReduce()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.TypeIs(x, typeof(int)),
                        x
                    ),
                    F
                );

            var r =
                Expression.TypeIs(F, typeof(int));

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_CanReduce_AddLeft()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(x, G),
                        x
                    ),
                    F
                );

            var r =
                Expression.Add(F, G);

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_CanReduce_AddRightPure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(
                            Expression.Default(typeof(int)),
                            x
                        ),
                        x
                    ),
                    F
                );

            var r =
                Expression.Add(
                    Expression.Default(typeof(int)),
                    F
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_CantReduce_SideEffectBefore()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(G, x),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_CantReduce_MayShortCircuit_AndAlso()
        {
            var x = Expression.Parameter(typeof(bool));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.AndAlso(
                            PureNoThrowNoNull(typeof(bool)),
                            x
                        ),
                        x
                    ),
                    B
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_CantReduce_MayShortCircuit_OrElse()
        {
            var x = Expression.Parameter(typeof(bool));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.OrElse(
                            PureNoThrowNoNull(typeof(bool)),
                            x
                        ),
                        x
                    ),
                    B
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_CantReduce_MayShortCircuit_Coalesce()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Coalesce(
                            PureNoThrowNoNull(typeof(int?)),
                            x
                        ),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_Coalesce_WithConversion_CantReduce_NonDeterminism()
        {
            var d = Expression.Parameter(typeof(DateTime));
            var x = Expression.Parameter(typeof(DateTimeOffset));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Coalesce(
                            Expression.Convert(
                                Expression.Property(
                                    null,
                                    typeof(DateTime).GetProperty(nameof(DateTime.Now))
                                ),
                                typeof(DateTime?)
                            ),
                            x,
                            Expression.Lambda(
                                Expression.Convert(d, typeof(DateTimeOffset)),
                                d
                            )
                        ),
                        x
                    ),
                    D
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_Coalesce_WithConversion_CantReduce_LeftNeverNull_NoRightSideEffect()
        {
            var n = DateTime.Now;
            var d = Expression.Parameter(typeof(DateTime));
            var x = Expression.Parameter(typeof(DateTimeOffset));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Coalesce(
                            Expression.Constant(
                                n,
                                typeof(DateTime?)
                            ),
                            x,
                            Expression.Lambda(
                                Expression.Convert(d, typeof(DateTimeOffset)),
                                d
                            )
                        ),
                        x
                    ),
                    D
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_Coalesce_WithConversion_CanReduce_LeftNeverNull_ConversionSideEffect()
        {
            var n = DateTime.Now;
            var d = Expression.Parameter(typeof(DateTime));
            var x = Expression.Parameter(typeof(DateTimeOffset));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Coalesce(
                            Expression.Constant(
                                n,
                                typeof(DateTime?)
                            ),
                            PureNoThrowNoNull(typeof(DateTimeOffset)),
                            Expression.Lambda(
                                x,
                                d
                            )
                        ),
                        x
                    ),
                    D
                );

            var r =
                Expression.Coalesce(
                    Expression.Constant(
                        n,
                        typeof(DateTime?)
                    ),
                    PureNoThrowNoNull(typeof(DateTimeOffset)),
                    Expression.Lambda(
                        D,
                        d
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_Coalesce_WithConversion_CanReduce_LeftMaybeNull_AllSideEffectsEnsured()
        {
            var g = Expression.Parameter(typeof(DateTime?));
            var d = Expression.Parameter(typeof(DateTime));
            var x = Expression.Parameter(typeof(DateTimeOffset));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Block(
                            x,
                            Expression.Coalesce(
                                g,
                                PureNoThrowNoNull(typeof(DateTimeOffset)),
                                Expression.Lambda(
                                    Expression.Convert(
                                        d,
                                        typeof(DateTimeOffset)
                                    ),
                                    d
                                )
                            )
                        ),
                        x
                    ),
                    D
                );

            var r =
                Expression.Block(
                    D,
                    Expression.Coalesce(
                        g,
                        PureNoThrowNoNull(typeof(DateTimeOffset)),
                        Expression.Lambda(
                            Expression.Convert(
                                d,
                                typeof(DateTimeOffset)
                            ),
                            d
                        )
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Binary_AddAssign_WithConversion_CanReduce()
        {
            var c = Expression.Parameter(typeof(TimeSpan));
            var v = Expression.Parameter(typeof(TimeSpan));
            var x = Expression.Parameter(typeof(TimeSpan));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.AddAssign(
                            v,
                            x,
                            typeof(TimeSpan).GetMethod("op_Addition"),
                            Expression.Lambda(
                                c,
                                c
                            )
                        ),
                        x
                    ),
                    T
                );

            var r =
                Expression.AddAssign(
                    v,
                    T,
                    typeof(TimeSpan).GetMethod("op_Addition"),
                    Expression.Lambda(
                        c,
                        c
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_BinaryMayThrow_AddMethod()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(TimeSpan),
                Expression.Add(
                    PureNoThrowNoNull(typeof(TimeSpan)),
                    PureNoThrowNoNull(typeof(TimeSpan))
                ),
                T
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_BinaryMayThrow_AddChecked()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.AddChecked(
                    PureNoThrowNoNull(typeof(int)),
                    PureNoThrowNoNull(typeof(int))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_BinaryMayThrow_ArrayIndex()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.ArrayIndex(
                    PureNoThrowNoNull(typeof(int[])),
                    PureNoThrowNoNull(typeof(int))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_BinaryMayThrow_Divide()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.Divide(
                    PureNoThrowNoNull(typeof(int)),
                    PureNoThrowNoNull(typeof(int))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_BinaryMayThrow_Modulo()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.Modulo(
                    PureNoThrowNoNull(typeof(int)),
                    PureNoThrowNoNull(typeof(int))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_TwoParams_Binary_CanReduce_Add()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(x, y),
                        x,
                        y
                    ),
                    F,
                    G
                );

            var r =
                Expression.Add(
                    F,
                    G
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_TwoParams_Binary_CantReduce_ReordersOperands()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Add(y, x),
                        x,
                        y
                    ),
                    F,
                    G
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Conditional_CanReduceTest()
        {
            var x = Expression.Parameter(typeof(bool));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Condition(
                            x,
                            Expression.Constant(2),
                            Expression.Constant(3)
                        ),
                        x
                    ),
                    B
                );

            var r =
                Expression.Condition(
                    B,
                    Expression.Constant(2),
                    Expression.Constant(3)
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Conditional_CantReduce_NotDeterministic_IfTrue()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Condition(
                            PureNoThrowNoNull(typeof(bool)),
                            x,
                            Expression.Constant(3)
                        ),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Conditional_CantReduce_NotDeterministic_IfFalse()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Condition(
                            PureNoThrowNoNull(typeof(bool)),
                            Expression.Constant(2),
                            x
                        ),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Index_CanReduce_ArrayAccess_Index()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.ArrayAccess(
                            PureNoThrowNoNull(typeof(int[])),
                            x
                        ),
                        x
                    ),
                    F
                );

            var r =
                Expression.ArrayAccess(
                    PureNoThrowNoNull(typeof(int[])),
                    F
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Index_CanReduce_Index_Index()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.MakeIndex(
                            PureNoThrowNoNull(typeof(List<int>)),
                            typeof(List<int>).GetProperty("Item"),
                            new[]
                            {
                                x
                            }
                        ),
                        x
                    ),
                    F
                );

            var r =
                Expression.MakeIndex(
                    PureNoThrowNoNull(typeof(List<int>)),
                    typeof(List<int>).GetProperty("Item"),
                    new[]
                    {
                        F
                    }
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_IndexMayThrow_ArrayAccess()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.ArrayAccess(
                    PureNoThrowNoNull(typeof(int[])),
                    PureNoThrowNoNull(typeof(int))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Index_CanReduce_Goto()
        {
            var x = Expression.Parameter(typeof(int));
            var l = Expression.Label(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Block(
                            Expression.Goto(
                                l,
                                x
                            ),
                            Expression.Label(
                                l,
                                Expression.Constant(0)
                            )
                        ),
                        x
                    ),
                    F
                );

            var r =
                Expression.Block(
                    Expression.Goto(
                        l,
                        F
                    ),
                    Expression.Label(
                        l,
                        Expression.Constant(0)
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Index_CantReduce_Goto_NonDeterminism()
        {
            var x = Expression.Parameter(typeof(int));
            var l = Expression.Label(typeof(void));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Block(
                            Expression.Goto(
                                l
                            ),
                            x,
                            Expression.Label(
                                l
                            )
                        ),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_Member_CanReduce()
        {
            var x = Expression.Parameter(typeof(string));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Property(
                            x,
                            typeof(string).GetProperty(nameof(string.Length))
                        ),
                        x
                    ),
                    S
                );

            var r =
                Expression.Property(
                    S,
                    typeof(string).GetProperty(nameof(string.Length))
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_MemberMayThrow_Instance()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.Property(
                    PureNoThrowNoNull(typeof(string)),
                    typeof(string).GetProperty(nameof(string.Length))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_MemberMayThrow_Static()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.Property(
                    null,
                    typeof(DateTime).GetProperty(nameof(DateTime.Now))
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_LoopCanRepeat()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Loop(
                            x
                        ),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CanReduce_LoopCanRepeat_Pure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Loop(
                            x
                        ),
                        x
                    ),
                    PureRepeat(typeof(int))
                );

            var r =
                Expression.Loop(
                    PureRepeat(typeof(int))
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_ListInit_CanReduce_Pure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.ListInit(
                            Expression.New(
                                typeof(List<int>).GetConstructor(Type.EmptyTypes)
                            ),
                            Expression.ElementInit(
                                typeof(List<int>).GetMethod(nameof(List<int>.Add)),
                                x
                            )
                        ),
                        x
                    ),
                    PureNoThrowNoNull(typeof(int))
                );

            var r =
                Expression.ListInit(
                    Expression.New(
                        typeof(List<int>).GetConstructor(Type.EmptyTypes)
                    ),
                    Expression.ElementInit(
                        typeof(List<int>).GetMethod(nameof(List<int>.Add)),
                        PureNoThrowNoNull(typeof(int))
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_ListInit_CantReduce_NewMayThrow()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.ListInit(
                            Expression.New(
                                typeof(List<int>).GetConstructor(Type.EmptyTypes)
                            ),
                            Expression.ElementInit(
                                typeof(List<int>).GetMethod(nameof(List<int>.Add)),
                                x
                            )
                        ),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_MemberInit_CanReduce_BindPure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.MemberInit(
                            Expression.New(
                                typeof(StrongBox<int>).GetConstructor(Type.EmptyTypes)
                            ),
                            Expression.Bind(
                                typeof(StrongBox<int>).GetField(nameof(StrongBox<int>.Value)),
                                x
                            )
                        ),
                        x
                    ),
                    PureNoThrowNoNull(typeof(int))
                );

            var r =
                Expression.MemberInit(
                    Expression.New(
                        typeof(StrongBox<int>).GetConstructor(Type.EmptyTypes)
                    ),
                    Expression.Bind(
                        typeof(StrongBox<int>).GetField(nameof(StrongBox<int>.Value)),
                        PureNoThrowNoNull(typeof(int))
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_MemberInit_CanReduce_MemberBindPure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.MemberInit(
                            Expression.New(
                                typeof(Bar).GetConstructor(Type.EmptyTypes)
                            ),
                            Expression.MemberBind(
                                typeof(Bar).GetProperty(nameof(Bar.Foo)),
                                Expression.Bind(
                                    typeof(Foo).GetField(nameof(Foo.Qux)),
                                    x
                                )
                            )
                        ),
                        x
                    ),
                    PureNoThrowNoNull(typeof(int))
                );

            var r =
                Expression.MemberInit(
                    Expression.New(
                        typeof(Bar).GetConstructor(Type.EmptyTypes)
                    ),
                    Expression.MemberBind(
                        typeof(Bar).GetProperty(nameof(Bar.Foo)),
                        Expression.Bind(
                            typeof(Foo).GetField(nameof(Foo.Qux)),
                            PureNoThrowNoNull(typeof(int))
                        )
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_MemberInit_CanReduce_ListBindPure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.MemberInit(
                            Expression.New(
                                typeof(Bar).GetConstructor(Type.EmptyTypes)
                            ),
                            Expression.ListBind(
                                typeof(Bar).GetProperty(nameof(Bar.Xs)),
                                Expression.ElementInit(
                                    typeof(List<int>).GetMethod(nameof(List<int>.Add)),
                                    x
                                )
                            )
                        ),
                        x
                    ),
                    PureNoThrowNoNull(typeof(int))
                );

            var r =
                Expression.MemberInit(
                    Expression.New(
                        typeof(Bar).GetConstructor(Type.EmptyTypes)
                    ),
                    Expression.ListBind(
                        typeof(Bar).GetProperty(nameof(Bar.Xs)),
                        Expression.ElementInit(
                            typeof(List<int>).GetMethod(nameof(List<int>.Add)),
                            PureNoThrowNoNull(typeof(int))
                        )
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_MemberInit_CantReduce_NewMayThrow()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.MemberInit(
                            Expression.New(
                                typeof(StrongBox<int>).GetConstructor(Type.EmptyTypes)
                            ),
                            Expression.Bind(
                                typeof(StrongBox<int>).GetField(nameof(StrongBox<int>.Value)),
                                x
                            )
                        ),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_New_CanReduce1()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.New(
                            typeof(TimeSpan).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) }),
                            x,
                            Expression.Constant(2),
                            Expression.Constant(3)
                        ),
                        x
                    ),
                    F
                );

            var r =
                Expression.New(
                    typeof(TimeSpan).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) }),
                    F,
                    Expression.Constant(2),
                    Expression.Constant(3)
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_NewMayThrow()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.New(
                    typeof(TimeSpan).GetConstructor(new[] { typeof(long) }),
                    Expression.Constant(42L)
                ),
                F
            );
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_New_CanReduce2()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.New(
                            typeof(TimeSpan).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) }),
                            Expression.Constant(1),
                            x,
                            Expression.Constant(3)
                        ),
                        x
                    ),
                    F
                );

            var r =
                Expression.New(
                    typeof(TimeSpan).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) }),
                    Expression.Constant(1),
                    F,
                    Expression.Constant(3)
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CanReduce_NewArrayInit1()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.NewArrayInit(
                            typeof(int),
                            x,
                            Expression.Constant(2)
                        ),
                        x
                    ),
                    F
                );

            var r =
                Expression.NewArrayInit(
                    typeof(int),
                    F,
                    Expression.Constant(2)
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CanReduce_NewArrayInit2()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.NewArrayInit(
                            typeof(int),
                            Expression.Constant(1),
                            x
                        ),
                        x
                    ),
                    F
                );

            var r =
                Expression.NewArrayInit(
                    typeof(int),
                    Expression.Constant(1),
                    F
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_NewArrayMayThrow()
        {
            StrictBetaReducer_OneParam_CantReduce_MayThrow(
                typeof(int),
                Expression.NewArrayBounds(
                    typeof(int),
                    Expression.Constant(-1)
                ),
                F
            );
        }

        private static void StrictBetaReducer_OneParam_CantReduce_MayThrow(Type type, Expression mayThrow, Expression after)
        {
            var x = Expression.Parameter(type);

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.Block(
                            mayThrow,
                            x
                        ),
                        x
                    ),
                    after
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CanReduce_TryCatch_Constant()
        {
            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.TryCatch(
                            x,
                            Expression.Catch(
                                ex,
                                Expression.Constant(2)
                            )
                        ),
                        x
                    ),
                    Expression.Constant(1)
                );

            var r =
                Expression.TryCatch(
                    Expression.Constant(1),
                    Expression.Catch(
                        ex,
                        Expression.Constant(2)
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CanReduce_TryCatch_NoThrow()
        {
            var x = Expression.Parameter(typeof(int));
            var b = PureNoThrowNoNull(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.TryCatch(
                            x,
                            Expression.Catch(
                                ex,
                                Expression.Constant(2)
                            )
                        ),
                        x
                    ),
                    b
                );

            var r =
                Expression.TryCatch(
                    b,
                    Expression.Catch(
                        ex,
                        Expression.Constant(2)
                    )
                );

            AssertBetaReduce(e, r);
        }

        [TestMethod]
        public void StrictBetaReducer_OneParam_CantReduce_TryCatch()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Invoke(
                    Expression.Lambda(
                        Expression.TryCatch(
                            x,
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Constant(2)
                            )
                        ),
                        x
                    ),
                    F
                );

            var r =
                e;

            AssertBetaReduce(e, r);
        }

        private static void AssertBetaReduce(InvocationExpression original, Expression expected)
        {
            var reducer = new StrictBetaReducer(new SemanticProvider());

            if (!reducer.TryReduce(original, out var res))
            {
                res = original;
            }

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(res, expected), $"{res} != ${expected}");
        }

        private static readonly MethodInfo s_PureNoThrowNoNull = typeof(StrictBetaReducerTests).GetMethod(nameof(PureMethodNoThrowNeverNull));
        private static readonly MethodInfo s_PureNoThrowMaybeNull = typeof(StrictBetaReducerTests).GetMethod(nameof(PureMethodNoThrow));

        private static Expression PureNoThrowNoNull(Type type) => Expression.Call(s_PureNoThrowNoNull.MakeGenericMethod(type));
        private static Expression PureNoThrowMaybeNull(Type type) => Expression.Call(s_PureNoThrowMaybeNull.MakeGenericMethod(type));
        private static Expression PureRepeat(Type type) => Expression.Default(type);

        public static bool MB() => false;
        public static DateTimeOffset MD() => new(1975, 1, 2, 3, 45, 56, TimeSpan.Zero);
        public static int MF() => 42;
        public static int MG() => 42;
        public static string MS() => "";
        public static TimeSpan MT() => TimeSpan.Zero;
        public static int UnaryOp(int x) => x;
        public static int BinaryOp(int x, int y) => x + y;
        public static T PureMethodNoThrowNeverNull<T>() => default;
        public static T PureMethodNoThrow<T>() => default;

        private sealed class Bar
        {
            public Foo Foo { get; } = new Foo();
            public List<int> Xs { get; } = new List<int>();
        }

        private sealed class Foo
        {
#pragma warning disable 0649
            public int Qux;
#pragma warning restore
        }

        private sealed class SemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(Expression expression)
            {
                return base.IsPure(expression) || IsPureMethod(expression);
            }

            public override bool IsPure(MemberInfo member)
            {
                return base.IsPure(member) || IsPureMethod(member);
            }

            public override bool IsNeverNull(Expression expression)
            {
                return base.IsNeverNull(expression) || IsPureMethod(expression, isNeverNull: true);
            }

            public override bool NeverThrows(Expression expression)
            {
                return base.NeverThrows(expression) || IsPureMethod(expression);
            }

            public override bool NeverThrows(MemberInfo member)
            {
                return base.NeverThrows(member) || IsPureMethod(member);
            }

            private static bool IsPureMethod(Expression expression, bool isNeverNull = false)
            {
                return expression is MethodCallExpression mce && IsPureMethod(mce.Method, isNeverNull);
            }

            private static bool IsPureMethod(MemberInfo member, bool isNeverNull = false)
            {
                var method = member as MethodInfo;
                if (method != null)
                {
                    if (method.DeclaringType == typeof(StrictBetaReducerTests))
                    {
                        if (!method.Name.StartsWith("PureMethod"))
                        {
                            return false;
                        }

                        if (isNeverNull && !method.Name.Contains("NoNull"))
                        {
                            return false;
                        }

                        return true;
                    }
                }

                return false;
            }
        }
    }
}
