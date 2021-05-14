// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Core compiler logic for operation fusion.
//
// BD - October 2014
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using RuntimeLib;

namespace OperatorFusion
{
    internal static class Compiler
    {
        public static Type Compile(Type inputElementType, params IFusionOperator[] chain)
        {
            chain = Optimize(chain).ToArray();

            var dbg = DebugInfoGenerator.CreatePdbGenerator();

            var outputElementType = chain.Last().OutputType;

            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.RunAndSave);
            var mod = asm.DefineDynamicModule("Artifacts.dll", true);

            var stt = mod.DefineType("<>__Fused01_State", TypeAttributes.Class, typeof(Sink<>).MakeGenericType(outputElementType));

            /* TODO: hoist non-trivial constants to params object passed to ctor
            var foo = stt.DefineMethod("Foo", MethodAttributes.Public | MethodAttributes.Static, typeof(int), Type.EmptyTypes);
            var p = new { Age = 21 };
            Expression<Func<int>> f = Expression.Lambda<Func<int>>(Expression.Property(Expression.Constant(p), "Age"));
            f.CompileToMethod(foo);
             */

            var sttCtor = stt.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(IObserver<>).MakeGenericType(outputElementType), typeof(IDisposable) });
            var sttCtorIlg = sttCtor.GetILGenerator();
            sttCtorIlg.Emit(OpCodes.Ldarg_0);
            sttCtorIlg.Emit(OpCodes.Ldarg_1);
            sttCtorIlg.Emit(OpCodes.Ldarg_2);
            sttCtorIlg.Emit(OpCodes.Call, typeof(Sink<>).MakeGenericType(outputElementType).GetConstructors()[0]);
            sttCtorIlg.Emit(OpCodes.Ret);

            var dirty = stt.DefineField("dirty", typeof(bool), FieldAttributes.Public);

            var fldIndex = 0;
            FieldBuilder declareField(Type t) => stt.DefineField("state" + fldIndex++, t, FieldAttributes.Public);

#if HOIST
            var mtdOnNextIndex = 0;
            MethodBuilder declareOnNext(Type t) => stt.DefineMethod("OnNext" + mtdOnNextIndex++, MethodAttributes.Public, typeof(void), new[] { t });

            var mtdOnErrorIndex = 0;
            MethodBuilder declareOnError() => stt.DefineMethod("OnError" + mtdOnErrorIndex++, MethodAttributes.Public, typeof(void), new[] { typeof(Exception) });

            var mtdOnCompletedIndex = 0;
            MethodBuilder declareOnCompleted() => stt.DefineMethod("OnCompleted" + mtdOnCompletedIndex++, MethodAttributes.Public, typeof(void), Type.EmptyTypes);
