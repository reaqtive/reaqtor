// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ThunkFactoryTests
    {
        [TestMethod]
        public void GetThunkType_Constraints()
        {
            var t = ThunkFactory.Compiled.GetThunkType(typeof(Foo<int, object, BarInt32>), typeof(int));
            Assert.IsTrue(t.IsGenericType);

            var g = t.GetGenericTypeDefinition();
            var a = g.GetGenericArguments();
            Assert.AreEqual(4, a.Length);

            //
            // REVIEW: Something's not quite right on Mono. The GenericParameterAttributes below run on the builder type,
            //         rather than the result of creating the type (i.e. through CreateType) and throw NotSupportedExeption.
            //         Similar issues occur for accesses to GetGenericParameterConstraints, throwing InvalidOperationException.
            //
            if (Type.GetType("Mono.Runtime") != null)
            {
                return;
            }

            Assert.AreEqual(GenericParameterAttributes.NotNullableValueTypeConstraint | GenericParameterAttributes.DefaultConstructorConstraint, a[1].GenericParameterAttributes);
            var c1 = a[1].GetGenericParameterConstraints();
            Assert.AreEqual(1, c1.Length);
            Assert.AreEqual(typeof(ValueType), c1[0]);

            Assert.AreEqual(GenericParameterAttributes.DefaultConstructorConstraint, a[2].GenericParameterAttributes);
            var c2 = a[2].GetGenericParameterConstraints();
            Assert.AreEqual(0, c2.Length);

            Assert.AreEqual(GenericParameterAttributes.ReferenceTypeConstraint, a[3].GenericParameterAttributes);
            var c3 = a[3].GetGenericParameterConstraints();
            Assert.AreEqual(1, c3.Length);
            Assert.AreEqual(typeof(IBar<>).MakeGenericType(a[1]), c3[0]);
        }

        [TestMethod]
        public void GetThunkType_Custom()
        {
            AssertThunkTypes(typeof(BinOp), Data.Values);
        }

        [TestMethod]
        public void GetThunkType_BuiltIn_NonGeneric()
        {
            AssertThunkTypes(typeof(Action), Data.Values);
        }

        [TestMethod]
        public void GetThunkType_BuiltIn_Generic()
        {
            var funcs = new[]
            {
                typeof(Func<>),
                typeof(Func<,>),
                typeof(Func<,,>),
                typeof(Func<,,,>),
                typeof(Func<,,,,>),
                typeof(Func<,,,,,>),
                typeof(Func<,,,,,,>),
                typeof(Func<,,,,,,,>),
                typeof(Func<,,,,,,,,>),
                typeof(Func<,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,,,,>),
                typeof(Func<,,,,,,,,,,,,,,,,>),
                typeof(Action<>),
                typeof(Action<,>),
                typeof(Action<,,>),
                typeof(Action<,,,>),
                typeof(Action<,,,,>),
                typeof(Action<,,,,,>),
                typeof(Action<,,,,,,>),
                typeof(Action<,,,,,,,>),
                typeof(Action<,,,,,,,,>),
                typeof(Action<,,,,,,,,,>),
                typeof(Action<,,,,,,,,,,>),
                typeof(Action<,,,,,,,,,,,>),
                typeof(Action<,,,,,,,,,,,,>),
                typeof(Action<,,,,,,,,,,,,,>),
                typeof(Action<,,,,,,,,,,,,,,>),
                typeof(Action<,,,,,,,,,,,,,,,>),
            };

            foreach (var func in funcs)
            {
                var arity = func.GetGenericArguments().Length;
                var args = Data.Types.Take(arity).ToArray();
                var funcType = func.MakeGenericType(args);
                AssertThunkTypes(funcType, Data.Values);
            }
        }

        private static void AssertThunkTypes(Type delegateType, Dictionary<Type, object[]> values)
        {
            AssertThunkType(ThunkFactory.Compiled, delegateType, values);
            AssertThunkType(ThunkFactory.Interpreted, delegateType, values);

            if (Type.GetType("Mono.Runtime") != null)
            {
                // REVIEW: Tiered compilation has issues on Mono.
                return;
            }

            AssertThunkType(ThunkFactory.TieredCompilation, delegateType, values);
        }

        private static void AssertThunkType(IThunkFactory factory, Type delegateType, Dictionary<Type, object[]> values)
        {
            //
            // Get the delegate's Invoke method and infer parameter and return types.
            //
            var invokeMethod = delegateType.GetMethod("Invoke");

            var retType = invokeMethod.ReturnType;
            var ret = retType != typeof(void) ? values[retType][0] : null;

            var parameterTypes = invokeMethod.GetParameters().Select(p => p.ParameterType).ToArray();
            var args = parameterTypes.Select(p => values[p][0]).ToArray();

            //
            // Use a special closure type to assert behavior.
            //
            var closureType = typeof(AssertiveClosure);
            var closureAdd = closureType.GetMethod("Add");
            var closureFlag = closureType.GetMethod("Flag");

            //
            // This is the functionality we're testing.
            //
            var t = factory.GetThunkType(delegateType, closureType);

            //
            // Assert we got only one constructor.
            //
            var ctors = t.GetConstructors();
            Assert.AreEqual(1, ctors.Length);

            var ctor = ctors[0];
            var ctorParams = ctor.GetParameters();
            Assert.AreEqual(1, ctorParams.Length);

            //
            // Obtain the expression tree type passed to the constructor.
            //
            var exprType = ctorParams[0].ParameterType;
            Assert.IsTrue(typeof(LambdaExpression).IsAssignableFrom(exprType));
            Assert.IsTrue(exprType.IsGenericType);
            Assert.AreEqual(typeof(Expression<>), exprType.GetGenericTypeDefinition());
            var innerDelegateType = exprType.GetGenericArguments()[0];

            //
            // Build an expression tree of the required type, using the "assertive closure"
            // to record all the parameters passed in.
            //
            var closureParam = Expression.Parameter(closureType);
            var valueParams = parameterTypes.Select(p => Expression.Parameter(p)).ToArray();
            var lambdaParams = new[] { closureParam }.Concat(valueParams).ToArray();

            var exprs = new List<Expression>();

            foreach (var valueParam in valueParams)
            {
                exprs.Add(Expression.Call(closureParam, closureAdd, Expression.Convert(valueParam, typeof(object))));
            }

            if (retType != typeof(void))
            {
                exprs.Add(Expression.Constant(ret, retType));
            }
            else
            {
                exprs.Add(Expression.Constant(value: null));
            }

            var e =
                Expression.Lambda(
                    innerDelegateType,
                    Expression.Block(
                        retType,
                        exprs
                    ),
                    lambdaParams
                );

            //
            // Instantiate the thunk type.
            //
            var o = Activator.CreateInstance(t, e);
            var thunk = (Thunk)o;

            //
            // Ensure we can obtain the original expression at runtime.
            //
            Assert.AreSame(e, thunk.Lambda);

            //
            // Check the Target field and get its original value.
            //
            var target = t.GetField("Target");
            Assert.IsNotNull(target);
            var jitCompiler = (Delegate)target.GetValue(o);

            //
            // Assert the shape of the CreateDelegate method.
            //
            var createDelegate = t.GetMethod("CreateDelegate");
            Assert.IsNotNull(createDelegate);
            Assert.IsFalse(createDelegate.IsStatic);
            Assert.IsTrue(createDelegate.IsPublic);
            Assert.AreEqual(delegateType, createDelegate.ReturnType);
            var createDelegateParams = createDelegate.GetParameters();
            Assert.AreEqual(1, createDelegateParams.Length);
            Assert.AreEqual(closureType, createDelegateParams[0].ParameterType);

            //
            // Invoke CreateDelegate with an "assertive closure" instance.
            //
            var closure = new AssertiveClosure();
            var del = (Delegate)createDelegate.Invoke(o, new object[] { closure });

            //
            // Prior to invoking the delegate, check we haven't triggered JIT compilation yet.
            //
            var stillJit = (Delegate)target.GetValue(o);
            Assert.AreSame(jitCompiler, stillJit);

            //
            // Invoke the delegate and assert the result through the "assertive closure".
            //
            var res = del.DynamicInvoke(args);
            Assert.AreEqual(ret, res);
            closure.Check(flag: false, args);

            //
            // Check we triggered a JIT compilation and the Target has been replaced.
            //
            var compiled = (Delegate)target.GetValue(o);
            Assert.AreNotSame(jitCompiler, compiled);

            //
            // Invoke the delegate again to assert we don't trigger recompilation.
            //
            closure.Clear();
            res = del.DynamicInvoke(args);
            Assert.AreEqual(ret, res);
            closure.Check(flag: false, args);

            //
            // Invoke the delegate a couple more times to ensure it keeps working.
            //
            // NB: This also tests the tiered compilation logic, which is count-based at the moment.
            //
            for (var i = 0; i < 8; i++)
            {
                res = del.DynamicInvoke(args);
                Assert.AreEqual(ret, res);
            }

            //
            // Substitute the expression to assert reinstallation of the JIT.
            //
            exprs.Insert(0, Expression.Call(closureParam, closureFlag));
            var newExpression =
                Expression.Lambda(
                    innerDelegateType,
                    Expression.Block(
                        retType,
                        exprs
                    ),
                    lambdaParams
                );

            thunk.Lambda = newExpression;

            //
            // Assert the JIT gets re-installed.
            //
            // NB: Note we assert the target method and not the delegate instance. This is
            //     because the instance can differ; we don't store the compiler delegate
            //     to save space but implement a virtual Compiler property instead, whose
            //     getter returns an instance of the JIT delegate.
            //
            var reinstalledJit = (Delegate)target.GetValue(o);
            Assert.AreSame(jitCompiler.Method, reinstalledJit.Method);

            //
            // Invoke the lambda and assert that the new lambda expression is used.
            //
            closure.Clear();
            res = del.DynamicInvoke(args);
            Assert.AreEqual(ret, res);
            closure.Check(flag: true, args); // NB: Note the check for `flag` being set to `true`.

            //
            // Assert the target is substituted for the newly compiled delegate.
            //
            var recompiled = (Delegate)target.GetValue(o);
            Assert.AreNotSame(compiled, recompiled);
            Assert.AreNotSame(reinstalledJit, recompiled);
        }
    }

    internal sealed class AssertiveClosure
    {
        private readonly List<object> _args = new();
        private bool _flag;

        public void Add(object arg)
        {
            _args.Add(arg);
        }

        public void Flag()
        {
            _flag = true;
        }

        public void Clear()
        {
            _args.Clear();
            _flag = false;
        }

        public void Check(bool flag, object[] args)
        {
            Assert.AreEqual(flag, _flag);

            Assert.AreEqual(args.Length, _args.Count);

            for (var i = 0; i < args.Length; i++)
            {
                Assert.AreEqual(args[i], _args[i]);
            }
        }
    }

    public delegate int BinOp(int left, int right);

    public delegate R Foo<T1, T2, R>(T1 arg1, T2 arg2)
        where R : class, IBar<T1>
        where T1 : struct
        where T2 : new();

    public class BarInt32 : IBar<int>
    {
    }

    public interface IBar<T>
    {
    }

    public class Qux
    {
    }
}
