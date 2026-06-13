// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

using Nuqleon.Serialization;

namespace Nuqleon.Linq.Expressions.Serialization
{
    using CSharp = Microsoft.CSharp.RuntimeBinder;
    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Rule table for dynamic expression support using the Microsoft.CSharp binders.
    /// </summary>
    internal class CSharpDynamicRuleTable : JsonRuleTable<ExpressionJsonSerializationContext>
    {
        #region Singleton pattern

        /// <summary>
        /// Single instance creation lock.
        /// </summary>
        private static readonly object s_singletonLock = new();

        /// <summary>
        /// Singleton instance for the default, read-only expression tree serialization rule table.
        /// </summary>
        private static volatile RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> s_instance;

        /// <summary>
        /// Gets the singleton instance for the default, read-only expression tree serialization rule table.
        /// </summary>
        public static RuleTableBase<object, Json.Expression, ExpressionJsonSerializationContext> Instance
        {
            get
            {
                //
                // Double-locking pattern to ensure single initialization even in the face of multiple
                // threads racing for initialization.
                //
                if (s_instance == null)
                {
                    lock (s_singletonLock)
                    {
                        s_instance ??= new CSharpDynamicRuleTable();
                    }
                }

                return s_instance;
            }
        }

        /// <summary>
        /// Creates an rule table with default rules for expression tree serialization.
        /// </summary>
        protected CSharpDynamicRuleTable()
        {
            //
            // Initialize rules.
            //
            AddDynamicExpressionRules();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds rules for dynamic expressions.
        /// </summary>
        private void AddDynamicExpressionRules()
        {
            var isCSharpBinder = new Func<CallSiteBinder, bool>(cb => cb.GetType().Assembly == typeof(CSharp.Binder).Assembly);
            var getPrivateProperty = new Func<Type, object, string, object>((type, obj, name) => type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(obj));

            Add<System.Dynamic.BinaryOperationBinder>(
                "CSharpBinaryOperationBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();

                    var isChecked = (bool)getPrivateProperty(t, cb, "IsChecked");
                    var isLogicalOperation = (bool)getPrivateProperty(t, cb, "IsLogicalOperation");

                    var flags = CSharp.CSharpBinderFlags.None
                        | (isChecked ? CSharp.CSharpBinderFlags.CheckedContext : 0)
                        | (isLogicalOperation ? CSharp.CSharpBinderFlags.BinaryOperationLogical : 0);
                    var operation = cb.Operation;
                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(t, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(operation, ctx),
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var operation = (ExpressionType)args[1];
                    var context = (Type)args[2];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[3];

                    return (System.Dynamic.BinaryOperationBinder)CSharp.Binder.BinaryOperation(flags, operation, context, argumentInfo);
                }
            );
            Add<System.Dynamic.ConvertBinder>(
                "CSharpConvertBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    const int ArrayCreationConversion = 2;

                    var t = cb.GetType();

                    var conversionKind = (int)getPrivateProperty(t, cb, "ConversionKind");
                    var isChecked = (bool)getPrivateProperty(t, cb, "IsChecked");
                    var @explicit = cb.Explicit;

                    var flags = CSharp.CSharpBinderFlags.None
                        | (isChecked ? CSharp.CSharpBinderFlags.CheckedContext : 0)
                        | (@explicit ? CSharp.CSharpBinderFlags.ConvertExplicit : 0)
                        | (conversionKind == ArrayCreationConversion ? CSharp.CSharpBinderFlags.ConvertArrayIndex : 0);
                    var type = cb.Type;
                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(type, ctx),
                        serialize(context, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var type = (Type)args[1];
                    var context = (Type)args[2];

                    return (System.Dynamic.ConvertBinder)CSharp.Binder.Convert(flags, type, context);
                }
            );
            Add<System.Dynamic.GetIndexBinder>(
                "CSharpGetIndexBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();

                    var flags = CSharp.CSharpBinderFlags.None;
                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(t, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var context = (Type)args[1];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[2];

                    return (System.Dynamic.GetIndexBinder)CSharp.Binder.GetIndex(flags, context, argumentInfo);
                }
            );
            Add<System.Dynamic.GetMemberBinder>(
                "CSharpGetMemberBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();

                    var resultIndexed = (bool)getPrivateProperty(t, cb, "ResultIndexed");

                    var flags = resultIndexed ? CSharp.CSharpBinderFlags.ResultIndexed : CSharp.CSharpBinderFlags.None;
                    var name = cb.Name;
                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(t, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(name, ctx),
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var name = (string)args[1];
                    var context = (Type)args[2];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[3];

                    return (System.Dynamic.GetMemberBinder)CSharp.Binder.GetMember(flags, name, context, argumentInfo);
                }
            );
            Add<System.Dynamic.InvokeBinder>(
                "CSharpInvokeBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();
                    var i = t.GetInterface("ICSharpInvokeOrInvokeMemberBinder");

                    var resultDiscarded = (bool)getPrivateProperty(i, cb, "ResultDiscarded");

                    var flags = resultDiscarded ? CSharp.CSharpBinderFlags.ResultDiscarded : CSharp.CSharpBinderFlags.None;
                    var context = (Type)getPrivateProperty(i, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(i, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var context = (Type)args[1];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[2];

                    return (System.Dynamic.InvokeBinder)CSharp.Binder.Invoke(flags, context, argumentInfo);
                }
            );
            Add<System.Dynamic.DynamicMetaObjectBinder>(
                "CSharpInvokeConstructorBinder", b => isCSharpBinder(b) && b.GetType().Name == "CSharpInvokeConstructorBinder",
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();

                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(t, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = CSharp.CSharpBinderFlags.None; // ignored by InvokeConstructor
                    var context = (Type)args[0];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[1];

                    return (System.Dynamic.DynamicMetaObjectBinder)CSharp.Binder.InvokeConstructor(flags, context, argumentInfo);
                }
            );
            Add<System.Dynamic.InvokeMemberBinder>(
                "CSharpInvokeMemberBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    const int SimpleNameCall = 1;
                    const int EventHookup = 2;
                    const int ResultDiscarded = 4;

                    var t = cb.GetType();
                    var i = t.GetInterface("ICSharpInvokeOrInvokeMemberBinder");

                    var allFlags = (int)getPrivateProperty(i, cb, "Flags");

                    var flags = CSharp.CSharpBinderFlags.None
                        | ((allFlags & SimpleNameCall) != 0 ? CSharp.CSharpBinderFlags.InvokeSimpleName : 0)
                        | ((allFlags & EventHookup) != 0 ? CSharp.CSharpBinderFlags.InvokeSpecialName : 0)
                        | ((allFlags & ResultDiscarded) != 0 ? CSharp.CSharpBinderFlags.ResultDiscarded : 0);
                    var name = cb.Name;
                    var typeArguments = (IEnumerable<Type>)getPrivateProperty(i, cb, "TypeArguments");
                    var context = (Type)getPrivateProperty(i, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(i, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(name, ctx),
                        serialize(typeArguments, ctx),
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var name = (string)args[1];
                    var typeArguments = (IEnumerable<Type>)args[2];
                    var context = (Type)args[3];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[4];

                    return (System.Dynamic.InvokeMemberBinder)CSharp.Binder.InvokeMember(flags, name, typeArguments, context, argumentInfo);
                }
            );
            Add<System.Dynamic.DynamicMetaObjectBinder>(
                "CSharpIsEventBinder", b => isCSharpBinder(b) && b.GetType().Name == "CSharpIsEventBinder",
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();

                    var name = (string)getPrivateProperty(t, cb, "Name");
                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");

                    return Json.Expression.Array(
                        serialize(name, ctx),
                        serialize(context, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = CSharp.CSharpBinderFlags.None; // ignored by IsEvent
                    var name = (string)args[0];
                    var context = (Type)args[1];

                    return (System.Dynamic.DynamicMetaObjectBinder)CSharp.Binder.IsEvent(flags, name, context);
                }
            );
            Add<System.Dynamic.SetIndexBinder>(
                "CSharpSetIndexBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();

                    var isChecked = (bool)getPrivateProperty(t, cb, "IsChecked");
                    var isCompoundAssignment = (bool)getPrivateProperty(t, cb, "IsCompoundAssignment");

                    var flags = CSharp.CSharpBinderFlags.None
                        | (isChecked ? CSharp.CSharpBinderFlags.CheckedContext : 0)
                        | (isCompoundAssignment ? CSharp.CSharpBinderFlags.ValueFromCompoundAssignment : 0);
                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(t, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var context = (Type)args[1];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[2];

                    return (System.Dynamic.SetIndexBinder)CSharp.Binder.SetIndex(flags, context, argumentInfo);
                }
            );
            Add<System.Dynamic.SetMemberBinder>(
                "CSharpSetMemberBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();

                    var isChecked = (bool)getPrivateProperty(t, cb, "IsChecked");
                    var isCompoundAssignment = (bool)getPrivateProperty(t, cb, "IsCompoundAssignment");

                    var flags = CSharp.CSharpBinderFlags.None
                        | (isChecked ? CSharp.CSharpBinderFlags.CheckedContext : 0)
                        | (isCompoundAssignment ? CSharp.CSharpBinderFlags.ValueFromCompoundAssignment : 0);
                    var name = cb.Name;
                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(t, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(name, ctx),
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var name = (string)args[1];
                    var context = (Type)args[2];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[3];

                    return (System.Dynamic.SetMemberBinder)CSharp.Binder.SetMember(flags, name, context, argumentInfo);
                }
            );
            Add<System.Dynamic.UnaryOperationBinder>(
                "CSharpUnaryOperationBinder", isCSharpBinder,
                (cb, serialize, ctx) =>
                {
                    var t = cb.GetType();

                    var isChecked = (bool)getPrivateProperty(t, cb, "IsChecked");

                    var flags = CSharp.CSharpBinderFlags.None
                        | (isChecked ? CSharp.CSharpBinderFlags.CheckedContext : 0);
                    var operation = cb.Operation;
                    var context = (Type)getPrivateProperty(t, cb, "CallingContext");
                    var argumentInfo = (IList<CSharp.CSharpArgumentInfo>)getPrivateProperty(t, cb, "ArgumentInfo");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(operation, ctx),
                        serialize(context, ctx),
                        serialize(argumentInfo, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpBinderFlags)args[0];
                    var operation = (ExpressionType)args[1];
                    var context = (Type)args[2];
                    var argumentInfo = (IEnumerable<CSharp.CSharpArgumentInfo>)args[3];

                    return (System.Dynamic.UnaryOperationBinder)CSharp.Binder.UnaryOperation(flags, operation, context, argumentInfo);
                }
            );

            Add(
                "CSharpArgumentInfos", default(IList<CSharp.CSharpArgumentInfo>),
                (cs, serialize, ctx) => Json.Expression.Array(from c in cs select serialize(c, ctx)),
                (jo, deserialize, ctx) => (from e in ((Json.ArrayExpression)jo).Elements select (CSharp.CSharpArgumentInfo)deserialize(e, ctx)).ToList()
            );
            Add<CSharp.CSharpArgumentInfo>(
                "CSharpArgumentInfo",
                (ai, serialize, ctx) =>
                {
                    var t = ai.GetType();

                    var flags = (CSharp.CSharpArgumentInfoFlags)getPrivateProperty(t, ai, "Flags");
                    var name = (string)getPrivateProperty(t, ai, "Name");

                    return Json.Expression.Array(
                        serialize(flags, ctx),
                        serialize(name, ctx)
                    );
                },
                (jo, deserialize, ctx) =>
                {
                    var args = ((Json.ArrayExpression)jo).Elements.Select(e => deserialize(e, ctx)).ToArray();

                    var flags = (CSharp.CSharpArgumentInfoFlags)args[0];
                    var name = (string)args[1];

                    return CSharp.CSharpArgumentInfo.Create(flags, name);
                }
            );
        }

        #endregion
    }
}