#endif

            var methods = new Dictionary<MethodBuilder, LambdaExpression>();

            var ctorBlock = new List<Expression>();
            Action<Expression> appendToCtor = ctorBlock.Add;

            var stateParameter = Expression.Parameter(stt);

            var markDirty = Expression.Assign(Expression.Field(stateParameter, dirty), Expression.Constant(true));

            var result = Expression.Parameter(typeof(IObserver<>).MakeGenericType(outputElementType));
            var disposable = Expression.Parameter(typeof(IDisposable));

            var curOnNext = default(Func<Expression, Expression>);
            var curOnError = default(Func<Expression, Expression>);
            var curOnCompleted = default(Expression);

            foreach (var op in chain.Reverse())
            {
                op.Initialize(stateParameter, declareField, appendToCtor, result, disposable);

                var oldOnNext = curOnNext;
                var oldOnError = curOnError;
                var oldOnCompleted = curOnCompleted;

#if HOIST
                if ((op.Hoist & HoistOperations.OnNext) != 0)
                {
                    var onNext = declareOnNext(op.OutputType);
                    var onNextValue = Expression.Parameter(op.OutputType);
                    var onNextMtd = Expression.Lambda(oldOnNext(onNextValue), onNextValue);
                    methods[onNext] = onNextMtd;

                    // TODO: replace oldOnNext
                }

                if ((op.Hoist & HoistOperations.OnError) != 0)
                {
                    var onError = declareOnError();
                    var onErrorValue = Expression.Parameter(typeof(Exception));
                    var onErrorMtd = Expression.Lambda(oldOnError(onErrorValue), onErrorValue);
                    methods[onError] = onErrorMtd;

                    // TODO: replace oldOnError
                }

                if ((op.Hoist & HoistOperations.OnCompleted) != 0)
                {
                    var onCompleted = declareOnCompleted();
                    var onCompletedMtd = Expression.Lambda(oldOnCompleted);
                    methods[onCompleted] = onCompletedMtd;

                    // TODO: replace oldOnCompleted
                }
#endif

                curOnNext = op.CreateOnNext(oldOnNext, oldOnError, oldOnCompleted, stateParameter, markDirty);
                curOnError = op.CreateOnError(oldOnNext, oldOnError, oldOnCompleted, stateParameter, markDirty);
                curOnCompleted = op.CreateOnCompleted(oldOnNext, oldOnError, oldOnCompleted, stateParameter, markDirty);
            }


            var ivOnNextValue = Expression.Parameter(inputElementType);
            var ivOnNextBody = curOnNext(ivOnNextValue);

            var ivOnErrorValue = Expression.Parameter(typeof(Exception));
            var ivOnErrorBody = curOnError(ivOnErrorValue);

            var ivOnCompletedBody = curOnCompleted;

            var ivCtorBody = (Expression)Expression.Empty();
            if (ctorBlock.Count > 0)
            {
                ivCtorBody = Expression.Block(ctorBlock.Concat(new[] { ivCtorBody }));
            }


            var stateType = stt.CreateType();


            var stateParameterNew = Expression.Parameter(stateType);

            var subst = new CloseStateParameter(stateParameter, stateParameterNew);

            ivOnNextBody = subst.Visit(ivOnNextBody);
            ivOnErrorBody = subst.Visit(ivOnErrorBody);
            ivOnCompletedBody = subst.Visit(ivOnCompletedBody);
            ivCtorBody = subst.Visit(ivCtorBody);


            var ivOnNext = Expression.Lambda(ivOnNextBody, stateParameterNew, ivOnNextValue);
            var ivOnError = Expression.Lambda(ivOnErrorBody, stateParameterNew, ivOnErrorValue);
            var ivOnCompleted = Expression.Lambda(ivOnCompletedBody, stateParameterNew);
            var ivCtor = Expression.Lambda(ivCtorBody, stateParameterNew, result, disposable);


            var typ = mod.DefineType("<>__Fused01", TypeAttributes.Public, stateType, new[] { typeof(IObserver<>).MakeGenericType(inputElementType) });


            var ivOnNextMtdImpl = typ.DefineMethod("OnNextImpl", MethodAttributes.Private | MethodAttributes.Static, typeof(void), new[] { stateType, inputElementType });
            ivOnNextMtdImpl.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            ivOnNext.CompileToMethod(ivOnNextMtdImpl, dbg);

            var ivOnNextMtd = typ.DefineMethod("OnNext", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { inputElementType });
            var ivOnNextIlg = ivOnNextMtd.GetILGenerator();
            ivOnNextIlg.Emit(OpCodes.Ldarg_0);
            ivOnNextIlg.Emit(OpCodes.Ldarg_1);
            ivOnNextIlg.EmitCall(OpCodes.Call, ivOnNextMtdImpl, null);
            ivOnNextIlg.Emit(OpCodes.Ret);

            var ivOnErrorMtdImpl = typ.DefineMethod("OnErrorImpl", MethodAttributes.Private | MethodAttributes.Static, typeof(void), new[] { stateType, typeof(Exception) });
            ivOnErrorMtdImpl.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            ivOnError.CompileToMethod(ivOnErrorMtdImpl, dbg);

            var ivOnErrorMtd = typ.DefineMethod("OnError", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { typeof(Exception) });
            var ivOnErrorIlg = ivOnErrorMtd.GetILGenerator();
            ivOnErrorIlg.Emit(OpCodes.Ldarg_0);
            ivOnErrorIlg.Emit(OpCodes.Ldarg_1);
            ivOnErrorIlg.EmitCall(OpCodes.Call, ivOnErrorMtdImpl, null);
            ivOnErrorIlg.Emit(OpCodes.Ret);

            var ivOnCompletedMtdImpl = typ.DefineMethod("OnCompletedImpl", MethodAttributes.Private | MethodAttributes.Static, typeof(void), new[] { stateType });
            ivOnCompleted.CompileToMethod(ivOnCompletedMtdImpl, dbg);

            var ivOnCompletedMtd = typ.DefineMethod("OnCompleted", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), Type.EmptyTypes);
            ivOnCompletedMtdImpl.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var ivOnCompletedIlg = ivOnCompletedMtd.GetILGenerator();
            ivOnCompletedIlg.Emit(OpCodes.Ldarg_0);
            ivOnCompletedIlg.EmitCall(OpCodes.Call, ivOnCompletedMtdImpl, null);
            ivOnCompletedIlg.Emit(OpCodes.Ret);

            var ivCtorMtdImpl = typ.DefineMethod(".ctorImpl", MethodAttributes.Private | MethodAttributes.Static, typeof(void), new[] { stateType, result.Type, typeof(IDisposable) });
            ivCtor.CompileToMethod(ivCtorMtdImpl, dbg);

            var ivCtorMtd = typ.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { result.Type, typeof(IDisposable) });
            ivCtorMtdImpl.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var ivCtorIlg = ivCtorMtd.GetILGenerator();
            ivCtorIlg.Emit(OpCodes.Ldarg_0);
            ivCtorIlg.Emit(OpCodes.Ldarg_1);
            ivCtorIlg.Emit(OpCodes.Ldarg_2);
            ivCtorIlg.Emit(OpCodes.Call, stateType.GetConstructors()[0]);
            ivCtorIlg.Emit(OpCodes.Ldarg_0);
            ivCtorIlg.Emit(OpCodes.Ldarg_1);
            ivCtorIlg.Emit(OpCodes.Ldarg_2);
            ivCtorIlg.EmitCall(OpCodes.Call, ivCtorMtdImpl, null);
            ivCtorIlg.Emit(OpCodes.Ret);


            var ivt = typ.CreateType();

            asm.Save("Artifacts.dll");

            return ivt;
        }

        private static IEnumerable<IFusionOperator> Optimize(IFusionOperator[] chain)
        {
            // TODO: these optimizations can be moved to the expression space above the plan

            var cur = default(IFusionOperator);

            foreach (var op in chain)
            {
                if (cur != null)
                {
                    if (cur is WhereFactory where1 && op is WhereFactory where2)
                    {
                        var p1 = where1.Predicate;
                        var p2 = where2.Predicate;
                        var x = Expression.Parameter(cur.OutputType);
                        var p = Expression.Lambda(Expression.AndAlso(Expression.Invoke(p1, x), Expression.Invoke(p2, x)), x);
                        cur = new WhereFactory { Predicate = p };
                    }
                    else if (cur is TakeFactory take1 && op is TakeFactory take2)
                    {
                        // TODO: Count should be an expression (which could be constant, allowing for this optimization)
                        var n1 = take1.Count;
                        var n2 = take2.Count;
                        var n = Math.Min(n1, n2);
                        cur = new TakeFactory { Count = n };
                    }
                    else if (cur is SelectFactory select1 && op is SelectFactory select2)
                    {
                        var s1 = select1.Selector;
                        var s2 = select2.Selector;
                        var p = Expression.Parameter(s1.Parameters[0].Type);
                        var s = Expression.Lambda(Expression.Invoke(s2, Expression.Invoke(s1, p)), p);
                        cur = new SelectFactory { Selector = s };
                    }
                    else
                    {
                        yield return cur;
                        cur = op;
                    }
                }
                else
                {
                    cur = op;
                }
            }

            if (cur != null)
            {
                yield return cur;
            }
        }
    }
}
